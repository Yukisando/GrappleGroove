#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class GrabThrow : MonoBehaviour
    {
        //Input properties
        [Header("Input Properties")]
        [SerializeField] KeyCode grabThrowKey = KeyCode.G;
        
        //Grab and throw properties
        [Header("Grab/Throw Properties")]
        public float maxGrabDistance = 8f;
        [SerializeField] float grabbingDistance = 0.3f;
        [SerializeField] float grabSpeed = 15;
        [SerializeField] float throwForce = 800f;
        
        //Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip grabSound;
        [SerializeField] AudioClip throwSound;
        
        AudioSource audioSource;
        
        // Reference to the Crosshair script
        Crosshair crosshair;
        Rigidbody grabbedObject;
        
        Transform grabPoint;
        RaycastHit hit;
        int mask;
        int originalLayer;
        Transform originalParent;
        PlayerDependencies playerDependencies;
        Ray ray;
        
        //-----------------------
        
        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
            grabPoint = playerDependencies.grabPoint;
            audioSource = playerDependencies.audioSourceTop;
            
            //Create and make grab point A kinematic rigidbody
            grabPoint.gameObject.AddComponent<Rigidbody>().useGravity = false;
            grabPoint.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            mask = ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Player"));
        }
        
        void Update() {
            GrabHoldThrow(); //- Line 88
        }
        
        void FixedUpdate() {
            Hold(); //- Line 137
        }
        
        void GrabHoldThrow() {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            // Update mask to also ignore player layer
            
            switch (playerDependencies.isGrabbing) {
                case true when grabbedObject != null && Input.GetKeyDown(grabThrowKey):
                    ThrowObject();
                    break;
                case false when !playerDependencies.isInspecting && Physics.Raycast(ray.origin, ray.direction, out hit, maxGrabDistance, mask, QueryTriggerInteraction.Ignore):
                    HandleRaycastHit();
                    break;
            }
        }
        
        void ThrowObject() {
            grabbedObject.AddForce(playerDependencies.cam.transform.forward * throwForce, ForceMode.Impulse);
            grabbedObject.gameObject.layer = originalLayer;
            grabbedObject = null;
            playerDependencies.isGrabbing = false;
            audioSource.PlayOneShot(throwSound);
        }
        
        void HandleRaycastHit() {
            var hitRigidbody = hit.collider.gameObject.GetComponent<Rigidbody>();
            if (hitRigidbody != null && !hitRigidbody.isKinematic && hit.collider.CompareTag("Grab") && Input.GetKeyDown(grabThrowKey)) {
                GrabObject(hitRigidbody);
            }
        }
        
        void GrabObject(Rigidbody hitRigidbody) {
            grabPoint.position = hit.point;
            grabbedObject = hitRigidbody;
            grabbedObject.linearVelocity = Vector3.zero;
            grabbedObject.angularVelocity = Vector3.zero;
            playerDependencies.isGrabbing = true;
            originalLayer = grabbedObject.gameObject.layer;
            audioSource.PlayOneShot(grabSound);
        }
        
        void Hold() {
            if (playerDependencies.isGrabbing && grabbedObject != null) {
                // Move the grabbed object towards grab point, maintaining a slight distance
                var targetPosition = grabPoint.position + grabPoint.forward * grabbedObject.transform.localScale.magnitude * grabbingDistance;
                grabbedObject.linearVelocity = grabSpeed * (targetPosition - grabbedObject.transform.position);
            }
        }
    }
}