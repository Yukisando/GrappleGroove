#region

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

#endregion

namespace PrototypeFPC
{
    public class GrapplingHook : MonoBehaviour
    {
        // Hook properties
        [Header("Hook Properties")]
        [SerializeField] public LayerMask grappleLayerMask;
        [SerializeField] public LayerMask ropeLayerMask;
        [SerializeField] GameObject hookModel;
        [SerializeField] GameObject platformPrefab;
        public float hookDistance = 50f;
        [SerializeField] KeyCode cutRopeKey;
        [SerializeField] KeyCode resetHookKey;
        [SerializeField] float minimumRopeLength = 1f;
        [SerializeField] float releaseImpulseFactor = 50f;
        [SerializeField] float holdDelayToSwing = 0.2f;
        [SerializeField] float playerRetractStrength = 1000f;
        [SerializeField] float retractStrength = 500f;
        [SerializeField] float latchOnImpulse = 200f;

        // Rope properties
        [Header("Rope Properties")]
        [SerializeField] Material leftRopeMaterial;
        [SerializeField] Material rightRopeMaterial;
        [SerializeField] float startThickness = 0.02f;
        [SerializeField] float endThickness = 0.06f;

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
        [SerializeField] AudioClip retractSound;

        // PlayerDependencies

        [Header("PlayerDependencies")] public PlayerDependencies playerDependencies;

        AudioSource audioSource;

        bool executeHookSwing;
        RaycastHit hit;
        bool hooked;
        bool hookRelease;

        float mouseDownTimer;
        Ray ray;
        Rigidbody rb;

        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }

        void Start() {
            Setup();
        }

        void Update() {
            InputCheck();
            CreateHooks(0);
            CreateHooks(1);
            RetractHooks();
            CutRopes();
        }

        void FixedUpdate() {
            DrawRopes();
        }

