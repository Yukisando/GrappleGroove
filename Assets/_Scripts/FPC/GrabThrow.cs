#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class GrabThrow : MonoBehaviour
    {
        [Header("Input Properties")]
        [SerializeField] KeyCode grabDropKey = KeyCode.E;
        [SerializeField] KeyCode throwKey = KeyCode.F;

        [Header("Grab/Throw Properties")]
        public float maxGrabDistance = 8f;
        [SerializeField] float desiredHoldDistance = 4.5f;
        [SerializeField] float grabbingDistance = 0.3f;
        [SerializeField] float grabSpeed = 15f;
        [SerializeField] float throwForce = 800f;

        [Header("Audio Properties")]
        [SerializeField] AudioClip grabSound;
        [SerializeField] AudioClip throwSound;

        AudioSource audioSource;
        Rigidbody grabbedObject;
        Transform grabPoint;
        RaycastHit hit;
        PlayerDependencies playerDependencies;
        Ray ray;
        ID grabbedID;
        int originalLayer = -1;

        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
            grabPoint = playerDependencies.grabPoint;
            audioSource = playerDependencies.audioSourceTop;

            grabPoint.gameObject.AddComponent<Rigidbody>().useGravity = false;
            grabPoint.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        void Update() {
            GrabHoldThrow();
        }

        void FixedUpdate() {
            Hold();
        }

        void GrabHoldThrow() {
            ray = playerDependencies.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (playerDependencies.isGrabbing && grabbedObject != null) {
                if (Input.GetKeyDown(throwKey))
                    ThrowObject(); // Throw
                else if (Input.GetKeyDown(grabDropKey))
                    ThrowObject(true); // Release
            }
            else if (!playerDependencies.isGrabbing && !playerDependencies.isInspecting) HandleGrabAttempt();
        }

        void HandleGrabAttempt() {
            if (Input.GetKeyDown(grabDropKey))
                if (Physics.SphereCast(ray.origin, 0.25f, ray.direction, out hit, maxGrabDistance)) {
                    var grabbable = hit.collider.gameObject.GetComponent<Grabbable>();
                    if (grabbable) {
                        var grappling = playerDependencies.GetComponent<GrapplingHook>();
                        for (int i = grappling.ropes.Count - 1; i >= 0; i--) {
                            var rope = grappling.ropes[i];
                            bool isPlayerDirectlyConnected = rope.connectedObject1 == hit.collider.gameObject &&
                                                             rope.connectedObject2 &&
                                                             rope.connectedObject2.CompareTag("Player");

                            if (isPlayerDirectlyConnected) {
                                grappling.DestroyRope(i);
                                break;
                            }
                        }

                        GrabObject(grabbable.GetComponent<Rigidbody>());
                    }
                }
        }

        void GrabObject(Rigidbody hitRigidbody) {
            hitRigidbody.TryGetComponent(out grabbedID);
            if (grabbedID) grabbedID.onReset.AddListener(OnGrabbedObjectReset);

            grabPoint.position = hit.point;
            grabbedObject = hitRigidbody;

            grabbedObject.linearVelocity = Vector3.zero;
            grabbedObject.angularVelocity = Vector3.zero;

            // Store original layer and change to "Slip"
            originalLayer = grabbedObject.gameObject.layer;
            grabbedObject.gameObject.layer = LayerMask.NameToLayer("Grabbed");

            // Disable collision with player
            Physics.IgnoreCollision(grabbedObject.GetComponent<Collider>(), playerDependencies.cc, true);

            playerDependencies.isGrabbing = true;
            audioSource.PlayOneShot(grabSound);
        }

        void ThrowObject(bool drop = false) {
            if (grabbedObject) {
                // Re-enable collision with player
                Physics.IgnoreCollision(grabbedObject.GetComponent<Collider>(), playerDependencies.cc, false);

                // Restore original layer
                if (originalLayer != -1)
                    grabbedObject.gameObject.layer = originalLayer;

                if (!drop)
                    grabbedObject.AddForce(playerDependencies.cam.transform.forward * throwForce, ForceMode.Impulse);
            }

            grabbedObject = null;
            if (grabbedID) grabbedID.onReset.RemoveListener(OnGrabbedObjectReset);
            grabbedID = null;
            playerDependencies.isGrabbing = false;
            audioSource.PlayOneShot(throwSound);
        }

        void OnGrabbedObjectReset(bool wasSpawned) {
            ThrowObject(true);
        }

        void Hold() {
            if (playerDependencies.isGrabbing && grabbedObject != null) {
                // Move toward camera forward position
                var targetPosition = playerDependencies.cam.transform.position + playerDependencies.cam.transform.forward * desiredHoldDistance;
                var moveDirection = targetPosition - grabbedObject.position;
                grabbedObject.linearVelocity = moveDirection.normalized * Mathf.Min(moveDirection.magnitude * grabSpeed, grabSpeed);

                // Torque-based rotation alignment to grabPoint.forward
                AlignGrabbedObjectRotation();
            }
        }

        // Aligns the grabbed object's rotation to the grab point's forward.
        void AlignGrabbedObjectRotation() {
            if (grabbedObject == null) return;

            var targetForward = grabPoint.forward;
            var currentRotation = grabbedObject.rotation;
            var targetRotation = Quaternion.LookRotation(targetForward, Vector3.up);

            // Calculate shortest rotation
            var deltaRotation = targetRotation * Quaternion.Inverse(currentRotation);
            deltaRotation.ToAngleAxis(out float angle, out var axis);
            if (angle > 180f) angle -= 360f;

            var alignTorque = 500f; // Adjust for snappiness
            var torque = axis * (angle * Mathf.Deg2Rad * alignTorque);

            // Apply torque with damping
            grabbedObject.AddTorque(torque - grabbedObject.angularVelocity * 10f, ForceMode.Acceleration);
        }
    }
}