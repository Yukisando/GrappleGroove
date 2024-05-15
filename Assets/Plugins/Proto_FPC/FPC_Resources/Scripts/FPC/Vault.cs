//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using UnityEngine;
using PrototypeFPC;

namespace PrototypeFPC
{
    public class Vault : MonoBehaviour
    {
        //Dependencies
        [Header("Dependencies")]
        [SerializeField] Dependencies dependencies;

        //Input
        [Header("Input Properties")]
        [SerializeField] KeyCode vaultKey = KeyCode.LeftShift;

        //Vault Properties
        [Header("Vault Properties")]
        [SerializeField] float speed = 10f;
        [SerializeField] float duration = 1f;
        [SerializeField] float vaultTilt = 10;
        [SerializeField] float vaultRayLength = 1.1f;

        //Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip vaultSound;

        //Helpers
        float vaultTimer;

        Vector3 lastPos, lastVel;
        Vector3 vaultRayPos;
        
        Transform vaultPoint;
        Rigidbody rb;
        CapsuleCollider cc;
        AudioSource audioSource;
        RaycastHit hit;


        //-----------------------


        //Functions
        ///////////////

        void Start()
        {
            Setup(); //- Line 66
        }

        void FixedUpdate()
        {
            Vaulting(); //- Line 76
        }


        //-----------------------


        void Setup()
        {
            //Setup dependencies
            rb = dependencies.rb;
            vaultPoint = dependencies.vaultPoint;
            cc = dependencies.cc;
            audioSource = dependencies.audioSourceBottom;
        }


        void Vaulting()
        {
             if(rb.linearVelocity.magnitude > 1 && Input.GetKey(vaultKey) && !dependencies.isVaulting && !dependencies.isWallRunning && !dependencies.isSliding)
            {
                //Raycast check vault
                if(Physics.Raycast(dependencies.vaultPoint.position, -dependencies.vaultPoint.up, out hit, vaultRayLength, ~(1 << LayerMask.NameToLayer("Ignore Raycast")), QueryTriggerInteraction.Ignore))
                {
                    //Check if obstacle is static
                    if(!hit.collider.gameObject.GetComponent<Rigidbody>() || hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic)
                    {
                        //If obstacle is flat
                        if(hit.normal == Vector3.up)
                        {
                            //set player, vault position and velocity before vaulting
                            vaultRayPos = hit.point;
                            lastPos = rb.transform.position;
                            lastVel = rb.linearVelocity;

                            rb.useGravity = false;
                            cc.enabled = false;

                            dependencies.isVaulting = true;

                            //Audio
                            audioSource.PlayOneShot(vaultSound);
                        }
                    }
                }
            }

            else if(dependencies.isVaulting)
            {
                //Start vault timer
                vaultTimer += speed * Time.fixedDeltaTime;

                //Move player to vault position
                rb.MovePosition(Vector3.Lerp(lastPos, vaultRayPos + new Vector3(0, cc.height / 2, 0), vaultTimer));
                dependencies.tilt = Mathf.Lerp(dependencies.tilt, vaultTilt, vaultTimer);

                //Vault timer
                if(vaultTimer >= duration)
                {
                    //Apply last velocity after vaulting
                    rb.linearVelocity = lastVel += lastVel * 0.2f;

                    rb.useGravity = true;
                    cc.enabled = true;

                    vaultTimer = 0;

                    dependencies.isVaulting = false;
                }
            }
        }
    }
}
