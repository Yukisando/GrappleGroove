#region

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

#endregion

namespace PrototypeFPC
{
    public enum RopeType
    {
        BOTH,
        LEFT,
        RIGHT,
    }
    
    public class GrapplingHook : MonoBehaviour
    {
        // Hook properties
        [Header("Assignments")]
        [SerializeField] KeyCode cutRopeKey;
        [SerializeField] KeyCode resetHookKey;
        [SerializeField] public LayerMask grappleLayerMask;
        [SerializeField] public LayerMask ropeLayerMask;
        [SerializeField] GameObject hookModel;
        [SerializeField] GameObject platformPrefab;
        
        [Header("Settings")]
        [SerializeField] float minimumRopeLength = 1f;
        [SerializeField] float releaseImpulseFactor = 50f;
        [SerializeField] float holdDelayToSwing = 0.2f;
        [SerializeField] float connectionSpringStrength = 10000f;
        [SerializeField] float connectionDamperStrength = 1000f;
        [SerializeField] float retractAmount = .1f;
        [SerializeField] float latchOnImpulse = 200f;
        public float hookDistance = 50f;
        
        // Rope properties
        [Header("Rope Properties")]
        [SerializeField] Material leftRopeMaterial;
        [SerializeField] Material rightRopeMaterial;
        [SerializeField] float startThickness = 0.02f;
        [SerializeField] float endThickness = 0.06f;
        [SerializeField] int maxRopes = 2; // Maximum number of ropes allowed
        
        // Rope visual spring properties
        [Header("Rope Visual Spring Properties")]
        [SerializeField] int segments = 50;
        [SerializeField] float damper = 12;
        [SerializeField] float springStrength = 800;
        [SerializeField] float speed = 12;
        [SerializeField] float waveCount = 5;
        [SerializeField] float waveHeight = 4;
        [SerializeField] AnimationCurve affectCurve;
        [SerializeField] List<Rope> ropes = new List<Rope>();
        
        // Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip grapplingSound;
        [SerializeField] AudioClip releaseSound;
        [SerializeField] AudioClip pushClip;
        [SerializeField] AudioClip pullClip;
        
        AudioSource audioSource;
        
        bool executeHookSwing;
        RaycastHit hit;
        bool hooked;
        bool hookRelease;
        float mouseDownTimer;
        
        PlayerDependencies playerDependencies;
        Ray ray;
        Rigidbody rb;
        
        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }
        
        void Start() {
            rb = playerDependencies.rb;
            audioSource = playerDependencies.audioSourceTop;
        }
        
        void Update() {
            InputCheck();
            HandleRopeLength();
            
            if (playerDependencies.isInspecting && playerDependencies.isGrabbing) return;
            CreateHooks(0);
            CreateHooks(1);
            CutRopes();
        }
        
        void LateUpdate() {
            DrawRopes();
        }
        
