#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Slide : MonoBehaviour
    {
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
        AudioSource audioSource;
        CapsuleCollider cc;
        bool hardLanding;
        bool headNotSafe;
        RaycastHit headNotSafeHit;
        Transform orientation;
        
        //Helpers
        float originalHeight;
        [Header("PlayerDependencies")]
        PlayerDependencies playerDependencies;
        
        Rigidbody rb;
        
        bool slid;
        
        //-----------------------
        
        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }
        
        void Start() {
            Setup(); //- Line 76
        }
        
        void Update() {
            HeadSafeCheck(); //- Line 89
            InitiateSlide(); //- Line 96
            SlideDrag(); //- Line 189
        }
        
        void FixedUpdate() {
            Sliding(); //- Line 161
        }
        
        //-----------------------
        
        void Setup() {
            //Setup playerDependencies
            rb = playerDependencies.rb;
            cc = playerDependencies.cc;
            orientation = playerDependencies.orientation;
            audioSource = playerDependencies.audioSourceBottom;
            
            //Record original height
            originalHeight = cc.height;
        }
        
        void HeadSafeCheck() {
            //Check if safe to stand up
            headNotSafe = Physics.Raycast(rb.transform.position, orientation.up, out headNotSafeHit, originalHeight);
        }
        
        void InitiateSlide() {
            //Toggle hard land slide
            if (rb.linearVelocity.y < -35 && rb.linearVelocity.z > 5 && !playerDependencies.isGrounded) hardLanding = true;
            
            if (rb.linearVelocity.magnitude < 5 && playerDependencies.isGrounded) hardLanding = false;
            
            if (playerDependencies.isGrounded) {
                //Slide when landed hard
                if (hardLanding && !playerDependencies.isSliding) {
                    //Set collider height
                    cc.height = slideHeight;
                    
                    playerDependencies.isSliding = true;
                    
                    //Audio
                    audioSource.PlayOneShot(slideSound);
                }
                
                //Slide
                if (Input.GetKey(slideKey) && rb.linearVelocity.magnitude > 5 && !playerDependencies.isSliding) {
                    //Set collider height
                    cc.height = slideHeight;
                    
                    playerDependencies.isSliding = true;
                    
                    //Audio
                    audioSource.PlayOneShot(slideSound);
                }
                
                //Unslide
                else if (!Input.GetKey(slideKey) && playerDependencies.isSliding && !headNotSafe && !hardLanding) {
                    //Reset collider height
                    cc.height = originalHeight;
                    
                    playerDependencies.isSliding = false;
                    slid = false;
                    
                    //Audio
                    audioSource.PlayOneShot(getUpSound);
                }
            }
            
            //Unslide if in air
            else if (!playerDependencies.isGrounded && playerDependencies.isSliding) {
                //Reset collider height
                cc.height = originalHeight;
                
                playerDependencies.isSliding = false;
                slid = false;
            }
        }
        
        void Sliding() {
            //Add slide force
            if (playerDependencies.isGrounded && playerDependencies.isSliding && !slid) {
                slid = true;
                rb.AddForce(orientation.forward * rb.linearVelocity.magnitude * amount, ForceMode.Impulse);
            }
            
            else if (headNotSafe && playerDependencies.isSliding) {
                if (cc.height != slideHeight) cc.height = slideHeight;
                
                rb.AddForce(orientation.forward * amount * 250, ForceMode.Force);
            }
            
            //Slide tilt
            if (cc.height == slideHeight) {
                float tiltSpeed = slideTiltSpeed * Time.deltaTime;
                playerDependencies.tilt = Mathf.Lerp(playerDependencies.tilt, slideTilt, tiltSpeed);
            }
        }
        
        void SlideDrag() {
            //Slide drag
            if (playerDependencies.isGrounded && playerDependencies.isSliding) rb.linearDamping = drag;
        }
    }
}