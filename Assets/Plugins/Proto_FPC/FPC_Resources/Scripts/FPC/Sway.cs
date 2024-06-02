#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Sway : MonoBehaviour
    {
        // Dependencies
        [Header("Dependencies")]
        [SerializeField] Dependencies dependencies;
        
        // Sway properties
        [Header("Sway Properties")]
        [SerializeField] float amount = 25f;
        [SerializeField] float maxAmount = 30f;
        [SerializeField] float positionDelay = 0.05f;
        [SerializeField] float smoothness = 3f;
        Vector3 drag;
        
        Vector3 localPosition;
        
        Vector3 localPositionLeft; // Position for left hand
        Quaternion localRotation;
        Quaternion localRotationLeft; // Rotation for left hand
        Quaternion newRotation;
        Quaternion newRotationLeft; // New rotation for left hand
        Transform swayPivotLeft; // Add second sway pivot
        
        Transform swayPivotRight;
        
        // Helpers
        float y, z;
        
        //-----------------------
        
        // Functions
        ///////////////
        
        void Start() {
            Setup();
        }
        
        void LateUpdate() {
            ControlSway();
            ControlPositionDelay();
        }
        
        //-----------------------
        
        void Setup() {
            // Setup dependencies
            swayPivotRight = dependencies.swayPivotRight;
            swayPivotLeft = dependencies.swayPivotLeft; // Initialize second sway pivot
            
            // Set local rotation
            localRotation = swayPivotRight.localRotation;
            localRotationLeft = swayPivotLeft.localRotation; // Initialize left hand rotation
            
            // Set local position
            localPosition = swayPivotRight.localPosition;
            localPositionLeft = swayPivotLeft.localPosition; // Initialize left hand position
        }
        
        void ControlSway() {
            if (!dependencies.isInspecting) {
                // Record input axis
                y = Input.GetAxis("Mouse Y") * amount;
                z = -Input.GetAxis("Mouse X") * amount;
                
                // Clamp input value
                y = Mathf.Clamp(y, -maxAmount, maxAmount);
                z = Mathf.Clamp(z, -maxAmount, maxAmount);
                
                // Apply rotation for right hand
                float smooth = smoothness * Time.deltaTime;
                newRotation = Quaternion.Euler(localRotation.x, localRotation.y + y, localRotation.z + z);
                swayPivotRight.localRotation = Quaternion.Lerp(swayPivotRight.localRotation, newRotation, smooth);
                
                // Apply rotation for left hand
                newRotationLeft = Quaternion.Euler(localRotationLeft.x, localRotationLeft.y + y, localRotationLeft.z + z);
                swayPivotLeft.localRotation = Quaternion.Lerp(swayPivotLeft.localRotation, newRotationLeft, smooth);
            }
        }
        
        void ControlPositionDelay() {
            if (!dependencies.isInspecting) {
                // Calculate drag when moving
                drag = new Vector3(-Input.GetAxisRaw("Horizontal") * positionDelay, 0f, -Input.GetAxisRaw("Vertical") * positionDelay);
                
                // Apply position drag for right hand
                float smooth = smoothness * Time.deltaTime;
                swayPivotRight.localPosition = Vector3.Lerp(swayPivotRight.localPosition, localPosition + drag, smooth);
                
                // Apply position drag for left hand
                swayPivotLeft.localPosition = Vector3.Lerp(swayPivotLeft.localPosition, localPositionLeft + drag, smooth);
            }
        }
    }
}