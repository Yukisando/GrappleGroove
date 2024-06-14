#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Inspect : MonoBehaviour
    {
        //Input properties
        [Header("Input Properties")]
        [SerializeField] KeyCode inspectKey = KeyCode.F;
        
        //Inspection properties
        [Header("Inspection Properties")]
        public float maxPickupDistance = 6;
        [SerializeField] float pickupSpeed = 5f;
        [SerializeField] float rotateSpeed = 2f;
        [SerializeField] float zoomSpeed = 0.2f;
        
        //Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip pickUpSound;
        [SerializeField] AudioClip putDownSound;
        [SerializeField] AudioClip zoomSound;
        
        //Inspect & Disable Lists
        public LayerMask inspectLayerMask;
        AudioSource audioSource;
        Camera cam;
        RaycastHit hit;
        GameObject inspectedObject;
        Transform inspectPoint;
        
        Vector3 objectOrigin;
        
        Quaternion objectRotation;
        Vector3 originalDistance;
        
        PlayerDependencies playerDependencies;
        
        Ray ray;
        
        //Helpers
        float rotX, rotY;
        
        //-----------------------
        
        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }
        
        void Start() {
            Setup();
        }
        
        void Update() {
            Inspection();
        }
        
        //-----------------------
        
        void Setup() {
            //Setup playerDependencies
            cam = playerDependencies.cam;
            inspectPoint = playerDependencies.inspectPoint;
            audioSource = playerDependencies.audioSourceTop;
        }
        
        void Inspection() {
            //Track the mouse position for raycasting
            ray = cam.ScreenPointToRay(Input.mousePosition);
            
            //Revert if already inspecting
            if (playerDependencies.isInspecting) {
                if (Input.GetKeyDown(inspectKey)) {
                    playerDependencies.isInspecting = false;
                    
                    //Enable object collider
                    inspectedObject.GetComponent<Collider>().enabled = true;
                    
                    //Audio
                    audioSource.PlayOneShot(putDownSound);
                }
            }
            
            //Inspect if not already
            else if (!playerDependencies.isInspecting && !playerDependencies.isGrabbing) {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, maxPickupDistance, inspectLayerMask, QueryTriggerInteraction.Ignore))
                
                    if (Input.GetKeyDown(inspectKey)) {
                        playerDependencies.isInspecting = true;
                        
                        //Record the original position and rotation of the inspected object
                        inspectedObject = hit.collider.gameObject;
                        objectOrigin = inspectedObject.transform.position;
                        objectRotation = inspectedObject.transform.rotation;
                        originalDistance = inspectPoint.localPosition;
                        
                        //Set kinematic if rigidbody found
                        if (inspectedObject != null && inspectedObject.GetComponent<Rigidbody>())
                            inspectedObject.GetComponent<Rigidbody>().isKinematic = true;
                        
                        //Disable object collider
                        inspectedObject.GetComponent<Collider>().enabled = false;
                        
                        //Audio
                        audioSource.PlayOneShot(pickUpSound);
                    }
            }
            
            //Inspection
            if (playerDependencies.isInspecting && inspectedObject != null) {
                //Move object position to inspection point
                inspectedObject.transform.position = Vector3.Lerp(inspectedObject.transform.position, inspectPoint.position, pickupSpeed * Time.deltaTime);
                
                //Get and set mouse input axis
                rotX += Input.GetAxisRaw("Mouse X");
                rotY += Input.GetAxisRaw("Mouse Y");
                
                //Set object rotation based on input
                inspectedObject.transform.localRotation = Quaternion.Euler(inspectedObject.transform.localRotation.x + rotY * rotateSpeed, inspectedObject.transform.localRotation.y - rotX * rotateSpeed, 0);
                
                //Set inspection distance
                if (Input.mouseScrollDelta.y != 0) {
                    inspectPoint.localPosition = new Vector3(inspectPoint.localPosition.x, inspectPoint.localPosition.y, inspectPoint.localPosition.z + Input.mouseScrollDelta.y * zoomSpeed);
                    
                    //Audio
                    audioSource.PlayOneShot(zoomSound);
                }
            }
            
            //Exit inspection
            else if (!playerDependencies.isInspecting && inspectedObject != null) {
                //Reset object position and rotation to original
                inspectedObject.transform.position = objectOrigin;
                inspectedObject.transform.rotation = objectRotation;
                inspectPoint.localPosition = originalDistance;
                
                rotX = 0f;
                rotY = 0f;
                
                //Revert kinematic if rigidbody found
                if (inspectedObject.GetComponent<Rigidbody>())
                    inspectedObject.GetComponent<Rigidbody>().isKinematic = false;
                
                inspectedObject = null;
            }
        }
    }
}