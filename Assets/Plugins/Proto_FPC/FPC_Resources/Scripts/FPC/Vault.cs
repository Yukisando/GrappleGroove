#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Vault : MonoBehaviour
    {
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
        AudioSource audioSource;
        CapsuleCollider cc;
        RaycastHit hit;
        
        Vector3 lastPos, lastVel;
        [Header("PlayerDependencies")]
        PlayerDependencies playerDependencies;
        Rigidbody rb;
        
        Transform vaultPoint;
        Vector3 vaultRayPos;
        
        //Helpers
        float vaultTimer;
        
        //-----------------------
        
        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }
        
        void Start() {
            Setup(); //- Line 66
        }
        
        void FixedUpdate() {
            Vaulting(); //- Line 76
        }
        
        //-----------------------
        
        void Setup() {
            //Setup playerDependencies
            rb = playerDependencies.rb;
            vaultPoint = playerDependencies.vaultPoint;
            cc = playerDependencies.cc;
            audioSource = playerDependencies.audioSourceBottom;
        }
        
        void Vaulting() {
            if (rb.linearVelocity.magnitude > 1 && Input.GetKey(vaultKey) && !playerDependencies.isVaulting && !playerDependencies.isWallRunning && !playerDependencies.isSliding) {
                //Raycast check vault
                if (Physics.Raycast(playerDependencies.vaultPoint.position, -playerDependencies.vaultPoint.up, out hit, vaultRayLength, ~(1 << LayerMask.NameToLayer("Ignore Raycast")), QueryTriggerInteraction.Ignore))
                    
                    //Check if obstacle is static
                    if (!hit.collider.gameObject.GetComponent<Rigidbody>() || hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic)
                        
                        //If obstacle is flat
                        if (hit.normal == Vector3.up) {
                            //set player, vault position and velocity before vaulting
                            vaultRayPos = hit.point;
                            lastPos = rb.transform.position;
                            lastVel = rb.linearVelocity;
                            
                            rb.useGravity = false;
                            cc.enabled = false;
                            
                            playerDependencies.isVaulting = true;
                            
                            //Audio
                            audioSource.PlayOneShot(vaultSound);
                        }
            }
            
            else if (playerDependencies.isVaulting) {
                //Start vault timer
                vaultTimer += speed * Time.fixedDeltaTime;
                
                //Move player to vault position
                rb.MovePosition(Vector3.Lerp(lastPos, vaultRayPos + new Vector3(0, cc.height / 2, 0), vaultTimer));
                playerDependencies.tilt = Mathf.Lerp(playerDependencies.tilt, vaultTilt, vaultTimer);
                
                //Vault timer
                if (vaultTimer >= duration) {
                    //Apply last velocity after vaulting
                    rb.linearVelocity = lastVel += lastVel * 0.2f;
                    
                    rb.useGravity = true;
                    cc.enabled = true;
                    
                    vaultTimer = 0;
                    
                    playerDependencies.isVaulting = false;
                }
            }
        }
    }
}