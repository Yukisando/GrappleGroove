#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class WallRun : MonoBehaviour
    {
        //Detection properties
        [Header("Detection Properties")]
        [SerializeField] float wallCheckDistance = 1f;
        [SerializeField] float minOffGroundHeight = 1f;

        //Wall run properties
        [Header("Wall Run Properties")]
        [SerializeField] float onWallGravity = 2f;
        [SerializeField] float onWallJumpAmount = 8f;

        //Wall Run camera properties
        [Header("Wall Run Camera Properties")]
        [SerializeField] float onWallFov = 65f;
        [SerializeField] float fovChangeSpeed = 10f;
        [SerializeField] float onWallTilt = 20f;
        [SerializeField] float onWallTiltSpeed = 5f;

        //Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip wallJumpSound;
        AudioSource audioSource;
        Camera cam;
        CapsuleCollider cc;

        //Helpers
        float fov = 60;
        bool gravityChange;
        Vector3 jumpDirection;
        bool jumping;

        RaycastHit leftWallHit;
        Transform orientation;
        [Header("PlayerDependencies")]
        PlayerDependencies playerDependencies;

        Rigidbody rb;
        RaycastHit rightWallHit;

        bool wallLeft;
        bool wallRight;

        //------------------------

        //Functions
        ///////////////

        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }

        void Start() {
            Setup(); //- Line 84
        }

        void Update() {
            CheckWall(); //- Line 104
            WallRunning(); //- Line 111
        }

        void FixedUpdate() {
            WallRunPhysics(); //- Line 200
        }

        //------------------------

        void Setup() {
            //Setup playerDependencies
            rb = playerDependencies.rb;
            cam = playerDependencies.cam;
            cc = playerDependencies.cc;
            orientation = playerDependencies.orientation;
            audioSource = playerDependencies.audioSourceBottom;

            //Record defaulted fov
            fov = cam.fieldOfView;
        }

        //Check if possible to wall run (is off the ground)
        bool CanWallRun() {
            return !Physics.Raycast(rb.transform.position + new Vector3(0, cc.height / 2, 0), Vector3.down, minOffGroundHeight);
        }

        //Check sides for walls
        void CheckWall() {
            wallLeft = Physics.Raycast(rb.transform.position, -orientation.right, out leftWallHit, wallCheckDistance);
            wallRight = Physics.Raycast(rb.transform.position, orientation.right, out rightWallHit, wallCheckDistance);
        }

        //Wall run
        void WallRunning() {
            if (CanWallRun()) {
                if (!playerDependencies.isGrounded && (wallLeft || wallRight)) {
                    rb.useGravity = false;
                    playerDependencies.isWallRunning = true;

                    //Transition camera FOV
                    float fovSpeed = fovChangeSpeed * Time.deltaTime;
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, onWallFov, fovSpeed);

                    //Transition tilt value
                    if (wallLeft) {
                        float tiltSpeed = onWallTiltSpeed * Time.deltaTime;
                        playerDependencies.tilt = Mathf.Lerp(playerDependencies.tilt, -onWallTilt, tiltSpeed);
                    }
                    else if (wallRight) {
                        float tiltSpeed = onWallTiltSpeed * Time.deltaTime;
                        playerDependencies.tilt = Mathf.Lerp(playerDependencies.tilt, onWallTilt, tiltSpeed);
                    }

                    //Toggle wall gravity
                    if (!gravityChange) gravityChange = true;

                    //Wall run jump
                    if (Input.GetKeyDown(KeyCode.Space)) {
                        //Jump to the right
                        if (wallLeft) {
                            jumpDirection = rb.transform.up * 1.8f + leftWallHit.normal;

                            if (!jumping) jumping = true;
                        }

                        //Jump to the left
                        else if (wallRight) {
                            jumpDirection = rb.transform.up + rightWallHit.normal;

                            if (!jumping) jumping = true;
                        }

                        //Audio
                        audioSource.PlayOneShot(wallJumpSound);
                    }
                }

                //On wall run exit
                else {
                    jumping = false;
                    gravityChange = false;

                    rb.useGravity = true;

                    playerDependencies.isWallRunning = false;
                }
            }

            //Not wall running
            else {
                jumping = false;
                gravityChange = false;

                rb.useGravity = true;

                playerDependencies.isWallRunning = false;

                //Set FOV to default
                float fovSpeed = fovChangeSpeed * Time.deltaTime;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, fovSpeed);
            }
        }

        void WallRunPhysics() {
            //Wall run gravity
            if (gravityChange) rb.AddForce(Vector3.down * (onWallGravity * 0.01f), ForceMode.VelocityChange);

            //Wall run jump
            if (jumping) {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(jumpDirection * (onWallJumpAmount * 0.05f), ForceMode.VelocityChange);
            }
        }
    }
}