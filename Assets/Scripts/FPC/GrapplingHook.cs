#region

using System;
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
        [SerializeField] List<Rope> ropes = new List<Rope>();

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

            if (!playerDependencies.isInspecting || !playerDependencies.isGrabbing) {
                CreateHooks(0);
                CreateHooks(1);
                CutRopes();
            }
        }

        void LateUpdate() {
            DrawRopes();
        }

        void InputCheck() {
            bool mouseDown = Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0);
            bool mouseHeld = Input.GetMouseButton(1) || Input.GetMouseButton(0);
            bool mouseUp = Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0);
            bool controlNotHeld = !Input.GetKey(KeyCode.LeftControl);
            bool notInspecting = !playerDependencies.isInspecting;

            if (mouseDown && controlNotHeld && notInspecting) {
                mouseDownTimer = 0;
                hookRelease = false;
                executeHookSwing = false;
            }

            if (mouseHeld && controlNotHeld && notInspecting) {
                mouseDownTimer += Time.deltaTime;
                if (hooked && mouseDownTimer >= holdDelayToSwing && !executeHookSwing)
                    executeHookSwing = true;
            }

            if (mouseUp && controlNotHeld && mouseDownTimer >= holdDelayToSwing && executeHookSwing && notInspecting) {
                executeHookSwing = false;
                hookRelease = true;
                ApplyReleaseImpulse();
            }
        }

        void ApplyReleaseImpulse() {
            var playerVelocity = rb.linearVelocity;
            float speedFactor = playerVelocity.magnitude;
            var releaseImpulse = playerVelocity.normalized * (speedFactor * releaseImpulseFactor);
            rb.AddForce(releaseImpulse, ForceMode.Impulse);
        }

        void CreateHooks(int mouseButton) {
            if (!Input.GetMouseButtonDown(mouseButton) || Input.GetKey(KeyCode.LeftControl) || playerDependencies.isInspecting)
                return;

            ray = playerDependencies.cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray.origin, ray.direction, out hit, hookDistance, grappleLayerMask, QueryTriggerInteraction.Ignore))
                return;

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Default") || hit.transform.gameObject.layer == LayerMask.NameToLayer("Static"))
                return;

            EnsureRigidbodyOnHitObject();

            if (!hooked)
                CreateInitialHook(mouseButton);
            else if (hooked)
                CreateHookLatch();
        }

        void EnsureRigidbodyOnHitObject() {
            if (hit.transform.gameObject.GetComponent<Rigidbody>() == null)
                hit.transform.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        }

        void CreateInitialHook(int mouseButton) {
            if (hit.collider.isTrigger || hit.collider.gameObject.GetComponent<Rigidbody>() == rb)
                return;

            hooked = true;
            var rope = new Rope {
                hook = new GameObject("Hook"),
                type = mouseButton == 0 ? RopeType.LEFT : RopeType.RIGHT,
                connectedObject1 = hit.transform.gameObject,
            };

            SetupHook(rope);
            SetupRopeRenderer(rope, mouseButton);
            SetupSpringJoint(rope);
            AddRopeCollider(rope);
            ApplyHookKnockback(rope);

            audioSource.PlayOneShot(grapplingSound);
            ropes.Add(rope);
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
            rb.gameObject.AddComponent<SpringJoint>().connectedBody = rope.hook.GetComponent<Rigidbody>();
            rope.hook.AddComponent<FixedJoint>().connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();

            var sj = rb.GetComponent<SpringJoint>();
            sj.autoConfigureConnectedAnchor = false;
            sj.connectedAnchor = Vector3.zero;

            float distanceFromHook = Vector3.Distance(rb.gameObject.transform.position, rope.hook.transform.position);
            sj.maxDistance = distanceFromHook;
            sj.minDistance = distanceFromHook;
            sj.spring = 20000f;
            sj.damper = 10000f;
        }

        void AddRopeCollider(Rope rope) {
            rope.ropeCollider = new GameObject("RopeCollider");
            rope.ropeCollider.transform.parent = rope.hook.transform;
            var collider = rope.ropeCollider.AddComponent<BoxCollider>();
            collider.size = new Vector3(0.1f, 0, 0.1f);
            collider.isTrigger = true;
            collider.enabled = false;
        }

        void ApplyHookKnockback(Rope rope) {
            rope.hook.GetComponent<Rigidbody>().AddForce(ray.direction * (latchOnImpulse * 0.2f), ForceMode.Impulse);
        }

        void CreateHookLatch() {
            if (!Physics.Raycast(ray.origin, ray.direction, out hit, hookDistance, grappleLayerMask, QueryTriggerInteraction.Ignore))
                return;

            if (hit.collider.isTrigger || hit.collider.gameObject.GetComponent<Rigidbody>() == rb)
                return;

            var rope = ropes[^1];
            SetupHookLatch(rope);
            SetupLatchJoints(rope);
            UpdateRopeProperties(rope);
            CreatePlank(rope);

            hooked = false;
            audioSource.PlayOneShot(grapplingSound);

            CheckRopeLimit();
        }

        void SetupHookLatch(Rope rope) {
            rope.hookLatch = new GameObject("HookLatch");
            rope.hookLatch.transform.position = hit.point;
            rope.hookLatch.transform.parent = hit.transform;

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

            rope.hookLatch.GetComponent<Rigidbody>().AddForce(ray.direction * (latchOnImpulse * 0.2f), ForceMode.Impulse);
        }

        void UpdateRopeProperties(Rope rope) {
            rope.lineRenderer.startWidth = endThickness;
            rope.lineRenderer.endWidth = endThickness;
            rope.ropeCollider.GetComponent<BoxCollider>().enabled = true;
            rope.connectedObject2 = hit.transform.gameObject;
        }

        void CreatePlank(Rope rope) {
            float ropeLength = Vector3.Distance(rope.hook.transform.position, hit.point);
            if (ropeLength < minimumRopeLength) {
                DestroyRope(ropes.Count - 1);
                audioSource.PlayOneShot(releaseSound);
                return;
            }

            rope.plank = Instantiate(platformPrefab, rope.hook.transform.position, Quaternion.identity);
            rope.plank.transform.parent = null;

            UpdatePlankTransform(rope);
        }

        void UpdatePlankTransform(Rope rope) {
            var startPoint = rope.hook.transform.position;
            var endPoint = hit.point;
            var midPoint = (startPoint + endPoint) / 2;
            rope.plank.transform.position = midPoint;

            float distance = Vector3.Distance(startPoint, endPoint);
            rope.plank.transform.localScale = new Vector3(distance, rope.plank.transform.localScale.y, rope.plank.transform.localScale.z);

            rope.plank.transform.rotation = Quaternion.LookRotation(endPoint - startPoint);
            rope.plank.transform.Rotate(0, 90, 0);
        }

        void CheckRopeLimit() {
            var leftRopes = ropes.FindAll(r => r.type == RopeType.LEFT);
            var rightRopes = ropes.FindAll(r => r.type == RopeType.RIGHT);
            if (leftRopes.Count > maxRopes) DestroyRope(ropes.IndexOf(leftRopes[0]));
            if (rightRopes.Count > maxRopes) DestroyRope(ropes.IndexOf(rightRopes[0]));
        }

        void HandleRopeLength() {
            if (ropes.Count == 0) return;

            if (Input.mouseScrollDelta.y < 0) {
                audioSource.PlayOneShot(pullClip);
                AdjustRopeLength(-retractAmount);
            }
            else if (Input.mouseScrollDelta.y > 0) {
                audioSource.PlayOneShot(pushClip);
                AdjustRopeLength(retractAmount);
            }
        }

        void AdjustRopeLength(float adjustment) {
            foreach (var rope in ropes) {
                var sj = rope.hook.GetComponent<SpringJoint>();
                if (sj != null) {
                    sj.maxDistance = Mathf.Max(sj.maxDistance + adjustment, minimumRopeLength);
                    sj.minDistance = Mathf.Max(sj.minDistance + adjustment, 0f);
                }
            }
        }

        void CutRopes() {
            if ((Input.GetKey(cutRopeKey) || hookRelease) && hooked) {
                hookRelease = false;
                DestroyGrappleRope();
                return;
            }

            if (Input.GetKey(cutRopeKey)) {
                CutRopeWithRaycast();
            }

            if (Input.GetKeyDown(resetHookKey) && !playerDependencies.isInspecting) {
                DestroyRopes();
            }
        }

        void CutRopeWithRaycast() {
            var r = playerDependencies.cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r.origin, r.direction, out hit, hookDistance, ropeLayerMask)) {
                int ropeIndex = GameObjectToIndex(hit.collider.gameObject);
                if (ropeIndex != -1) {
                    DestroyRope(ropeIndex);
                }
            }
        }

        int GameObjectToIndex(GameObject ropeColliderObject) {
            return ropes.FindIndex(rope => rope.ropeCollider == ropeColliderObject);
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
            if (rope.plank != null) Destroy(rope.plank);
            foreach (var model in rope.hookModels) {
                Destroy(model);
            }

            ropes.RemoveAt(index);

            if (ropes.Count == 0) hooked = false;
            audioSource.PlayOneShot(releaseSound);
        }

        public void DestroyRopes(RopeType ropeType = RopeType.BOTH) {
            if (hooked && (ropes.Last().type == ropeType || ropeType == RopeType.BOTH)) {
                DestroyGrappleRope();
            }

            var ropesToRemove = ropes.Where(rope => ropeType == RopeType.BOTH || rope.type == ropeType).ToList();

            foreach (var rope in ropesToRemove) {
                DestroyRopeComponents(rope);
                ropes.Remove(rope);
            }

            if (ropesToRemove.Count > 0) {
                audioSource.PlayOneShot(releaseSound);
            }
        }

        void DestroyRopeComponents(Rope rope) {
            Destroy(rope.hook.gameObject);
            if (rope.hookLatch != null) Destroy(rope.hookLatch.gameObject);
            if (rope.ropeCollider != null) Destroy(rope.ropeCollider.gameObject);
            if (rope.plank != null) Destroy(rope.plank);
            foreach (var model in rope.hookModels) {
                Destroy(model);
            }
        }

        void DrawRopes() {
            foreach (var rope in ropes) {
                rope.spring.Update(Time.fixedDeltaTime);

                Vector3 startPoint, endPoint;
                if (!GetRopePoints(rope, out startPoint, out endPoint)) continue;

                UpdateRopeRenderer(rope, startPoint, endPoint);
                UpdateRopeCollider(rope, startPoint, endPoint);
                UpdatePlankPosition(rope, startPoint, endPoint);
            }
        }

        bool GetRopePoints(Rope rope, out Vector3 startPoint, out Vector3 endPoint) {
            startPoint = endPoint = Vector3.zero;

            if (rb.GetComponent<SpringJoint>() != null && rb.GetComponent<SpringJoint>().connectedBody == rope.hook.GetComponent<Rigidbody>()) {
                startPoint = rope.type == RopeType.LEFT ? playerDependencies.spawnPointLeft.position : playerDependencies.spawnPointRight.position;
                endPoint = rope.hook.transform.position;
                return true;
            }
            if (rope.hook.GetComponent<SpringJoint>() != null && rope.hook.GetComponent<SpringJoint>().connectedBody != rb) {
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

            for (int t = 0; t <= segments; t++) {
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
            if (rope.ropeCollider != null && rope.hook.GetComponent<SpringJoint>() != null) {
                rope.ropeCollider.transform.position = startPoint;
                rope.ropeCollider.transform.LookAt(endPoint);
                float distance = Vector3.Distance(startPoint, endPoint);
                rope.ropeCollider.GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, distance);
                rope.ropeCollider.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, distance / 2);

                rope.ropeCollider.layer = LayerMask.NameToLayer("Rope");
                rope.ropeCollider.tag = "Rope";
            }
        }

        void UpdatePlankPosition(Rope rope, Vector3 startPoint, Vector3 endPoint) {
            if (rope.plank != null) {
                var midPoint = (startPoint + endPoint) / 2;
                rope.plank.transform.position = midPoint;

                float distance = Vector3.Distance(startPoint, endPoint);
                rope.plank.transform.localScale = new Vector3(distance, rope.plank.transform.localScale.y, rope.plank.transform.localScale.z);

                rope.plank.transform.rotation = Quaternion.LookRotation(endPoint - startPoint);
                rope.plank.transform.Rotate(0, 90, 0);
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