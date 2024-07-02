#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Inspect : MonoBehaviour
    {
        [Header("Input Properties")]
        [SerializeField] KeyCode inspectKey = KeyCode.F;

        [Header("Inspection Properties")]
        public float maxPickupDistance = 6;
        [SerializeField] float pickupSpeed = 5f;
        [SerializeField] float rotateSpeed = 2f;
        [SerializeField] float zoomSpeed = 0.2f;

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
            Setup();
        }

        void Update() {
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
    }
}