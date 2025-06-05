#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

#endregion

namespace PrototypeFPC
{
    public enum RopeType { BOTH, LEFT, RIGHT }

    public class GrapplingHook : MonoBehaviour
    {
        [Header("Assignments")]
        [SerializeField] KeyCode cutRopeKey = KeyCode.C;
        [SerializeField] KeyCode resetHookKey = KeyCode.R;
        [SerializeField] KeyCode pullKey = KeyCode.F;
        [SerializeField] public LayerMask ropeLayerMask;
        [SerializeField] GameObject hookModel;
        [SerializeField] GameObject plankPrefab;

        [Header("Settings")]
        public float maxRopeLength = 50f;
        [SerializeField] float minimumRopeLength = 1f;
        [SerializeField] float releaseJumpForce = 10f;
        [SerializeField] float upwardForceRatio = 0.5f;
        [SerializeField] float connectionSpringStrength = 10000f;
        [SerializeField] float connectionDamperStrength = 1000f;

        [Header("Rope Properties")]
        [SerializeField] Material leftRopeMaterial;
        [SerializeField] Material rightRopeMaterial;
        [SerializeField] float startThickness = 0.02f;
        [SerializeField] float endThickness = 0.06f;
        [SerializeField] int maxRopes = 2;

        [Header("Rope Visual Spring Properties")]
        [SerializeField] int segments = 50;
        [SerializeField] float damper = 12;
        [SerializeField] float springStrength = 800;
        [SerializeField] float speed = 12;
        [SerializeField] float waveCount = 5;
        [SerializeField] float waveHeight = 4;
        [SerializeField] AnimationCurve affectCurve;
        public List<Rope> ropes = new List<Rope>();

        [Header("Audio Properties")]
        [SerializeField] AudioClip grapplingSound;
        [SerializeField] AudioClip releaseSound;
        [SerializeField] AudioClip pushPullClip;

        [Header("Settings")]
        [SerializeField] float reelSpeed = 20f;
        [SerializeField] float objectReelSpeed = 10f;

        AudioSource audioSource;
        bool executeHookSwing;
        RaycastHit hit;
        bool hooked;
        bool hookRelease;
        bool leftGrappleHeld;
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
            SnapRopesExceedingMaxLimit();

            if (!playerDependencies.isInspecting && !playerDependencies.isGrabbing) {
                CreateHooks(0);
                CreateHooks(1);
                RopeCutCheck();

                if (Input.GetKey(pullKey)) HandleRopeLengthControl();
            }
        }

        void HandleRopeLengthControl() {
            if (ropes.Count == 0) return;

            var anyRopeAdjusted = false;

            foreach (var rope in ropes) {
                if (rope == null || rope.hook == null || rope.hookLatch == null)
                    continue;

                var springJoint = rope.hook.GetComponent<SpringJoint>();
                if (!springJoint) continue;

                float currentLength = springJoint.maxDistance;
                float targetLength = currentLength;

                switch (rope.type) {
                    // Pull (shorten)
                    case RopeType.LEFT: {
                        float newLength = Mathf.Max(currentLength - objectReelSpeed * Time.deltaTime, minimumRopeLength);
                        if (!Mathf.Approximately(newLength, currentLength)) {
                            targetLength = newLength;
                            anyRopeAdjusted = true;
                        }
                        break;
                    }

                    // Push (extend)
                    case RopeType.RIGHT: {
                        float newLength = Mathf.Min(currentLength + objectReelSpeed * Time.deltaTime, maxRopeLength);
                        if (!Mathf.Approximately(newLength, currentLength)) {
                            targetLength = newLength;
                            anyRopeAdjusted = true;
                        }
                        break;
                    }
                }

                springJoint.maxDistance = targetLength;
                springJoint.minDistance = targetLength;
            }

            if (anyRopeAdjusted && !audioSource.isPlaying) audioSource.PlayOneShot(pushPullClip);
        }

