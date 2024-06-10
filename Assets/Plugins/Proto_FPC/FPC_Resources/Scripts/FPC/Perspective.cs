#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Perspective : MonoBehaviour
    {
        [Header("PlayerDependencies")]
        [SerializeField] PlayerDependencies playerDependencies;

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

        Camera cam;

        //Helpers
        float mouseX;
        float mouseY;
        Transform orientation;
        Quaternion targetRot;
        float xRotation;
        float yRotation;

        //-----------------

        //Functions
        ///////////////

        void Start() {
            Setup(); //- Line 67
        }

        void Update() {
            Time.timeScale = Cursor.lockState == CursorLockMode.Locked ? 1 : 0;
            MouseInput(); //- Line 81
        }

        void LateUpdate() {
            CalculatePerspective(); //- Line 98
        }

        //-----------------

        void Setup() {
            //Set cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Setup playerDependencies
            cam = playerDependencies.cam;
            orientation = playerDependencies.orientation;

            //Set fov
            cam.fieldOfView = fov;
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
                float smooth = smoothness * Time.deltaTime;
                targetRot = Quaternion.Euler(xRotation, 0f, playerDependencies.tilt);
                cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, targetRot, smooth);
                orientation.transform.rotation = Quaternion.Lerp(orientation.transform.rotation, Quaternion.Euler(0, yRotation, 0), smooth);
            }

            //Reset tilt
            if (!playerDependencies.isWallRunning && !playerDependencies.isSliding && !playerDependencies.isVaulting && mouseX == 0) {
                float allTiltSpeed = allTiltResetSpeed * Time.deltaTime;
                playerDependencies.tilt = Mathf.Lerp(playerDependencies.tilt, 0, allTiltSpeed);
            }
        }
    }
}