#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class GrabThrow : MonoBehaviour
    {
        [Header("Input Properties")]
        [SerializeField] KeyCode grabThrowKey = KeyCode.G;

        [Header("Grab/Throw Properties")]
        public float maxGrabDistance = 8f;
        [SerializeField] float grabbingDistance = 0.3f;
        [SerializeField] float grabSpeed = 15;
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

            switch (playerDependencies.isGrabbing) {
                case true when grabbedObject != null && Input.GetKeyDown(grabThrowKey):
                    ThrowObject();
                    break;
                case false when !playerDependencies.isInspecting:
                    HandleGrabAttempt();
                    break;
            }
        }

        void HandleGrabAttempt() {
            if (Input.GetKeyDown(grabThrowKey))
                if (Physics.SphereCast(ray.origin, 0.25f, ray.direction, out hit, maxGrabDistance)) {
                    var grabbable = hit.collider.gameObject.GetComponent<Grabbable>();
                    if (grabbable) {
                        // Get the GrapplingHook component
                        var grappling = playerDependencies.GetComponent<GrapplingHook>();

                        // Check if the player is directly connected to this object via an active grapple
                        for (int i = grappling.ropes.Count - 1; i >= 0; i--) {
                            var rope = grappling.ropes[i];

                            // Check if this rope connects the grabbed object to the player
                            // For active grapples (not latched), connectedObject1 is the target and connectedObject2 is the player
                            bool isPlayerDirectlyConnected = rope.connectedObject1 == hit.collider.gameObject &&
                                                             rope.connectedObject2 &&
                                                             rope.connectedObject2.CompareTag("Player");

                            if (isPlayerDirectlyConnected) {
                                // Cut this specific rope that connects player to target
                                grappling.DestroyRope(i);
                                break; // Only cut the first rope found that connects player to target
                            }
                        }

                        // Proceed with normal grab
                        GrabObject(grabbable.GetComponent<Rigidbody>());
                    }
                }
        }

        void ThrowObject(bool drop = false) {
            if (drop == false) grabbedObject.AddForce(playerDependencies.cam.transform.forward * throwForce, ForceMode.Impulse);
            grabbedObject = null;
            if (grabbedID) grabbedID.onReset -= OnGrabbedObjectReset;
            grabbedID = null;
            playerDependencies.isGrabbing = false;
            audioSource.PlayOneShot(throwSound);
        }

        void GrabObject(Rigidbody hitRigidbody) {
            hitRigidbody.TryGetComponent(out grabbedID);
            if (grabbedID) grabbedID.onReset += OnGrabbedObjectReset;

            grabPoint.position = hit.point;
            grabbedObject = hitRigidbody;
            grabbedObject.linearVelocity = Vector3.zero;
            grabbedObject.angularVelocity = Vector3.zero;
            playerDependencies.isGrabbing = true;
            audioSource.PlayOneShot(grabSound);
        }

        void OnGrabbedObjectReset(bool wasSpawned) {
            ThrowObject(true);
        }

        void Hold() {
            if (playerDependencies.isGrabbing && grabbedObject != null) {
                var targetPosition = grabPoint.position + grabPoint.forward * (grabbedObject.transform.localScale.magnitude * grabbingDistance);
                grabbedObject.linearVelocity = grabSpeed * (targetPosition - grabbedObject.transform.position);
            }
        }
    }
}