        void Setup() {
            // Setup playerDependencies
            rb = playerDependencies.rb;
            audioSource = playerDependencies.audioSourceTop;
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

                // Check and set target rigidbody if none
                if (Physics.Raycast(ray.origin, ray.direction, out hit, hookDistance, grappleLayerMask, QueryTriggerInteraction.Ignore))
                    if (hit.transform.gameObject.GetComponent<Rigidbody>() == null)
                        hit.transform.gameObject.AddComponent<Rigidbody>().isKinematic = true;

                // Create first hook
                if (!hooked) {
                    if (!Physics.Raycast(ray.origin, ray.direction, out hit, hookDistance, grappleLayerMask, QueryTriggerInteraction.Ignore)) return;
                    if (hit.collider.isTrigger || hit.collider.gameObject.GetComponent<Rigidbody>() == rb) return;
                    hooked = true;

                    CreateHook(_mouseButton, hit.point);
                }
                else if (hooked) {
                    if (!Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, grappleLayerMask, QueryTriggerInteraction.Ignore)) return;
                    if (hit.collider.isTrigger || hit.collider.gameObject.GetComponent<Rigidbody>() == rb) return;

                    CreateHookLatch(hit.point);
                }
            }
        }

        void CreateHook(int _mouseButton, Vector3 _position) {
            // Create new rope
            var rope = new Rope {
                hook = new GameObject("Hook") {
                    transform = {
                        position = _position,
                    },
                },
                type = _mouseButton == 0 ? RopeType.LEFT : RopeType.RIGHT,
            };

            // Add Rigidbody to hook
            var hookRb = rope.hook.AddComponent<Rigidbody>();
            hookRb.isKinematic = true;

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
            var sj = rb.GetComponent<SpringJoint>();
            sj.autoConfigureConnectedAnchor = false;
            sj.connectedAnchor = Vector3.zero;

            // Calculate the distance between the player and the hook point
            float distanceFromHook = Vector3.Distance(rb.gameObject.transform.position, rope.hook.transform.position);

            // Set the maxDistance and minDistance to the initial distance from the hook point
            sj.maxDistance = _mouseButton == 0 ? distanceFromHook : distanceFromHook * 3f;
            sj.minDistance = _mouseButton == 0 ? distanceFromHook * .025f : distanceFromHook * 0.95f;

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

        void CreateHookLatch(Vector3 position) {
            // Get the last rope
            var rope = ropes[^1];

            // Create new hook latch object
            rope.hookLatch = new GameObject("HookLatch") {
                transform = {
                    position = position,
                },
            };

            // Add Rigidbody to hook latch
            var latchRb = rope.hookLatch.AddComponent<Rigidbody>();
            latchRb.isKinematic = true;

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

            Destroy(rb.GetComponent<SpringJoint>());
            rope.hook.AddComponent<SpringJoint>().connectedBody = rope.hookLatch.GetComponent<Rigidbody>();
            var hsj = rope.hook.GetComponent<SpringJoint>();
            hsj.autoConfigureConnectedAnchor = false;
            hsj.anchor = Vector3.zero;
            hsj.connectedAnchor = Vector3.zero;
            hsj.spring = 0;
            hsj.damper = 0f;
            hsj.maxDistance = 0;
            hsj.minDistance = 0;

            // Knock back when hooked
            rope.hookLatch.GetComponent<Rigidbody>().AddForce(ray.direction * (latchOnImpulse * 0.2f), ForceMode.Impulse);

            // Set rope width
            rope.lineRenderer.startWidth = endThickness;
            rope.lineRenderer.endWidth = endThickness;

            // Enable rope collider
            rope.ropeCollider.GetComponent<BoxCollider>().enabled = true;
            hooked = false;

            // Calculate the distance between the hooks
            float ropeLength = Vector3.Distance(rope.hook.transform.position, hit.point);

            // Check if the rope is too short
            if (ropeLength < minimumRopeLength) {
                DestroyRope(ropes.Count - 1);
                audioSource.PlayOneShot(releaseSound);
                return; // Exit the method as the rope is too short
            }

            // Audio
            audioSource.PlayOneShot(grapplingSound);

            // Instantiate the plank as a child of the hook latch
            var plank = Instantiate(platformPrefab, rope.hook.transform.position, Quaternion.identity);
            plank.transform.parent = rope.lineRenderer.transform;

            // Adjust the scale and position of the plank
            var startPoint = rope.hook.transform.position;
            var endPoint = hit.point;
            var midPoint = (startPoint + endPoint) / 2;
            plank.transform.position = midPoint;

            float distance = Vector3.Distance(startPoint, endPoint);
            plank.transform.localScale = new Vector3(distance, plank.transform.localScale.y, plank.transform.localScale.z);

            // Adjust the rotation of the plank
            plank.transform.LookAt(endPoint);
            plank.transform.Rotate(0, 90, 0); // Rotate 90 degrees to make the plank align with the rope
        }

        void RetractHooks() {
            // Set player hook swing strength
            if (executeHookSwing && rb.GetComponent<SpringJoint>() && Mathf.Approximately(rb.GetComponent<SpringJoint>().spring, playerRetractStrength))
                rb.GetComponent<SpringJoint>().spring = playerRetractStrength;

            // Set player hook retract strength
            if (!Input.GetMouseButtonDown(2) || playerDependencies.isInspecting) return;

            if (rb.GetComponent<SpringJoint>() != null)
                rb.GetComponent<SpringJoint>().spring = playerRetractStrength;

            // Set all other hook and latched retract strengths
            foreach (var rope in ropes) {
                if (rope.hook.GetComponent<SpringJoint>() && rope.hook.GetComponent<SpringJoint>().connectedBody != rb)
                    rope.hook.GetComponent<SpringJoint>().spring = retractStrength;
            }

            if (ropes.Count > 0)
                audioSource.PlayOneShot(retractSound);
        }

        void CutRopes() {
            // Destroy player hooks upon hold release
            if (hookRelease && hooked) {
                hookRelease = false;
                DestroyGrappleRope();
            }

            // Remove specific hooks
            if (Input.GetKey(cutRopeKey) && !playerDependencies.isInspecting) {
                if (hooked)
                    DestroyGrappleRope();
                else if (!hooked && Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, ropeLayerMask))
                    if (hit.collider.isTrigger) {
                        int index = GameObjectToIndex(hit.collider.gameObject);
                        DestroyRope(index);
                    }
            }

            // Destroy everything created and clear all lists
            if (Input.GetKeyDown(resetHookKey) && !playerDependencies.isInspecting) DestroyRopes();
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

                for (var t = 0; t < segments + 1; t++) {
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
                }
            }
        }

        int GameObjectToIndex(GameObject ropeColliderList) {
            for (var i = 0; i < ropes.Count; i++) {
                if (ropes[i].ropeCollider == ropeColliderList)
                    return i;
            }

            return -1;
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
            public Spring spring;
        }
    }

    public enum RopeType
    {
        BOTH,
        LEFT,
        RIGHT,
    }
}