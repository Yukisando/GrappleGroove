//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using UnityEngine;
using PrototypeFPC;

namespace PrototypeFPC
{
    public class Sway : MonoBehaviour
    {
        //Dependencies
        [Header("Dependencies")]
        [SerializeField] Dependencies dependencies;

        //Sway properties
        [Header("Sway Properties")]
        [SerializeField] float amount = 25f;
        [SerializeField] float maxAmount = 30f;
        [SerializeField] float positionDelay = 0.05f;
        [SerializeField] float smoothness = 3f;

        //Helpers
        float y, z;

        Transform swayPivot;
        Quaternion localRotation;
        Quaternion newRotation;

        Vector3 localPosition;
        Vector3 drag;


        //-----------------------


        //Functions
        ///////////////

        void Start()
        {
            Setup(); //- Line 57
        }

        void LateUpdate()
        {
            ControlSway(); //- Line 70
            ControlPositionDelay(); //- Line 90
        }


        //-----------------------


        void Setup()
        {
            //Setup dependencies
            swayPivot = dependencies.swayPivot;

            //Set local rotation
            localRotation = swayPivot.localRotation;

            //Set local position
            localPosition = swayPivot.localPosition;
        }


        void ControlSway()
        {
            if(!dependencies.isInspecting)
            {
                //Record input axis
                y = Input.GetAxis("Mouse Y") * amount;
                z = -Input.GetAxis("Mouse X") * amount;

                //Clamp input value
                y = Mathf.Clamp(y, -maxAmount, maxAmount);
                z = Mathf.Clamp(z, -maxAmount, maxAmount);

                //Apply rotation
                var smooth = smoothness * Time.deltaTime;
                newRotation = Quaternion.Euler(localRotation.x, localRotation.y + y, localRotation.z + z);
                swayPivot.localRotation = Quaternion.Lerp(swayPivot.localRotation, newRotation, smooth);
            }
        }


        void ControlPositionDelay()
        {
            if(!dependencies.isInspecting)
            {
                //Calculate drag when moving
                drag = new Vector3(-Input.GetAxisRaw("Horizontal") * positionDelay, 0f, -Input.GetAxisRaw("Vertical") * positionDelay);

                //Apply position drag
                var smooth = smoothness * Time.deltaTime;
                swayPivot.localPosition = Vector3.Lerp(swayPivot.localPosition, localPosition + drag, smooth);
            }
        }
    }
}
