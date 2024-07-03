#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Perspective : MonoBehaviour
    {
        [Header("Camera Properties")]
        [SerializeField] float fov = 60f;
        [SerializeField] float minRotationLimit = -90f;
        [SerializeField] float maxRotationLimit = 90f;
        [SerializeField] float sensitivity = 180f;
        [SerializeField] float lookTiltAmount = 6f;
        [SerializeField] float lookTiltSpeed = 12f;
        [SerializeField] float tiltResetSpeed = 10f;
        Quaternion initialRotation;

        Vector2 mouseInput;
        PlayerDependencies playerDependencies;

        float xRotation;
        float yRotation;

        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Store the initial rotation
            initialRotation = transform.rotation;
        }

        void Start() {
            playerDependencies.cam.fieldOfView = fov;

            // Apply the initial rotation
            ForceOrientation(initialRotation);
        }

        void Update() {
            if (Cursor.lockState == CursorLockMode.Locked && !playerDependencies.isInspecting) {
                GetMouseInput();
                UpdateRotation();
            }
            Time.timeScale = Cursor.lockState == CursorLockMode.Locked ? 1 : 0;
        }

        void LateUpdate() {
            ApplyRotation();
            UpdateTilt();
        }

        void GetMouseInput() {
            if (!playerDependencies.isInspecting) {
                mouseInput.x = Input.GetAxisRaw("Mouse X") * sensitivity * 0.01f;
                mouseInput.y = Input.GetAxisRaw("Mouse Y") * sensitivity * 0.01f;
            }
        }

        void UpdateRotation() {
            yRotation += mouseInput.x;
            xRotation -= mouseInput.y;
            xRotation = Mathf.Clamp(xRotation, minRotationLimit, maxRotationLimit);
        }

        void ApplyRotation() {
            playerDependencies.cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, playerDependencies.tilt);
            playerDependencies.orientation.rotation = Quaternion.Slerp(playerDependencies.orientation.rotation, Quaternion.Euler(0, yRotation, 0), 0.1f);
        }

        void UpdateTilt() {
            if (!playerDependencies.isWallRunning && !playerDependencies.isSliding) {
                float tiltTarget = mouseInput.x != 0 ? -mouseInput.x * lookTiltAmount : 0f;
                float tiltSpeed = (mouseInput.x != 0 ? lookTiltSpeed : tiltResetSpeed) * Time.deltaTime;
                playerDependencies.tilt = Mathf.Lerp(playerDependencies.tilt, tiltTarget, tiltSpeed);
            }
        }

        public void ForceOrientation(Quaternion rotation) {
            playerDependencies.cam.transform.localRotation = rotation;
            playerDependencies.orientation.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

            var eulerRotation = rotation.eulerAngles;
            xRotation = eulerRotation.x;
            yRotation = eulerRotation.y;
        }
    }
}