        void InputCheck() {
            // Reset checker
            if ((Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) && !Input.GetKey(KeyCode.LeftControl) && !playerDependencies.isInspecting) {
                mouseDownTimer = 0;
                hookRelease = false;
                executeHookSwing = false;
            }
            
            // Check input for hook to swing
            if ((Input.GetMouseButton(1) || Input.GetMouseButton(0)) && !Input.GetKey(KeyCode.LeftControl) && !playerDependencies.isInspecting) {
                mouseDownTimer += Time.deltaTime;
                
                if (hooked && mouseDownTimer >= holdDelayToSwing && !executeHookSwing) executeHookSwing = true;
            }
            
            // Check input for hook to latch
            if ((Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0)) && !Input.GetKey(KeyCode.LeftControl) && mouseDownTimer >= holdDelayToSwing && executeHookSwing && !playerDependencies.isInspecting) {
                executeHookSwing = false;
                hookRelease = true;
                
                // Get the player's current velocity
                var playerVelocity = rb.linearVelocity;
                float speedFactor = playerVelocity.magnitude;
                
                // Apply an impulse based on the speed at release
                var releaseImpulse = playerVelocity.normalized * (speedFactor * releaseImpulseFactor);
                rb.AddForce(releaseImpulse, ForceMode.Impulse);
            }
        }
        
        void CreateHooks(int _mouseButton) {
            if (Input.GetMouseButtonDown(_mouseButton) && !Input.GetKey(KeyCode.LeftControl) && !playerDependencies.isInspecting) {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                // Perform a raycast to check if the first hit is on the grapple layer
                if (Physics.Raycast(ray.origin, ray.direction, out hit, hookDistance, grappleLayerMask, QueryTriggerInteraction.Ignore)) {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Default") || hit.transform.gameObject.layer == LayerMask.NameToLayer("Static")) return;
                    
                    // Check if the hit object has a Rigidbody, if not, add one and set it to kinematic
                    if (hit.transform.gameObject.GetComponent<Rigidbody>() == null) {
                        hit.transform.gameObject.AddComponent<Rigidbody>().isKinematic = true;
                    }
                    
                    // Create the first hook if not already hooked
                    if (!hooked) {
                        if (hit.collider.isTrigger || hit.collider.gameObject.GetComponent<Rigidbody>() == rb) return;
                        hooked = true;
                        CreateHook(_mouseButton, hit);
                    }
                    
                    // Create hook latch if already hooked
                    else if (hooked) {
                        if (!Physics.Raycast(ray.origin, ray.direction, out hit, hookDistance, grappleLayerMask, QueryTriggerInteraction.Ignore)) return;
                        if (hit.collider.isTrigger || hit.collider.gameObject.GetComponent<Rigidbody>() == rb) return;
                        CreateHookLatch(hit);
                    }
                }
            }
        }
        
        // Inside the CreateHook method
        void CreateHook(int _mouseButton, RaycastHit _hit) {
            // Create new rope
            var rope = new Rope {
                hook = new GameObject("Hook") {
                    transform = {
                        position = _hit.point,
                        parent = _hit.transform,
                    },
                },
                type = _mouseButton == 0 ? RopeType.LEFT : RopeType.RIGHT,
                connectedObject1 = _hit.transform.gameObject, // Keep track of the first connected object
            };
            
            // Add Rigidbody to hook
            var hookRB = rope.hook.AddComponent<Rigidbody>();
            hookRB.useGravity = false;
            
            // Hook end point model
            rope.hookModels.Add(Instantiate(hookModel, rope.hook.transform.position, Quaternion.identity));
            rope.hookModels[^1].transform.parent = rope.hook.transform;
            
            // Hook start point model
            var spawnPoint = _mouseButton == 0 ? playerDependencies.spawnPointLeft.position : playerDependencies.spawnPointRight.position;
            rope.hookModels.Add(Instantiate(hookModel, spawnPoint, Quaternion.identity));
            rope.hookModels[^1].transform.parent = _mouseButton == 0 ? playerDependencies.spawnPointLeft.transform : playerDependencies.spawnPointRight.transform;
            
            // Set hook rope values
            rope.lineRenderer = rope.hook.AddComponent<LineRenderer>();
            rope.lineRenderer.material = new Material(_mouseButton == 0 ? leftRopeMaterial : rightRopeMaterial);
            rope.lineRenderer.startWidth = startThickness;
            rope.lineRenderer.endWidth = endThickness;
            rope.lineRenderer.numCornerVertices = 2;
            rope.lineRenderer.numCapVertices = 10;
            rope.lineRenderer.textureMode = LineTextureMode.Tile;
            rope.lineRenderer.shadowCastingMode = ShadowCastingMode.On;
            rope.lineRenderer.receiveShadows = false;
            
            rope.lineRenderer.positionCount = segments + 1;
            
            // Initialize spring for the new rope
            rope.spring = new Spring();
            rope.spring.SetTarget(0);
            rope.spring.SetDamper(damper);
            rope.spring.SetStrength(springStrength);
            
            rb.gameObject.AddComponent<SpringJoint>().connectedBody = rope.hook.GetComponent<Rigidbody>();
            rope.hook.AddComponent<FixedJoint>().connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();
            
            var sj = rb.GetComponent<SpringJoint>();
            sj.autoConfigureConnectedAnchor = false;
            sj.connectedAnchor = Vector3.zero;
            
            // Calculate the distance between the player and the hook point
            float distanceFromHook = Vector3.Distance(rb.gameObject.transform.position, rope.hook.transform.position);
            
            // Set the maxDistance and minDistance to the initial distance from the hook point
            sj.maxDistance = distanceFromHook;
            sj.minDistance = distanceFromHook;
            
            sj.spring = 20000f; // Increase spring strength to make it tighter
            sj.damper = 10000f; // Adjust damper to control oscillation
            
            // Add collider for rope cutting
            rope.ropeCollider = new GameObject("RopeCollider") {
                transform = {
                    parent = rope.hook.transform,
                },
            };
            rope.ropeCollider.AddComponent<BoxCollider>().size = new Vector3(0.1f, 0, 0.1f);
            rope.ropeCollider.GetComponent<BoxCollider>().isTrigger = true;
            rope.ropeCollider.GetComponent<BoxCollider>().enabled = false;
            
            // Knock back when hooked
            rope.hook.GetComponent<Rigidbody>().AddForce(ray.direction * (latchOnImpulse * 0.2f), ForceMode.Impulse);
            
            // Audio
            audioSource.PlayOneShot(grapplingSound);
            
            ropes.Add(rope);
        }
        
        // Inside the CreateHookLatch method
        void CreateHookLatch(RaycastHit _hit) {
            // Get the last rope
            var rope = ropes[^1];
            
            // Create new hook latch object
            rope.hookLatch = new GameObject("HookLatch") {
                transform = {
                    position = _hit.point,
                    parent = _hit.transform,
                },
            };
            
            // Add Rigidbody to hook latch
            var hlrb = rope.hookLatch.AddComponent<Rigidbody>();
            hlrb.useGravity = false;
            
            // Remove hook start point model
            Destroy(rope.hookModels[^1].gameObject);
            rope.hookModels.RemoveAt(rope.hookModels.Count - 1);
            
            // Add hook latch point model
            rope.hookModels.Add(Instantiate(hookModel, rope.hookLatch.transform.position, Quaternion.identity));
            rope.hookModels[^1].transform.parent = rope.hookLatch.transform;
            
            // Reset and initialize the spring for the latch
            rope.spring.Reset();
            rope.spring.SetVelocity(speed);
            
            rope.hookLatch.AddComponent<FixedJoint>().connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();
            rope.hookLatch.transform.parent = hit.transform;
            
            Destroy(rb.GetComponent<SpringJoint>());
            rope.hook.AddComponent<SpringJoint>().connectedBody = rope.hookLatch.GetComponent<Rigidbody>();
            var hsj = rope.hook.GetComponent<SpringJoint>();
            hsj.autoConfigureConnectedAnchor = false;
            hsj.anchor = Vector3.zero;
            hsj.connectedAnchor = Vector3.zero;
            hsj.spring = connectionSpringStrength;
            hsj.damper = connectionDamperStrength;
            
            // Calculate the distance between the hooks
            float ropeLength = Vector3.Distance(rope.hook.transform.position, _hit.point);
            
            // Set the maxDistance and minDistance to the initial distance between the hooks
            hsj.maxDistance = ropeLength;
            hsj.minDistance = ropeLength;
            
            // Knock back when hooked
            rope.hookLatch.GetComponent<Rigidbody>().AddForce(ray.direction * (latchOnImpulse * 0.2f), ForceMode.Impulse);
            
            // Set rope width
            rope.lineRenderer.startWidth = endThickness;
            rope.lineRenderer.endWidth = endThickness;
            
            // Enable rope collider
            rope.ropeCollider.GetComponent<BoxCollider>().enabled = true;
            hooked = false;
            
            // Check if the rope is too short
            if (ropeLength < minimumRopeLength) {
                DestroyRope(ropes.Count - 1);
                audioSource.PlayOneShot(releaseSound);
                return; // Exit the method as the rope is too short
            }
            
            // Set the second connected object
            rope.connectedObject2 = _hit.transform.gameObject;
            
            // Audio
            audioSource.PlayOneShot(grapplingSound);
            
            // Instantiate the plank as a child of the hook latch
            rope.plank = Instantiate(platformPrefab, rope.hook.transform.position, Quaternion.identity);
            rope.plank.transform.parent = null; // Remove parent to avoid inheriting transformations
            
            // Adjust the scale and position of the plank
            var startPoint = rope.hook.transform.position;
            var endPoint = _hit.point;
            var midPoint = (startPoint + endPoint) / 2;
            rope.plank.transform.position = midPoint;
            
            float distance = Vector3.Distance(startPoint, endPoint);
            rope.plank.transform.localScale = new Vector3(distance, rope.plank.transform.localScale.y, rope.plank.transform.localScale.z);
            
            // Adjust the rotation of the plank
            rope.plank.transform.rotation = Quaternion.LookRotation(endPoint - startPoint);
            rope.plank.transform.Rotate(0, 90, 0); // Ensure the plank is perpendicular to the normal of the hooks
            
            // Check the limit of ropes after adding the new one
            var leftRopes = ropes.FindAll(_r => _r.type == RopeType.LEFT);
            var rightRopes = ropes.FindAll(_r => _r.type == RopeType.RIGHT);
            if (leftRopes.Count > maxRopes) DestroyRope(leftRopes.IndexOf(leftRopes.FirstOrDefault()));
            if (rightRopes.Count > maxRopes) DestroyRope(rightRopes.IndexOf(rightRopes.FirstOrDefault()));
        }
        
        void HandleRopeLength() {
            if (Input.mouseScrollDelta.y < 0) {
                RetractRopes();
                audioSource.PlayOneShot(pullClip);
            }
            else if (Input.mouseScrollDelta.y > 0) {
                ExtendRopes();
                audioSource.PlayOneShot(pushClip);
            }
        }
        
        void RetractRopes() {
            foreach (var rope in ropes) {
                if (rope.hook.GetComponent<SpringJoint>() != null) {
                    var sj = rope.hook.GetComponent<SpringJoint>();
                    sj.maxDistance = Mathf.Max(sj.maxDistance - retractAmount, minimumRopeLength);
                    sj.minDistance = Mathf.Max(sj.minDistance - retractAmount, 0f);
                }
            }
        }
        
        void ExtendRopes() {
            foreach (var rope in ropes) {
                if (rope.hook.GetComponent<SpringJoint>() != null) {
                    var sj = rope.hook.GetComponent<SpringJoint>();
                    sj.maxDistance += retractAmount;
                    sj.minDistance += retractAmount;
                }
            }
        }
        
        void CutRopes() {
            // Destroy player hooks upon hold release
            if ((Input.GetKey(cutRopeKey) || hookRelease) && hooked) {
                hookRelease = false;
                DestroyGrappleRope();
                return;
            }
            
            // Remove specific hooks
            if (Input.GetKey(cutRopeKey)) {
                // Perform a raycast to check if it hits a rope collider
                var r = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(r.origin, r.direction, out hit, hookDistance, ropeLayerMask)) {
                    // Get the rope index from the hit collider
                    int ropeIndex = GameObjectToIndex(hit.collider.gameObject);
                    if (ropeIndex != -1) {
                        DestroyRope(ropeIndex);
                    }
                }
            }
            
            // Destroy everything created and clear all lists
            if (Input.GetKeyDown(resetHookKey) && !playerDependencies.isInspecting) DestroyRopes();
        }
        
        int GameObjectToIndex(GameObject ropeColliderObject) {
            for (int i = 0; i < ropes.Count; i++) {
                if (ropes[i].ropeCollider == ropeColliderObject) {
                    return i;
                }
            }
            return -1;
        }
        
        void DestroyGrappleRope() {
            if (ropes.Count > 0) {
                Destroy(rb.GetComponent<SpringJoint>());
                DestroyRope(ropes.Count - 1);
            }
            
            hooked = false;
            hookRelease = false;
            audioSource.PlayOneShot(releaseSound);
        }
        
        void DestroyRope(int index) {
            var rope = ropes[index];
            Destroy(rope.hook.gameObject);
            if (rope.hookLatch != null) Destroy(rope.hookLatch.gameObject);
            if (rope.ropeCollider != null) Destroy(rope.ropeCollider.gameObject);
            if (rope.plank != null) Destroy(rope.plank); // Destroy the plank
            foreach (var model in rope.hookModels) {
                Destroy(model);
            }
            
            ropes.RemoveAt(index);
            
            if (ropes.Count == 0) hooked = false;
            audioSource.PlayOneShot(releaseSound);
        }
        
        public void DestroyRopes(RopeType _ropeType = RopeType.BOTH) {
            // Destroy matching player grapple
            if (hooked && (ropes.Last().type == _ropeType || _ropeType == RopeType.BOTH)) DestroyGrappleRope();
            
            var ropesToRemove = new List<Rope>();
            
            // Destroys matching ropes
            foreach (var rope in ropes.Where(_rope => _ropeType == RopeType.BOTH || _rope.type == _ropeType)) {
                Destroy(rope.hook.gameObject);
                if (rope.hookLatch != null) Destroy(rope.hookLatch.gameObject);
                if (rope.ropeCollider != null) Destroy(rope.ropeCollider.gameObject);
                if (rope.plank != null) Destroy(rope.plank); // Destroy the plank
                foreach (var model in rope.hookModels) {
                    Destroy(model);
                    ropesToRemove.Add(rope);
                }
            }
            
            if (ropesToRemove.Count > 0) {
                foreach (var rope in ropesToRemove) {
                    ropes.Remove(rope);
                }
                ropesToRemove.Clear();
                audioSource.PlayOneShot(releaseSound);
            }
        }
        
        void DrawRopes() {
            foreach (var rope in ropes) {
                rope.spring.Update(Time.fixedDeltaTime); // Update the spring value for each rope individually
                
                Vector3 startPoint;
                Vector3 endPoint;
                
                if (rb.GetComponent<SpringJoint>() != null && rb.GetComponent<SpringJoint>().connectedBody == rope.hook.GetComponent<Rigidbody>()) {
                    startPoint = rope.type == RopeType.LEFT ? playerDependencies.spawnPointLeft.position : playerDependencies.spawnPointRight.position;
                    endPoint = rope.hook.transform.position;
                }
                else if (rope.hook.GetComponent<SpringJoint>() != null && rope.hook.GetComponent<SpringJoint>().connectedBody != rb) {
                    startPoint = rope.hook.transform.position;
                    endPoint = rope.hookLatch.transform.position;
                }
                else
                    continue;
                
                // Ensure the LineRenderer has the correct number of positions
                if (rope.lineRenderer.positionCount != segments + 1) rope.lineRenderer.positionCount = segments + 1;
                
                var up = Quaternion.LookRotation((endPoint - startPoint).normalized) * Vector3.up;
                var right = Quaternion.LookRotation((endPoint - startPoint).normalized) * Vector3.right;
                
                for (int t = 0; t < segments + 1; t++) {
                    float delta = t / (float)segments;
                    var offset = up * (waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * rope.spring.Value * affectCurve.Evaluate(delta))
                                 + right * (waveHeight * Mathf.Cos(delta * waveCount * Mathf.PI) * rope.spring.Value * affectCurve.Evaluate(delta));
                    rope.lineRenderer.SetPosition(t, Vector3.Lerp(startPoint, endPoint, delta) + offset);
                }
                
                // Set rope collider size and position
                if (rope.ropeCollider != null && rope.hook.GetComponent<SpringJoint>() != null) {
                    rope.ropeCollider.transform.position = startPoint;
                    rope.ropeCollider.transform.LookAt(endPoint);
                    rope.ropeCollider.GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, Vector3.Distance(startPoint, endPoint));
                    float worldZCenter = Vector3.Distance(endPoint, startPoint) / 2;
                    rope.ropeCollider.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, worldZCenter);
                    
                    // Set the rope collider layer to the rope layer
                    rope.ropeCollider.layer = LayerMask.NameToLayer("Rope");
                    rope.ropeCollider.tag = "Rope";
                }
                
                // Update plank position, scale, and rotation
                if (rope.plank != null) {
                    var midPoint = (startPoint + endPoint) / 2;
                    rope.plank.transform.position = midPoint;
                    
                    float distance = Vector3.Distance(startPoint, endPoint);
                    rope.plank.transform.localScale = new Vector3(distance, rope.plank.transform.localScale.y, rope.plank.transform.localScale.z);
                    
                    // Ensure the plank faces up
                    rope.plank.transform.rotation = Quaternion.LookRotation(endPoint - startPoint);
                    rope.plank.transform.Rotate(0, 90, 0); // Adjust rotation so the plank faces up
                }
            }
        }
        
        [Serializable]
        public class Rope
        {
            public RopeType type = RopeType.LEFT;
            public GameObject hook;
            public GameObject hookLatch;
            public GameObject ropeCollider;
            public LineRenderer lineRenderer;
            public List<GameObject> hookModels = new List<GameObject>();
            public GameObject connectedObject1;
            public GameObject connectedObject2;
            public GameObject plank;
            public Spring spring;
        }
    }
}