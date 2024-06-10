#region

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#endregion

namespace PrototypeFPC
{
    public class GrapplingHook : MonoBehaviour
    {
        // Dependencies
        [Header("Dependencies")]
        [SerializeField] public Dependencies dependencies;

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
        [SerializeField] List<GrappleRope> ropes = new List<GrappleRope>();

        // Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip grapplingSound;
        [SerializeField] AudioClip releaseSound;
        [SerializeField] AudioClip retractSound;

        AudioSource audioSource;

        bool executeHookSwing;
        RaycastHit hit;
        bool hooked;
        bool hookRelease;

        float mouseDownTimer;
        Rigidbody player;
        Ray ray;
        Spring spring;

        void Start() {
            Setup();
            CreateSpring();
        }

        void Update() {
            InputCheck();
            CreateHooks(0);
            CreateHooks(1);
            RetractHooks();
            CutRopes();
        }

        void LateUpdate() {
            DrawRopes();
        }

        void Setup() {
            // Setup dependencies
            player = dependencies.rb;
            audioSource = dependencies.audioSourceTop;
        }

        void CreateSpring() {
            // Create and set rope visual spring value
            spring = new Spring();
            spring.SetTarget(0);
        }

        void InputCheck() {
            // Reset checker
            if ((Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) && !Input.GetKey(KeyCode.LeftControl) && !dependencies.isInspecting) {
                mouseDownTimer = 0;
                hookRelease = false;
                executeHookSwing = false;
            }

            // Check input for hook to swing
            if ((Input.GetMouseButton(1) || Input.GetMouseButton(0)) && !Input.GetKey(KeyCode.LeftControl) && !dependencies.isInspecting) {
                mouseDownTimer += Time.deltaTime;

                if (hooked && mouseDownTimer >= holdDelayToSwing && !executeHookSwing) executeHookSwing = true;
            }

            // Check input for hook to latch
            if ((Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0)) && !Input.GetKey(KeyCode.LeftControl) && mouseDownTimer >= holdDelayToSwing && executeHookSwing && !dependencies.isInspecting) {
                executeHookSwing = false;
                hookRelease = true;

                // Get the player's current velocity
                var playerVelocity = player.linearVelocity;
                float speedFactor = playerVelocity.magnitude;

                // Apply an impulse based on the speed at release
                var releaseImpulse = playerVelocity.normalized * (speedFactor * releaseImpulseFactor);
                player.AddForce(releaseImpulse, ForceMode.Impulse);
            }
        }

