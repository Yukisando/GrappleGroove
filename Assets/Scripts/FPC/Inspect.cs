#region

using Unity.Netcode;
using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Inspect : NetworkBehaviour
    {
        [Header("Input Properties")]
        [SerializeField] KeyCode inspectKey = KeyCode.F;

        [Header("Inspection Properties")]
        public float maxPickupDistance = 6;
        [SerializeField] float pickupSpeed = 5f;
        [SerializeField] float rotateSpeed = 2f;
        [SerializeField] float zoomSpeed = 0.2f;
        [SerializeField] float objectSizeFactor = 1.5f; // New: Factor to adjust position based on object size

        [Header("Audio Properties")]
        [SerializeField] AudioClip pickUpSound;
        [SerializeField] AudioClip putDownSound;
        [SerializeField] AudioClip zoomSound;

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

        float rotX, rotY;

        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }

        void Start() {
            if (!IsOwner) return;

            Setup();
        }

        void Update() {
            if (!IsOwner) return;

            Inspection();
        }

        void Setup() {
            cam = playerDependencies.cam;
            inspectPoint = playerDependencies.inspectPoint;
            audioSource = playerDependencies.audioSourceTop;
        }

        void Inspection() {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (playerDependencies.isInspecting) {
                if (Input.GetKeyDown(inspectKey)) {
                    EndInspection();
                }
            }
            else if (!playerDependencies.isInspecting && !playerDependencies.isGrabbing) {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, maxPickupDistance)) {
                    var inspectable = hit.collider.gameObject.GetComponent<Inspectable>();
                    if (inspectable != null && Input.GetKeyDown(inspectKey)) {
                        StartInspection(inspectable.gameObject);
                    }
                }
            }

            if (playerDependencies.isInspecting && inspectedObject != null) {
                InspectObject();
            }
        }

        void StartInspection(GameObject obj) {
            playerDependencies.isInspecting = true;
            inspectedObject = obj;
            objectOrigin = inspectedObject.transform.position;
            objectRotation = inspectedObject.transform.rotation;
            originalDistance = inspectPoint.localPosition;

            var rb = inspectedObject.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.isKinematic = true;
            }

            inspectedObject.GetComponent<Collider>().enabled = false;
            audioSource.PlayOneShot(pickUpSound);

            // Adjust inspect point based on object size
            AdjustInspectPointPosition();
        }

        void EndInspection() {
            playerDependencies.isInspecting = false;
            inspectedObject.GetComponent<Collider>().enabled = true;
            audioSource.PlayOneShot(putDownSound);

            inspectedObject.transform.position = objectOrigin;
            inspectedObject.transform.rotation = objectRotation;
            inspectPoint.localPosition = originalDistance;

            rotX = 0f;
            rotY = 0f;

            var rb = inspectedObject.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.isKinematic = false;
            }

            inspectedObject = null;
        }

        void InspectObject() {
            inspectedObject.transform.position = Vector3.Lerp(inspectedObject.transform.position, inspectPoint.position, pickupSpeed * Time.deltaTime);

            rotX += Input.GetAxisRaw("Mouse X");
            rotY += Input.GetAxisRaw("Mouse Y");

            inspectedObject.transform.localRotation = Quaternion.Euler(inspectedObject.transform.localRotation.x + rotY * rotateSpeed, inspectedObject.transform.localRotation.y - rotX * rotateSpeed, 0);

            if (Input.mouseScrollDelta.y != 0) {
                inspectPoint.localPosition = new Vector3(inspectPoint.localPosition.x, inspectPoint.localPosition.y, inspectPoint.localPosition.z + Input.mouseScrollDelta.y * zoomSpeed);
                audioSource.PlayOneShot(zoomSound);
            }
        }

        void AdjustInspectPointPosition() {
            // Calculate the object's size
            var bounds = CalculateObjectBounds(inspectedObject);
            float objectSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

            // Adjust the inspect point's forward position based on the object's size
            var adjustedPosition = inspectPoint.localPosition;
            adjustedPosition.z = objectSize * objectSizeFactor;
            inspectPoint.localPosition = adjustedPosition;
        }

        Bounds CalculateObjectBounds(GameObject obj) {
            var bounds = new Bounds(obj.transform.position, Vector3.zero);
            var renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (var rd in renderers) {
                bounds.Encapsulate(rd.bounds);
            }
            return bounds;
        }
    }
}