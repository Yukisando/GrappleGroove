#region

using Unity.Netcode;
using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Sway : NetworkBehaviour
    {
        // Sway properties
        [Header("Sway Properties")]
        [SerializeField] float amount = 25f;
        [SerializeField] float maxAmount = 30f;
        [SerializeField] float positionDelay = 0.05f;
        [SerializeField] float smoothness = 3f;

        Vector3 drag;
        Vector3 localPositionLeft;
        Vector3 localPositionRight;
        Quaternion localRotationLeft;
        Quaternion localRotationRight;
        Quaternion newRotationLeft;
        Quaternion newRotationRight;

        // PlayerDependencies

        [Header("PlayerDependencies")]
        PlayerDependencies playerDependencies;
        Transform swayPivotLeft;
        Transform swayPivotRight;
        float y, z;

        // Functions
        ///////////////

        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }

        void Start() {
            if (!IsOwner) return;

            Setup();
        }

        void LateUpdate() {
            if (!IsOwner) return;

            ControlSway();
            ControlPositionDelay();
        }

        //-----------------------

        void Setup() {
            // Setup playerDependencies
            swayPivotRight = playerDependencies.swayPivotRight;
            swayPivotLeft = playerDependencies.swayPivotLeft; // Initialize second sway pivot

            // Set local rotation
            localRotationRight = swayPivotRight.localRotation;
            localRotationLeft = swayPivotLeft.localRotation; // Initialize left hand rotation

            // Set local position
            localPositionRight = swayPivotRight.localPosition;
            localPositionLeft = swayPivotLeft.localPosition; // Initialize left hand position
        }

        void ControlSway() {
            if (!playerDependencies.isInspecting) {
                // Record input axis
                y = Input.GetAxis("Mouse Y") * amount;
                z = -Input.GetAxis("Mouse X") * amount;

                // Clamp input value
                y = Mathf.Clamp(y, -maxAmount, maxAmount);
                z = Mathf.Clamp(z, -maxAmount, maxAmount);

                // Apply rotation for right hand
                float smooth = smoothness * Time.deltaTime;
                newRotationRight = Quaternion.Euler(localRotationRight.x, localRotationRight.y + y, localRotationRight.z + z);
                swayPivotRight.localRotation = Quaternion.Lerp(swayPivotRight.localRotation, newRotationRight, smooth);

                // Apply rotation for left hand
                newRotationLeft = Quaternion.Euler(localRotationLeft.x, localRotationLeft.y + y, localRotationLeft.z + z);
                swayPivotLeft.localRotation = Quaternion.Lerp(swayPivotLeft.localRotation, newRotationLeft, smooth);
            }
        }

        void ControlPositionDelay() {
            if (!playerDependencies.isInspecting) {
                // Calculate drag when moving
                drag = new Vector3(-Input.GetAxisRaw("Horizontal") * positionDelay, 0f, -Input.GetAxisRaw("Vertical") * positionDelay);

                // Apply position drag for right hand
                float smooth = smoothness * Time.deltaTime;
                swayPivotRight.localPosition = Vector3.Lerp(swayPivotRight.localPosition, localPositionRight + drag, smooth);

                // Apply position drag for left hand
                swayPivotLeft.localPosition = Vector3.Lerp(swayPivotLeft.localPosition, localPositionLeft + drag, smooth);
            }
        }
    }
}