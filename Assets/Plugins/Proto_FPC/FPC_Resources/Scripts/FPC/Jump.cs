#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Jump : MonoBehaviour
    {
        //Dependencies
        [Header("Dependencies")]
        [SerializeField] Dependencies dependencies;

        //Input
        [Header("Input Properties")]
        [SerializeField] KeyCode jumpKey = KeyCode.Space;

        //Jump properties
        [Header("Jumping Properties")]
        [SerializeField] float amount = 14f;
        [SerializeField] float coolDownRate = 15f;

        //Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip jumpSound;
        [SerializeField] AudioClip landSound;
        AudioSource audioSource;
        RaycastHit fallHit;
        bool jumpKeyReleased = true;
        bool landed = true;

        Vector3 newFallVelocity;

        //Helpers
        float nextTimeToJump;
        Rigidbody rb;

        //-----------------------

        //Functions
        ///////////////

        void Start() {
            Setup(); //- Line 74
        }

        void Update() {
            Land(); //- Line 117

            // Check if jump key is released
            if (Input.GetKeyUp(jumpKey)) jumpKeyReleased = true;
        }

        void FixedUpdate() {
            SimulateJump(); //- Line 82
            Fall(); //- Line 102
        }

        //-----------------------

        void Setup() {
            // Setup dependencies
            rb = dependencies.rb;
            audioSource = dependencies.audioSourceBottom;
        }

        // Initiate jump
        void SimulateJump() {
            if (Input.GetKey(jumpKey) && jumpKeyReleased && dependencies.isGrounded && !dependencies.isWallRunning && !dependencies.isVaulting && !dependencies.isInspecting && Time.time >= nextTimeToJump) {
                // Jump cooldown rate
                nextTimeToJump = Time.time + 1f / coolDownRate;

                // Apply force if grounded
                if (dependencies.isGrounded) {
                    // Apply upward force
                    rb.AddForce(Vector3.up * amount - Vector3.up * rb.linearVelocity.y, ForceMode.VelocityChange);

                    // Audio
                    audioSource.PlayOneShot(jumpSound);

                    // Set jumpKeyReleased to false since the jump key is pressed
                    jumpKeyReleased = false;
                }
            }
        }

        void Fall() {
            //Add some gravity
            if (!dependencies.isGrounded)
                rb.linearVelocity += Vector3.up * (Physics.gravity.y * Time.fixedDeltaTime);
        }

        void Land() {
            // Toggle landing upon ground detection
            if (!dependencies.isGrounded && landed) landed = false;

            if (dependencies.isGrounded && !landed) {
                landed = true;

                if (!dependencies.isVaulting)

                    // Audio
                    audioSource.PlayOneShot(landSound);
            }
        }
    }
}