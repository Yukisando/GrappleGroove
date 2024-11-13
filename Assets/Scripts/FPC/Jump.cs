#region

using Unity.Netcode;
using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Jump : NetworkBehaviour
    {
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

        float landTime;

        Vector3 newFallVelocity;

        //Helpers
        float nextTimeToJump;
        [Header("PlayerDependencies")]
        PlayerDependencies playerDependencies;
        Rigidbody rb;

        //-----------------------

        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }

        void Start() {
            if (!IsOwner) return;

            Setup(); //- Line 74
        }

        void Update() {
            if (!IsOwner) return;

            Land(); //- Line 117

            // Check if jump key is released
            if (Input.GetKeyUp(jumpKey)) jumpKeyReleased = true;
        }

        void FixedUpdate() {
            if (!IsOwner) return;

            SimulateJump(); //- Line 82
            Fall(); //- Line 102
        }

        //-----------------------

        void Setup() {
            // Setup playerDependencies
            rb = playerDependencies.rb;
            audioSource = playerDependencies.audioSourceBottom;
        }

        void Land() {
            // Toggle landing upon ground detection
            if (!playerDependencies.isGrounded && landed) landed = false;

            if (playerDependencies.isGrounded && !landed) {
                landed = true;
                landTime = Time.time; // Record the time when the player lands

                if (!playerDependencies.isVaulting)

                    // Audio
                    audioSource.PlayOneShot(landSound);
            }
        }

        void SimulateJump() {
            if (Input.GetKey(jumpKey) && jumpKeyReleased && playerDependencies.isGrounded && !playerDependencies.isWallRunning && !playerDependencies.isVaulting && !playerDependencies.isInspecting && Time.time >= nextTimeToJump && Time.time >= landTime + 0.1f) { // Check that at least 0.1 seconds have passed since the player landed
                // Jump cooldown rate
                nextTimeToJump = Time.time + 1f / coolDownRate;

                // Apply force if grounded
                if (playerDependencies.isGrounded) {
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
            if (!playerDependencies.isGrounded)
                rb.linearVelocity += Vector3.up * (Physics.gravity.y * Time.fixedDeltaTime);
        }
    }
}