#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Movement : MonoBehaviour
    {
        //Input
        [Header("Input Properties")]
        [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
        
        //Movement properties
        [Header("Movement Properties")]
        [SerializeField] float walkSpeed = 6.5f;
        [SerializeField] float sprintSpeed = 12f;
        [SerializeField] float acceleration = 70f;
        [SerializeField] float multiplier = 10f;
        [SerializeField] float airMultiplier = 0.4f;
        
        //Tilt Properties
        [Header("Tilt Properties")]
        [SerializeField] float strafeTilt = 8f;
        [SerializeField] float stafeTiltSpeed = 12f;
        
        //Drag properties
        [Header("Drag Properties")]
        [SerializeField] float groundDrag = 6f;
        [SerializeField] float airDrag = 1f;
        
        //Ground detection
        [Header("Ground Detection Properties")]
        [SerializeField] LayerMask groundLayer;
        [SerializeField] Transform groundCheck;
        [SerializeField] float groundCheckRadius = 0.2f;
        
        //Footstep properties
        [Header("Footstep Properties")]
        [SerializeField] AnimationCurve footstepCurve;
        [SerializeField] float footstepMultiplier = 0.17f;
        [SerializeField] float footstepRate = 0.25f;
        
        //Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip[] footstepSound;
        [HideInInspector]
        [SerializeField]
        List<int>
            playedRandom,
            randomFilter;
        readonly float playerHeight = 2f;
        AudioSource audioSource;
        
        Camera cam;
        float curveTime;
        float horizontalMovement;
        
        //Helpers
        float moveAmount;
        
        Vector3 moveDirection;
        Transform orientation;
        [Header("PlayerDependencies")]
        PlayerDependencies playerDependencies;
        int randomNum;
        Rigidbody rb;
        
        RaycastHit slopeHit;
        Vector3 slopeMoveDirection;
        float verticalMovement;
        
        //-----------------------
        
        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }
        
        void Start() {
            Setup(); //- Line 113
        }
        
        void Update() {
            GroundCheck(); //- Line 132
            CalculateDirection(); //- Line 139
            CalculateSlope(); //- Line 151
            ControlSpeed(); //- Line 181
            ControlDrag(); //- Line 198
            StrafeTilt(); //- Line 215
            Footsteps(); //- Line 235
        }
        
        void FixedUpdate() {
            Move(); //- Line 158
        }
        
        //---------------------------
        
        void Setup() {
            //Set player on the ignore raycast layer
            transform.gameObject.layer = 2;
            
            //Setup playerDependencies
            rb = playerDependencies.rb;
            cam = playerDependencies.cam;
            orientation = playerDependencies.orientation;
            audioSource = playerDependencies.audioSourceBottom;
            
            //Set rigidbody properties
            rb.freezeRotation = true;
            rb.mass = 50;
        }
        
        //Check if grounded
        void GroundCheck() {
            playerDependencies.isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }
        
        //Calculate input direction
        void CalculateDirection() {
            //Get keyboard input axis
            horizontalMovement = Input.GetAxisRaw("Horizontal");
            verticalMovement = Input.GetAxisRaw("Vertical");
            
            //Set calculated direction
            moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        }
        
        //Calculate slope
        void CalculateSlope() {
            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        }
        
        //Apply player movement
        void Move() {
            //Grounded & NOT on slope
            if (playerDependencies.isGrounded && !playerDependencies.isInspecting && !OnSlope() && !playerDependencies.isSliding) rb.AddForce(moveDirection.normalized * moveAmount * multiplier, ForceMode.Acceleration);
            
            //Grounded & on slope
            if (playerDependencies.isGrounded && OnSlope()) rb.AddForce(slopeMoveDirection.normalized * (moveAmount * multiplier), ForceMode.Acceleration);
            
            //Not grounded & in the air
            if (!playerDependencies.isGrounded) rb.AddForce(moveDirection.normalized * (moveAmount * multiplier * airMultiplier), ForceMode.Acceleration);
        }
        
        //Speed control
        void ControlSpeed() {
            //Sprinting
            if (Input.GetKey(sprintKey) && playerDependencies.isGrounded)
                moveAmount = Mathf.Lerp(moveAmount, sprintSpeed, acceleration * Time.deltaTime);
            
            //Walking
            else
                moveAmount = Mathf.Lerp(moveAmount, walkSpeed, acceleration * Time.deltaTime);
        }
        
        //Add drag to movement
        void ControlDrag() {
            // Target drag value
            float targetDrag = playerDependencies.isGrounded && !playerDependencies.isSliding ? groundDrag : airDrag;
            
            // Smoothly interpolate the drag value
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, targetDrag, Time.deltaTime * 10f);
        }
        
        //Strafe Tilt
        void StrafeTilt() {
            //Calculate tilt direction
            if (horizontalMovement != 0f) {
                if (horizontalMovement > 0f) {
                    float tiltSpeed = stafeTiltSpeed * Time.deltaTime;
                    playerDependencies.tilt = Mathf.Lerp(playerDependencies.tilt, -strafeTilt, tiltSpeed);
                }
                else if (horizontalMovement < 0f) {
                    float tiltSpeed = stafeTiltSpeed * Time.deltaTime;
                    playerDependencies.tilt = Mathf.Lerp(playerDependencies.tilt, strafeTilt, tiltSpeed);
                }
            }
        }
        
        //Footsteps
        void Footsteps() {
            if (playerDependencies.isGrounded || playerDependencies.isWallRunning) {
                if (!playerDependencies.isVaulting && !playerDependencies.isInspecting && !playerDependencies.isSliding) {
                    //Combine input
                    var inputVector = new Vector2(horizontalMovement, verticalMovement);
                    
                    //Start curve timer
                    if (inputVector.magnitude > 0f) {
                        //Curve timer
                        if (playerDependencies.isGrounded)
                            curveTime += Time.deltaTime * footstepRate * moveAmount;
                        
                        else if (playerDependencies.isWallRunning) curveTime += Time.deltaTime * footstepRate * 2.5f * moveAmount;
                        
                        //Reset time, loop time and play footstep sound
                        if (curveTime >= 1) {
                            curveTime = 0f;
                            
                            //Audio
                            if (playedRandom.Count == footstepSound.Length) playedRandom.Clear();
                            
                            if (playedRandom.Count != footstepSound.Length) {
                                for (int i = 0; i < footstepSound.Length; i++) {
                                    if (!playedRandom.Contains(i)) randomFilter.Add(i);
                                }
                                
                                randomNum = Random.Range(randomFilter[0], randomFilter.Count);
                                playedRandom.Add(randomNum);
                                audioSource.PlayOneShot(footstepSound[randomNum]);
                                randomFilter.Clear();
                            }
                        }
                    }
                }
                
                //Set camera height (Bobbing) to animation curve value
                cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, footstepCurve.Evaluate(curveTime) * footstepMultiplier, cam.transform.localPosition.z);
            }
        }
        
        //Check for slopes
        bool OnSlope() {
            if (Physics.Raycast(rb.transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f)) {
                if (slopeHit.normal != Vector3.up) return true;
                return false;
            }
            return false;
        }
    }
}