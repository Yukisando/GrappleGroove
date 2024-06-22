#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Perspective : MonoBehaviour
    {
        //Camera Properties
        [Header("Camera Properties")]
        [SerializeField] float fov = 60f;
        [SerializeField] float minRotationLimit = -90f;
        [SerializeField] float maxRotationLimit = 90f;
        [SerializeField] float sensX = 180f;
        [SerializeField] float sensY = 180f;
        [SerializeField] float multiplier = 0.01f;
        [SerializeField] float smoothness = 17f;
        [SerializeField] float lookTiltAmount = 6f;
        [SerializeField] float lookTiltSpeed = 12f;
        [SerializeField] float allTiltResetSpeed = 10f;

        //Helpers
        float mouseX;
        float mouseY;
        Transform orientation;
        PlayerDependencies playerDependencies;
        bool skipLerp;
        Quaternion targetRot;
        float xRotation;
        float yRotation;

        //-----------------

        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }

        void Start() {
            Setup();
        }

        void Update() {
            Time.timeScale = Cursor.lockState == CursorLockMode.Locked ? 1 : 0;
            MouseInput();
        }

        void LateUpdate() {
            CalculatePerspective();
        }

        //-----------------

        void Setup() {
            //Set cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Setup playerDependencies
            orientation = playerDependencies.orientation;

            //Set fov
            playerDependencies.cam.fieldOfView = fov;
        }

        void MouseInput() {
            if (!playerDependencies.isInspecting) {
                //Get and set input axis
                mouseX = Input.GetAxisRaw("Mouse X");
                mouseY = Input.GetAxisRaw("Mouse Y");

                //Calculate rotation
                yRotation += mouseX * sensX * multiplier;
                xRotation -= mouseY * sensY * multiplier;

                //Limit rotation
                xRotation = Mathf.Clamp(xRotation, minRotationLimit, maxRotationLimit);
            }
        }

        void CalculatePerspective() {
            if (!playerDependencies.isInspecting) {
                //Perspective tilt
                if (!playerDependencies.isWallRunning && !playerDependencies.isSliding && mouseX != 0) {
                    float tiltSpeed = lookTiltSpeed * Time.deltaTime;
                    playerDependencies.tilt = Mathf.Lerp(playerDependencies.tilt, -mouseX * lookTiltAmount, tiltSpeed);
                }

                //Apply rotation
                if (skipLerp) {
                    playerDependencies.cam.transform.localRotation = targetRot;
                    orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
                    skipLerp = false;
                }
                else {
                    float smooth = smoothness * Time.deltaTime;
                    targetRot = Quaternion.Euler(xRotation, 0f, playerDependencies.tilt);
                    playerDependencies.cam.transform.localRotation = Quaternion.Lerp(playerDependencies.cam.transform.localRotation, targetRot, smooth);
                    orientation.transform.rotation = Quaternion.Lerp(orientation.transform.rotation, Quaternion.Euler(0, yRotation, 0), smooth);
                }
            }

            //Reset tilt
            if (!playerDependencies.isWallRunning && !playerDependencies.isSliding && !playerDependencies.isVaulting && mouseX == 0) {
                float allTiltSpeed = allTiltResetSpeed * Time.deltaTime;
                playerDependencies.tilt = Mathf.Lerp(playerDependencies.tilt, 0, allTiltSpeed);
            }
        }

        public void SetCameraRotation(Quaternion rotation) {
            playerDependencies.cam.transform.localRotation = rotation;

            // Extract x and y rotation from the given quaternion
            var eulerRotation = rotation.eulerAngles;
            xRotation = eulerRotation.x;
            yRotation = eulerRotation.y;

            // Set target rotation and skip lerping
            targetRot = rotation;
            skipLerp = true;
        }
    }
}