//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using PrototypeFPC;

namespace PrototypeFPC
{
    public class Movement : MonoBehaviour
    {
        //Dependencies
        [Header("Dependencies")]
        [SerializeField] Dependencies dependencies;

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

        //Helpers
        float moveAmount;
        float horizontalMovement;
        float verticalMovement;
        float playerHeight = 2f;
        float curveTime = 0f;
        int randomNum = 0;

        Camera cam;
        Rigidbody rb;
        Transform orientation;
        CapsuleCollider cc;

        Vector3 moveDirection;
        Vector3 slopeMoveDirection;
        
        RaycastHit slopeHit;
        AudioSource audioSource;
        [HideInInspector]
        [SerializeField] 
        List<int> 
        playedRandom, 
        randomFilter;


        //-----------------------


        //Functions
        ///////////////

        void Start()
        {
            Setup(); //- Line 113
        }

        void Update()
        {
            GroundCheck(); //- Line 132
            CalculatDirection(); //- Line 139
            CalculateSlope(); //- Line 151
            ControlSpeed(); //- Line 181
            ControlDrag(); //- Line 198
            StrafeTilt(); //- Line 215
            Footsteps(); //- Line 235
        }

        void FixedUpdate()
        {
            Move(); //- Line 158
        }


        //---------------------------


       void Setup()
        {
            //Set player on the ignore raycast layer
            transform.gameObject.layer = 2;

            //Setup dependencies
            rb = dependencies.rb;
            cc = dependencies.cc;
            cam = dependencies.cam;
            orientation = dependencies.orientation;
            audioSource = dependencies.audioSourceBottom;

            //Set rigidbody properties
            rb.freezeRotation = true;
            rb.mass = 50;
        }


        //Check if grounded
        void GroundCheck()
        {
            dependencies.isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius);
        }


        //Calculate input direction
        void CalculatDirection()
        {
            //Get keyboard input axis
            horizontalMovement = Input.GetAxisRaw("Horizontal");
            verticalMovement = Input.GetAxisRaw("Vertical");

            //Set calculated direction
            moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        }


        //Calculate slope
        void CalculateSlope()
        {
            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        }


        //Apply player movement
        void Move()
        {
            //Grounded & NOT on slope
            if(dependencies.isGrounded && !dependencies.isInspecting && !OnSlope() && !dependencies.isSliding)
            {
                rb.AddForce(moveDirection.normalized * moveAmount * multiplier, ForceMode.Acceleration);
            }

            //Grounded & on slope
            if(dependencies.isGrounded && OnSlope())
            {
                rb.AddForce(slopeMoveDirection.normalized * moveAmount * multiplier, ForceMode.Acceleration);
            }

            //Not grounded & in the air
            if(!dependencies.isGrounded)
            {
                rb.AddForce(moveDirection.normalized * moveAmount * multiplier * airMultiplier, ForceMode.Acceleration);
            }
        }


        //Speed control
        void ControlSpeed()
        {
            //Sprinting
            if(Input.GetKey(sprintKey) && dependencies.isGrounded)
            {
                moveAmount = Mathf.Lerp(moveAmount, sprintSpeed, acceleration * Time.deltaTime);
            }

            //Walking
            else
            {
                moveAmount = Mathf.Lerp(moveAmount, walkSpeed, acceleration * Time.deltaTime);
            }
        }


        //Add drag to movement
        void ControlDrag()
        {
            //Ground movement drag
            if(dependencies.isGrounded && !dependencies.isSliding)
            {
                rb.linearDamping = groundDrag;
            }

            //Air movement drag
            else
            {
                rb.linearDamping = airDrag;
            }
        }


        //Strafe Tilt
        void StrafeTilt()
        {
            //Calculate tilt direction
            if(horizontalMovement != 0f)
            {
                if(horizontalMovement > 0f)
                {
                    var tiltSpeed = stafeTiltSpeed * Time.deltaTime;
                    dependencies.tilt = Mathf.Lerp(dependencies.tilt, -strafeTilt, tiltSpeed);
                }
                else if(horizontalMovement < 0f)
                {
                    var tiltSpeed = stafeTiltSpeed * Time.deltaTime;
                    dependencies.tilt = Mathf.Lerp(dependencies.tilt, strafeTilt, tiltSpeed);
                }
            }
        }


        //Footsteps
        void Footsteps()
        {
            if(dependencies.isGrounded || dependencies.isWallRunning)
            {
                if(!dependencies.isVaulting && !dependencies.isInspecting && !dependencies.isSliding)
                {
                    //Combine input
                    Vector2 inputVector = new Vector2(horizontalMovement, verticalMovement);

                    //Start curve timer
                    if(inputVector.magnitude > 0f)
                    {
                        //Curve timer
                        if(dependencies.isGrounded)
                        {
                            curveTime += Time.deltaTime * footstepRate * moveAmount;
                        }

                        else if(dependencies.isWallRunning)
                        {
                            curveTime += Time.deltaTime * footstepRate * 2.5f * moveAmount;
                        }

                        //Reset time, loop time and play footstep sound
                        if(curveTime >= 1)
                        {
                            curveTime = 0f;

                            //Audio
                            if(playedRandom.Count == footstepSound.Length)
                            {
                                playedRandom.Clear();
                            }

                            if(playedRandom.Count != footstepSound.Length)
                            {
                                for(int i = 0; i < footstepSound.Length; i++) 
                                {
                                    if(!playedRandom.Contains(i))
                                    {
                                        randomFilter.Add(i);
                                    }
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
        bool OnSlope()
        {
            if(Physics.Raycast(rb.transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
            {
                if(slopeHit.normal != Vector3.up)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
