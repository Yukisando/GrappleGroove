//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using UnityEngine;
using PrototypeFPC;

namespace PrototypeFPC
{
    public class Perspective : MonoBehaviour
    {
        //Dependencies
        [Header("Dependencies")]
        [SerializeField] Dependencies dependencies;

        //Camera Properties
        [Header("Camera Properties")]
        [SerializeField] float fov = 60f;
        [SerializeField] float minRotationLimit = -90f;
        [SerializeField] float maxRotationLimit = 90f;
        [SerializeField] float sensX = 180f;
        [SerializeField] float sensY = 180f;
        [SerializeField]float multiplier = 0.01f;
        [SerializeField] float smoothness = 17f;
        [SerializeField] float lookTiltAmount = 6f;
        [SerializeField] float lookTiltSpeed = 12f;
        [SerializeField] float allTiltResetSpeed = 10f;

        //Helpers
        float mouseX;
        float mouseY;
        float xRotation;
        float yRotation;
        
        Camera cam;
        Transform orientation;
        Quaternion targetRot;


        //-----------------


        //Functions
        ///////////////

        void Start()
        {
            Setup(); //- Line 67
        }

        void Update()
        {
            MouseInput(); //- Line 81
        }

        void LateUpdate()
        {
            CalculatePerspective(); //- Line 98
        }


        //-----------------
        

        void Setup()
        {
            //Set cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Setup dependencies
            cam = dependencies.cam;
            orientation = dependencies.orientation;

            //Set fov
            cam.fieldOfView = fov;
        }

        void MouseInput()
        {
            if(!dependencies.isInspecting)
            {
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

        void CalculatePerspective()
        {
            if(!dependencies.isInspecting)
            {
                //Perspective tilt
                if(!dependencies.isWallRunning && !dependencies.isSliding && mouseX != 0)
                {
                    var tiltSpeed = lookTiltSpeed * Time.deltaTime;
                    dependencies.tilt = Mathf.Lerp(dependencies.tilt, -mouseX * lookTiltAmount, tiltSpeed);
                }

                //Apply rotation
                var smooth = smoothness * Time.deltaTime;
                targetRot = Quaternion.Euler(xRotation, 0f, dependencies.tilt);
                cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, targetRot, smooth);
                orientation.transform.rotation = Quaternion.Lerp(orientation.transform.rotation, Quaternion.Euler(0, yRotation, 0), smooth);
            }

            //Reset tilt
            if(!dependencies.isWallRunning && !dependencies.isSliding && !dependencies.isVaulting && mouseX == 0)
            {
                var allTiltSpeed = allTiltResetSpeed * Time.deltaTime;
                dependencies.tilt = Mathf.Lerp(dependencies.tilt, 0, allTiltSpeed);
            }
        }
    }
}