        void CreateHooks(int mouseButton) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(mouseButton) && !Input.GetKey(KeyCode.LeftControl) && !dependencies.isInspecting) {
                // Check and set target rigidbody if none
                if (Physics.Raycast(ray.origin, ray.direction, out hit, hookDistance, grappleLayerMask, QueryTriggerInteraction.Ignore))
                    if (hit.transform.gameObject.GetComponent<Rigidbody>() == null)
                        hit.transform.gameObject.AddComponent<Rigidbody>().isKinematic = true;

                // Create first hook
                if (!hooked) {
                    if (!Physics.Raycast(ray.origin, ray.direction, out hit, hookDistance, grappleLayerMask, QueryTriggerInteraction.Ignore)) return;
                    if (hit.collider.isTrigger || hit.collider.gameObject.GetComponent<Rigidbody>() == player) return;
                    hooked = true;

                    CreateHook(mouseButton, hit.point);
                }
                else if (hooked) {
                    if (!Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, grappleLayerMask, QueryTriggerInteraction.Ignore)) return;
                    if (hit.collider.isTrigger || hit.collider.gameObject.GetComponent<Rigidbody>() == player) return;

                    CreateHookLatch(hit.point);
                }
            }
        }

        void CreateHook(int _mouseButton, Vector3 _position) {
            // Create new rope
            var rope = new GrappleRope {
                hook = new GameObject("Hook") {
                    transform = {
                        position = _position,
                    },
                },
                isLeft = _mouseButton == 0,
            };

            // Add Rigidbody to hook
            var hookRb = rope.hook.AddComponent<Rigidbody>();
            hookRb.isKinematic = true;

            // Hook end point model
            rope.hookModels.Add(Instantiate(hookModel, rope.hook.transform.position, Quaternion.identity));
            rope.hookModels[^1].transform.parent = rope.hook.transform;

            // Hook start point model
            var spawnPoint = _mouseButton == 0 ? dependencies.spawnPointLeft.position : dependencies.spawnPointRight.position;
            rope.hookModels.Add(Instantiate(hookModel, spawnPoint, Quaternion.identity));
            rope.hookModels[^1].transform.parent = _mouseButton == 0 ? dependencies.spawnPointLeft.transform : dependencies.spawnPointRight.transform;

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
            rope.lineRenderer.positionCount = segments + 1; // Set positionCount here

            // Add and set joint parameters
            spring.Reset();
            rope.lineRenderer.positionCount = segments + 1;

            player.gameObject.AddComponent<SpringJoint>().connectedBody = rope.hook.GetComponent<Rigidbody>();
            var sj = player.GetComponent<SpringJoint>();
            sj.autoConfigureConnectedAnchor = false;
            sj.connectedAnchor = Vector3.zero;

            // Calculate the distance between the player and the hook point
            float distanceFromHook = Vector3.Distance(player.gameObject.transform.position, rope.hook.transform.position);

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

            // Set previous rope quality to 2 if not already
            if (ropes.Count > 1 && ropes[^2].lineRenderer.positionCount > 2)
                ropes[^2].lineRenderer.positionCount = 2;

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

            // Add and set joint parameters
            spring.Reset();
            spring.SetVelocity(speed);

            rope.hookLatch.AddComponent<FixedJoint>().connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();

            Destroy(player.GetComponent<SpringJoint>());
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
                DestroyHook(ropes.Count - 1);
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
            if (executeHookSwing && player.GetComponent<SpringJoint>() && Mathf.Approximately(player.GetComponent<SpringJoint>().spring, playerRetractStrength))
                player.GetComponent<SpringJoint>().spring = playerRetractStrength;

            // Set player hook retract strength
            if (!Input.GetMouseButtonDown(2) || dependencies.isInspecting) return;

            if (player.GetComponent<SpringJoint>() != null)
                player.GetComponent<SpringJoint>().spring = playerRetractStrength;

            // Set all other hook and latched retract strengths
            foreach (var rope in ropes) {
                if (rope.hook.GetComponent<SpringJoint>() && rope.hook.GetComponent<SpringJoint>().connectedBody != player)
                    rope.hook.GetComponent<SpringJoint>().spring = retractStrength;
            }

            if (ropes.Count > 0)
                audioSource.PlayOneShot(retractSound);
        }

        void CutRopes() {
            // Destroy player hooks upon hold release
            if (hookRelease && hooked) {
                hookRelease = false;
                DestroyLastHook();
            }

            // Remove specific hooks
            if (Input.GetKey(cutRopeKey) && !dependencies.isInspecting) {
                if (hooked)
                    DestroyLastHook();
                else if (!hooked && Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, ropeLayerMask))
                    if (hit.collider.isTrigger) {
                        int index = GameObjectToIndex(hit.collider.gameObject);
                        DestroyHook(index);
                    }
            }

            // Destroy everything created and clear all lists
            if (Input.GetKeyDown(resetHookKey) && !dependencies.isInspecting) ResetHook();
        }

        void DestroyLastHook() {
            if (ropes.Count > 0) {
                Destroy(player.GetComponent<SpringJoint>());
                DestroyHook(ropes.Count - 1);
            }

            hooked = false;
            hookRelease = false;
            audioSource.PlayOneShot(releaseSound);
        }

        void DestroyHook(int index) {
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

        public void ResetHook() {
            hooked = false;

            if (ropes.Count > 0 && player.GetComponent<SpringJoint>())
                Destroy(player.GetComponent<SpringJoint>());

            foreach (var rope in ropes) {
                Destroy(rope.hook.gameObject);
                if (rope.hookLatch != null) Destroy(rope.hookLatch.gameObject);
                if (rope.ropeCollider != null) Destroy(rope.ropeCollider.gameObject);
                foreach (var model in rope.hookModels) {
                    Destroy(model);
                }
            }

            ropes.Clear();
            audioSource.PlayOneShot(releaseSound);
        }

        void DrawRopes() {
            foreach (var rope in ropes) {
                // Update spring properties
                spring.SetDamper(damper);
                spring.SetStrength(springStrength);
                spring.Update(Time.deltaTime);

                Vector3 startPoint;
                Vector3 endPoint;

                if (player.GetComponent<SpringJoint>() != null && player.GetComponent<SpringJoint>().connectedBody == rope.hook.GetComponent<Rigidbody>()) {
                    startPoint = rope.isLeft ? dependencies.spawnPointLeft.position : dependencies.spawnPointRight.position;
                    endPoint = rope.hook.transform.position;
                }
                else if (rope.hook.GetComponent<SpringJoint>() != null && rope.hook.GetComponent<SpringJoint>().connectedBody != player) {
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
                    var offset = up * (waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta))
                                 + right * (waveHeight * Mathf.Cos(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta));
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
        public class GrappleRope
        {
            public bool isLeft;
            public GameObject hook;
            public GameObject hookLatch;
            public GameObject ropeCollider;
            public LineRenderer lineRenderer;
            public List<GameObject> hookModels = new List<GameObject>();
        }
    }
}