        void SnapRopesExceedingMaxLimit() {
            if (ropes.Count == 0) return;

            float maxAllowedLength = maxRopeLength + 3f;

            for (int i = ropes.Count - 1; i >= 0; i--) {
                var rope = ropes[i];

                // Try to get the rope's endpoints
                if (GetRopePoints(rope, out var startPoint, out var endPoint)) {
                    float ropeLength = Vector3.Distance(startPoint, endPoint);
                    if (ropeLength > maxAllowedLength) DestroyRope(i);
                }
            }
        }

        void LateUpdate() {
            DrawRopes();
        }

        void InputCheck() {
            bool mouseDown = Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0);
            bool mouseUp = Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0);
            bool controlNotHeld = !Input.GetKey(KeyCode.LeftControl);
            bool notInspecting = !playerDependencies.isInspecting;

            if (mouseDown && controlNotHeld && notInspecting) hookRelease = false;

            if (mouseUp && controlNotHeld && notInspecting && hooked) TryLatchOrDestroy();
        }

        void ApplyReleaseImpulse() {
            if (ropes.Count == 0 || playerDependencies.isGrounded) return;

            var lastRope = ropes[^1];
            var ropeAnchor = lastRope.hook.transform.position;

            // Calculate direction from player to rope anchor
            var swingDirection = (ropeAnchor - transform.position).normalized;

            // Blend in upward vector for a lift effect
            var releaseDirection = (swingDirection + Vector3.up).normalized;

            // Estimate swing speed (magnitude of current velocity)
            float swingSpeed = rb.linearVelocity.magnitude;

            // Clamp the swing factor to avoid extreme launches
            float clampedSpeedFactor = Mathf.Clamp(swingSpeed, 5f, 20f);

            // Scale the release force based on clamped swing velocity
            float scaledForce = releaseJumpForce * (clampedSpeedFactor / 10f);

            // Apply force in the release direction
            rb.AddForce(releaseDirection * scaledForce, ForceMode.Impulse);

            // // Optional: Add a little extra vertical boost
            rb.AddForce(Vector3.up * (scaledForce * upwardForceRatio), ForceMode.Impulse);

            audioSource.PlayOneShot(releaseSound);
        }

        void CreateHooks(int mouseButton) {
            if (!Input.GetMouseButtonDown(mouseButton) || Input.GetKey(KeyCode.LeftControl) || playerDependencies.isInspecting)
                return;

            // Cancel the opposite rope if it's held
            var oppositeType = mouseButton == 0 ? RopeType.RIGHT : RopeType.LEFT;
            if (hooked && ropes.Count > 0 && ropes[^1].type == oppositeType) DestroyGrappleRope(); // only cancel the held rope (if it's the opposite type)


            var r = GetCameraRay();
            if (!Physics.Raycast(r, out hit, maxRopeLength, ~LayerMask.GetMask("IgnoreRaycast", "Player", "PlayerHitbox"), QueryTriggerInteraction.Ignore))
                return;

            var hookable = hit.collider.GetComponent<Hookable>();
            if (!hookable) return;


            if (!hooked)
                CreateInitialHook(mouseButton);
        }

        void TryLatchOrDestroy() {
            var r = GetCameraRay();
            if (Physics.Raycast(r, out hit, maxRopeLength, ~LayerMask.GetMask("IgnoreRaycast", "Player", "PlayerHitbox"), QueryTriggerInteraction.Ignore)) {
                var hookable = hit.collider.GetComponent<Hookable>();
                if (hookable != null) {
                    CreateHookLatch(); // Success: connect second end
                    return;
                }
            }

            // Failed to find valid second connection
            ApplyReleaseImpulse();
            DestroyGrappleRope();
        }

        void CreateInitialHook(int mouseButton) {
            if (hit.collider.isTrigger || hit.collider.gameObject.GetComponent<Rigidbody>() == rb)
                return;

            hooked = true;
            var rope = new Rope {
                hook = new GameObject("Hook"),
                type = mouseButton == 0 ? RopeType.LEFT : RopeType.RIGHT,
                connectedObject1 = hit.transform.gameObject,
                connectedObject2 = rb.transform.gameObject,
            };

            rope.connectedObject1.GetComponent<Hookable>().isHooked = true;
            RegisterResetCallback(rope, ropes.Count);

            SetupHook(rope);
            SetupRopeRenderer(rope, mouseButton);
            SetupSpringJoint(rope);
            AddRopeCollider(rope);

            audioSource.PlayOneShot(grapplingSound);
            ropes.Add(rope);

            if (hit.collider.GetComponent<Reelable>() != null)
                StartCoroutine(ReelIn_(rope));
        }

        IEnumerator ReelIn_(Rope rope) {
            var closeEnoughDistance = 2f;
            var joint = playerSpringJoint;
            if (!joint) yield break;

            // Keep reeling while the joint exists and the player is far from the hook
            while (joint && Vector3.Distance(rb.position, rope.hook.transform.position) > closeEnoughDistance) {
                // Reduce the max distance to pull the player in
                joint.maxDistance = Mathf.Max(joint.maxDistance - reelSpeed * Time.deltaTime, minimumRopeLength);

                yield return null;
            }

            // Once close enough, destroy the rope
            if (joint) DestroyGrappleRope();
        }

        void SetupHook(Rope rope) {
            rope.hook.transform.position = hit.point;
            rope.hook.transform.parent = hit.transform;
            var hookRB = rope.hook.AddComponent<Rigidbody>();
            hookRB.useGravity = false;

            rope.hookModels.Add(Instantiate(hookModel, rope.hook.transform.position, Quaternion.identity));
            rope.hookModels[^1].transform.parent = rope.hook.transform;

            var spawnPoint = rope.type == RopeType.LEFT ? playerDependencies.spawnPointLeft.position : playerDependencies.spawnPointRight.position;
            rope.hookModels.Add(Instantiate(hookModel, spawnPoint, Quaternion.identity));
            rope.hookModels[^1].transform.parent = rope.type == RopeType.LEFT ? playerDependencies.spawnPointLeft.transform : playerDependencies.spawnPointRight.transform;
        }

        void SetupRopeRenderer(Rope rope, int mouseButton) {
            rope.lineRenderer = rope.hook.AddComponent<LineRenderer>();
            rope.lineRenderer.material = new Material(mouseButton == 0 ? leftRopeMaterial : rightRopeMaterial);
            rope.lineRenderer.startWidth = startThickness;
            rope.lineRenderer.endWidth = endThickness;
            rope.lineRenderer.numCornerVertices = 2;
            rope.lineRenderer.numCapVertices = 10;
            rope.lineRenderer.textureMode = LineTextureMode.Tile;
            rope.lineRenderer.shadowCastingMode = ShadowCastingMode.On;
            rope.lineRenderer.receiveShadows = false;
            rope.lineRenderer.positionCount = segments + 1;

            rope.spring = new Spring();
            rope.spring.SetTarget(0);
            rope.spring.SetDamper(damper);
            rope.spring.SetStrength(springStrength);
        }

        void SetupSpringJoint(Rope rope) {
            playerSpringJoint = rb.gameObject.AddComponent<SpringJoint>();
            playerSpringJoint.connectedBody = rope.hook.GetComponent<Rigidbody>();

            rope.hook.AddComponent<FixedJoint>().connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();

            playerSpringJoint.autoConfigureConnectedAnchor = false;
            playerSpringJoint.connectedAnchor = Vector3.zero;

            float distanceFromHook = Vector3.Distance(rb.gameObject.transform.position, rope.hook.transform.position);

            if (rope.type == RopeType.LEFT) {
                playerSpringJoint.maxDistance = distanceFromHook;
                playerSpringJoint.minDistance = distanceFromHook * 0.05f;
            }
            else {
                playerSpringJoint.minDistance = distanceFromHook;
                playerSpringJoint.maxDistance = distanceFromHook;
            }

            playerSpringJoint.spring = 4000f;
            playerSpringJoint.damper = 200f;
            playerSpringJoint.massScale = 4.0f;
        }

        Ray GetCameraRay() {
            return new Ray(playerDependencies.cam.transform.position, playerDependencies.cam.transform.forward);
        }

        void AddRopeCollider(Rope rope) {
            rope.ropeCollider = new GameObject("RopeCollider") {
                transform = {
                    parent = rope.hook.transform,
                },
            };
            var c = rope.ropeCollider.AddComponent<BoxCollider>();
            c.size = new Vector3(0.1f, 0, 0.1f);
            c.isTrigger = true;
            c.enabled = true;
        }

        void CreateHookLatch() {
            var r = GetCameraRay();
            if (!Physics.Raycast(r, out hit, maxRopeLength, ~LayerMask.GetMask("IgnoreRaycast", "Player", "PlayerHitbox"), QueryTriggerInteraction.Ignore))
                return;

            var hookable = hit.collider.GetComponent<Hookable>();
            if (!hookable) return;
            if (!hookable.connectable) {
                DestroyGrappleRope();
                return;
            }

            var rope = ropes[^1];
            SetupHookLatch(rope);
            SetupLatchJoints(rope);
            UpdateRopeProperties(rope);
            CreatePlank(rope);

            rope.connectedObject2.GetComponent<Hookable>().isHooked = true;
            hooked = false;
            audioSource.PlayOneShot(grapplingSound);

            CheckRopeLimit();
        }

        void SetupHookLatch(Rope rope) {
            if (playerSpringJoint) {
                Destroy(playerSpringJoint);
                playerSpringJoint = null;
            }

            rope.hookLatch = new GameObject("HookLatch") {
                transform = {
                    position = hit.point,
                    parent = hit.transform,
                },
            };

            var hlrb = rope.hookLatch.AddComponent<Rigidbody>();
            hlrb.useGravity = false;

            Destroy(rope.hookModels[^1].gameObject);
            rope.hookModels.RemoveAt(rope.hookModels.Count - 1);

            rope.hookModels.Add(Instantiate(hookModel, rope.hookLatch.transform.position, Quaternion.identity));
            rope.hookModels[^1].transform.parent = rope.hookLatch.transform;

            rope.spring.Reset();
            rope.spring.SetVelocity(speed);
        }

        void SetupLatchJoints(Rope rope) {
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

            float ropeLength = Vector3.Distance(rope.hook.transform.position, hit.point);
            hsj.maxDistance = ropeLength;
            hsj.minDistance = ropeLength;
        }

        void UpdateRopeProperties(Rope rope) {
            rope.lineRenderer.startWidth = endThickness;
            rope.lineRenderer.endWidth = endThickness;
            rope.ropeCollider.GetComponent<BoxCollider>().enabled = true;

            rope.lineRenderer.startWidth = endThickness;
            rope.lineRenderer.endWidth = endThickness;
            rope.ropeCollider.GetComponent<BoxCollider>().enabled = true;
            rope.connectedObject2 = hit.transform.gameObject;

            RegisterResetCallback(rope, ropes.IndexOf(rope));
        }

        void UnregisterResetCallback(Rope rope) {
            void TryUnsubscribe(GameObject obj) {
                if (obj == null) return;
                var idComponent = obj.GetComponent<ID>();
                if (idComponent != null)
                    idComponent.onReset -= spawned => DestroyRope(ropes.IndexOf(rope));
            }

            TryUnsubscribe(rope.connectedObject1);
            TryUnsubscribe(rope.connectedObject2);
        }

        void CreatePlank(Rope rope) {
            float ropeLength = Vector3.Distance(rope.hook.transform.position, hit.point);
            if (ropeLength < minimumRopeLength) {
                DestroyRope(ropes.Count - 1);
                audioSource.PlayOneShot(releaseSound);
                return;
            }

            rope.plank = Instantiate(plankPrefab, rope.hook.transform.position, Quaternion.identity);
            rope.plank.transform.parent = null;

            // Add and initialize the Plank component
            var plankComponent = rope.plank.GetComponent<Plank>();
            plankComponent.Initialize(this, ropes.Count - 1);

            // Schedule collision ignoring for next frame to ensure all components are properly initialized
            IgnoreCollisionsForRope(rope);
        }

        void IgnoreCollisionsForRope(Rope rope) {
            // Get all colliders from the plank
            // if (rope == null) return;
            var plankColliders = rope.plank.GetComponentsInChildren<Collider>();

            // Ignore collisions with connected object 1
            if (rope.connectedObject1 != null) {
                var obj1Colliders = rope.connectedObject1.GetComponentsInChildren<Collider>();
                foreach (var plankCol in plankColliders) {
                    foreach (var objCol in obj1Colliders) {
                        if (plankCol != null && objCol != null) Physics.IgnoreCollision(plankCol, objCol, true);
                    }
                }
            }

            // Ignore collisions with connected object 2
            if (rope.connectedObject2 != null) {
                var obj2Colliders = rope.connectedObject2.GetComponentsInChildren<Collider>();
                foreach (var plankCol in plankColliders) {
                    foreach (var objCol in obj2Colliders) {
                        if (plankCol != null && objCol != null) Physics.IgnoreCollision(plankCol, objCol, true);
                    }
                }
            }

            // Store the ignored colliders in the rope for later restoration
            rope.ignoredCollisions = new List<ColliderPair>();

            if (rope.connectedObject1 != null)
                foreach (var plankCol in plankColliders) {
                    foreach (var objCol in rope.connectedObject1.GetComponentsInChildren<Collider>()) {
                        rope.ignoredCollisions.Add(new ColliderPair {
                            first = plankCol,
                            second = objCol,
                        });
                    }
                }

            if (rope.connectedObject2 != null)
                foreach (var plankCol in plankColliders) {
                    foreach (var objCol in rope.connectedObject2.GetComponentsInChildren<Collider>()) {
                        rope.ignoredCollisions.Add(new ColliderPair {
                            first = plankCol,
                            second = objCol,
                        });
                    }
                }
        }

        void UpdatePlankTransform(Rope rope, Vector3 startPoint, Vector3 endPoint) {
            var midPoint = (startPoint + endPoint) / 2;
            rope.plank.transform.position = midPoint;

            float distance = Vector3.Distance(startPoint, endPoint);
            rope.plank.transform.rotation = Quaternion.LookRotation(endPoint - startPoint);
            rope.plank.transform.Rotate(-90, 0, 0);
            rope.plank.transform.localScale = new Vector3(.2f, distance / 2f, 0.2f);
        }

        void CheckRopeLimit() {
            var leftRopes = ropes.FindAll(r => r.type == RopeType.LEFT);
            var rightRopes = ropes.FindAll(r => r.type == RopeType.RIGHT);
            if (leftRopes.Count > maxRopes) DestroyRope(ropes.IndexOf(leftRopes[0]));
            if (rightRopes.Count > maxRopes) DestroyRope(ropes.IndexOf(rightRopes[0]));
        }

        void RopeCutCheck() {
            if ((Input.GetKey(cutRopeKey) || hookRelease) && hooked) {
                hookRelease = false;
                DestroyGrappleRope();
                return;
            }

            if (Input.GetKey(cutRopeKey)) CutRopeWithRaycast();

            if (Input.GetKeyDown(resetHookKey) && !playerDependencies.isInspecting) DestroyRopes();
        }

        void CutRopeWithRaycast() {
            var r = playerDependencies.cam.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(r.origin, r.direction * maxRopeLength, Color.red, 1f); // Debug ray visualization

            // Use Physics.RaycastAll to see everything the ray hits
            var hits = Physics.RaycastAll(r.origin, r.direction, maxRopeLength);

            if (hits.Length <= 0) return;

            // Check each hit
            foreach (var hitInfo in hits) {
                // First check if we hit a plank
                var plank = hitInfo.collider.GetComponent<Plank>();
                if (!plank) continue;
                plank.Cut();
                return;
            }

            // If no plank was hit, check for rope colliders
            foreach (var hitInfo in hits) {
                if (ropeLayerMask != (ropeLayerMask | 1 << hitInfo.collider.gameObject.layer)) continue;

                int ropeIndex = GameObjectToIndex(hitInfo.collider.gameObject);
                if (ropeIndex == -1) continue;

                Debug.Log($"Found rope with index {ropeIndex}");
                DestroyRope(ropeIndex);
                return;
            }
        }

        int GameObjectToIndex(GameObject ropeColliderObject) {
            // Check if any rope's ropeCollider matches the hit object
            return ropes.FindIndex(rope => rope.ropeCollider == ropeColliderObject);
        }

        void DestroyGrappleRope() {
            if (ropes.Count > 0) {
                if (playerSpringJoint != null) {
                    Destroy(playerSpringJoint);
                    playerSpringJoint = null;
                }
                DestroyRope(ropes.Count - 1);
            }

            hooked = false;
            hookRelease = false;
            audioSource.PlayOneShot(releaseSound);
        }

        void RegisterResetCallback(Rope rope, int ropeIndex) {
            void TrySubscribe(GameObject obj) {
                if (obj == null) return;
                var idComponent = obj.GetComponent<ID>();
                if (idComponent != null)
                    idComponent.onReset += spawned => DestroyRope(ropeIndex);
            }

            TrySubscribe(rope.connectedObject1);
            TrySubscribe(rope.connectedObject2);
        }

        public void DestroyRope(int index) {
            if (index < 0 || index >= ropes.Count) return;

            var rope = ropes[index];

            // Re-enable collisions before destroying objects
            if (rope.ignoredCollisions != null)
                foreach (var pair in rope.ignoredCollisions) {
                    if (pair.first && pair.second) Physics.IgnoreCollision(pair.first, pair.second, false);
                }

            DestroyRopeComponents(rope);
            UnregisterResetCallback(rope);
            ropes.RemoveAt(index);

            if (ropes.Count == 0) hooked = false;
            audioSource.PlayOneShot(releaseSound);

            // Update indices for remaining planks
            for (var i = 0; i < ropes.Count; i++) {
                if (ropes[i].plank) {
                    var plankComponent = ropes[i].plank.GetComponent<Plank>();
                    if (plankComponent) plankComponent.Initialize(this, i);
                }
            }
        }

        public void DestroyRopes(RopeType ropeType = RopeType.BOTH) {
            if (hooked && (ropes.Last().type == ropeType || ropeType == RopeType.BOTH)) DestroyGrappleRope();

            var ropesToRemove = ropes.Where(rope => ropeType == RopeType.BOTH || rope.type == ropeType).ToList();

            foreach (var rope in ropesToRemove) {
                DestroyRopeComponents(rope);
                ropes.Remove(rope);
            }

            if (ropesToRemove.Count > 0) audioSource.PlayOneShot(releaseSound);
        }

        void DestroyRopeComponents(Rope rope) {
            if (rope.hook) Destroy(rope.hook.gameObject);
            if (rope.hookLatch) Destroy(rope.hookLatch.gameObject);
            if (rope.ropeCollider) Destroy(rope.ropeCollider.gameObject);
            if (rope.plank) Destroy(rope.plank);
            rope.connectedObject1.GetComponent<Hookable>().isHooked = false;
            if (rope.connectedObject2.GetComponent<Hookable>()) rope.connectedObject2.GetComponent<Hookable>().isHooked = false;

            foreach (var model in rope.hookModels) {
                Destroy(model);
            }
        }

        void DrawRopes() {
            for (int i = ropes.Count - 1; i >= 0; i--) {
                var rope = ropes[i];

                rope.spring.Update(Time.fixedDeltaTime);

                if (!GetRopePoints(rope, out var startPoint, out var endPoint)) continue;

                UpdateRopeRenderer(rope, startPoint, endPoint);
                UpdateRopeCollider(rope, startPoint, endPoint);
                if (rope.plank) UpdatePlankTransform(rope, startPoint, endPoint);
            }
        }

        SpringJoint playerSpringJoint;

        bool GetRopePoints(Rope rope, out Vector3 startPoint, out Vector3 endPoint) {
            startPoint = endPoint = Vector3.zero;

            if (rope == null || rope.hook == null)
                return false;

            var ropeHookRb = rope.hook.GetComponent<Rigidbody>();

            if (ropeHookRb != null && playerSpringJoint != null && playerSpringJoint.connectedBody == ropeHookRb) {
                startPoint = rope.type == RopeType.LEFT ? playerDependencies.spawnPointLeft.position : playerDependencies.spawnPointRight.position;
                endPoint = rope.hook.transform.position;
                return true;
            }

            var ropeSpring = rope.hook.GetComponent<SpringJoint>();
            if (ropeSpring != null && ropeSpring.connectedBody != rb && rope.hookLatch != null) {
                startPoint = rope.hook.transform.position;
                endPoint = rope.hookLatch.transform.position;
                return true;
            }

            return false;
        }

        void UpdateRopeRenderer(Rope rope, Vector3 startPoint, Vector3 endPoint) {
            if (rope.lineRenderer.positionCount != segments + 1)
                rope.lineRenderer.positionCount = segments + 1;

            var up = Quaternion.LookRotation((endPoint - startPoint).normalized) * Vector3.up;
            var right = Quaternion.LookRotation((endPoint - startPoint).normalized) * Vector3.right;

            for (var t = 0; t <= segments; t++) {
                float delta = t / (float)segments;
                var offset = CalculateRopeOffset(rope, delta, up, right);
                rope.lineRenderer.SetPosition(t, Vector3.Lerp(startPoint, endPoint, delta) + offset);
            }
        }

        Vector3 CalculateRopeOffset(Rope rope, float delta, Vector3 up, Vector3 right) {
            float sinWave = Mathf.Sin(delta * waveCount * Mathf.PI);
            float cosWave = Mathf.Cos(delta * waveCount * Mathf.PI);
            float affectValue = affectCurve.Evaluate(delta);

            return up * (waveHeight * sinWave * rope.spring.Value * affectValue)
                   + right * (waveHeight * cosWave * rope.spring.Value * affectValue);
        }

        void UpdateRopeCollider(Rope rope, Vector3 startPoint, Vector3 endPoint) {
            if (!rope.ropeCollider) return;

            var direction = endPoint - startPoint;
            float distance = direction.magnitude;
            var center = startPoint + direction / 2;
            var halfExtents = new Vector3(0.05f, 0.05f, distance / 2f);

            // Align orientation
            var rotation = Quaternion.LookRotation(direction);

            // Raycast-style collision check to detect CutsRope
            var hits = Physics.OverlapBox(center, halfExtents, rotation, ~LayerMask.GetMask("Player", "IgnoreRaycast"));

            foreach (var hit in hits) {
                if (hit.GetComponent<CutsRopes>() != null) {
                    DestroyRope(ropes.IndexOf(rope));
                    return;
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
            public List<ColliderPair> ignoredCollisions;
        }

        public struct ColliderPair
        {
            public Collider first;
            public Collider second;
        }
    }
}