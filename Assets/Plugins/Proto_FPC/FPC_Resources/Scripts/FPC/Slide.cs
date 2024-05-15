//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using UnityEngine;
using PrototypeFPC;

namespace PrototypeFPC
{
    public class Slide : MonoBehaviour
    {
        //Dependencies
        [Header("Dependencies")]
        [SerializeField] Dependencies dependencies;

        //Input properties
        [Header("Input Properties")]
        [SerializeField] KeyCode slideKey = KeyCode.C;

        //Slide properties
        [Header("Slide Properties")]
        [SerializeField] float slideHeight = 0.5f;
        [SerializeField] float amount = 25f;
        [SerializeField] float drag = 1f;
        [SerializeField] float slideTilt = 18f;
        [SerializeField] float slideTiltSpeed = 3f;

        //Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip slideSound;
        [SerializeField] AudioClip getUpSound;

        //Helpers
        float originalHeight;

        bool slid = false;
        bool headNotSafe = false;
        bool hardLanding = false;
        
        Rigidbody rb;
        CapsuleCollider cc;
        Transform orientation;
        RaycastHit headNotSafeHit;
        AudioSource audioSource;


        //-----------------------


        //Functions
        ///////////////

        void Start()
        {
            Setup(); //- Line 76
        }

        void Update()
        {
            HeadSafeCheck(); //- Line 89
            InitiateSlide(); //- Line 96
            SlideDrag(); //- Line 189
        }

        void FixedUpdate()
        {
            Sliding(); //- Line 161
        }


        //-----------------------


        void Setup()
        {
            //Setup dependencies
            rb = dependencies.rb;
            cc = dependencies.cc;
            orientation = dependencies.orientation;
            audioSource = dependencies.audioSourceBottom;

            //Record original height
            originalHeight = cc.height;
        }


        void HeadSafeCheck()
        {
            //Check if safe to stand up
            headNotSafe = Physics.Raycast(rb.transform.position, orientation.up, out headNotSafeHit, originalHeight);
        }


        void InitiateSlide()
        {
            //Toggle hard land slide
            if(rb.linearVelocity.y < -35 && rb.linearVelocity.z > 5 && !dependencies.isGrounded)
            {
                hardLanding = true;
            }

            if(rb.linearVelocity.magnitude < 5 && dependencies.isGrounded)
            {
                hardLanding = false;
            }

            if(dependencies.isGrounded)
            {
                //Slide when landed hard
                if(hardLanding && !dependencies.isSliding)
                {
                    //Set collider height
                    cc.height = slideHeight;

                    dependencies.isSliding = true;

                    //Audio
                    audioSource.PlayOneShot(slideSound);
                }

                //Slide
                if(Input.GetKey(slideKey) && rb.linearVelocity.magnitude > 5 && !dependencies.isSliding)
                {
                    //Set collider height
                    cc.height = slideHeight;

                    dependencies.isSliding = true;

                    //Audio
                    audioSource.PlayOneShot(slideSound);
                }

                //Unslide
                else if(!Input.GetKey(slideKey) && dependencies.isSliding && !headNotSafe && !hardLanding)
                {
                    //Reset collider height
                    cc.height = originalHeight;

                    dependencies.isSliding = false;
                    slid = false;

                    //Audio
                    audioSource.PlayOneShot(getUpSound);
                }
            }

            //Unslide if in air
            else if(!dependencies.isGrounded && dependencies.isSliding)
            {
                //Reset collider height
                cc.height = originalHeight;

                dependencies.isSliding = false;
                slid = false;
            }
        }


        void Sliding()
        {
            //Add slide force
            if(dependencies.isGrounded && dependencies.isSliding && !slid)
            {
                slid = true;
                rb.AddForce(orientation.forward * rb.linearVelocity.magnitude * amount, ForceMode.Impulse);
            }

            else if(headNotSafe && dependencies.isSliding)
            {
                if(cc.height != slideHeight)
                {
                    cc.height = slideHeight;
                }
                
                rb.AddForce(orientation.forward * amount * 250, ForceMode.Force);
            }

            //Slide tilt
            if(cc.height == slideHeight)
            {
                var tiltSpeed = slideTiltSpeed * Time.deltaTime;
                dependencies.tilt = Mathf.Lerp(dependencies.tilt, slideTilt, tiltSpeed);
            }
        }


        void SlideDrag()
        {
            //Slide drag
            if(dependencies.isGrounded && dependencies.isSliding)
            {
                rb.linearDamping = drag;
            }
        }
    }
}
