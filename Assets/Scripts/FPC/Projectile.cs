#region

using System.Collections;
using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Projectile : MonoBehaviour
    {
        //Projectile properties
        [Header("Projectile Properties")]
        [SerializeField] GameObject projectile;
        [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
        [SerializeField] float size = 0.1f;
        [SerializeField] float force = 500f;
        [Range(0.1f, 0.5f)] [SerializeField] float projectRate = 0.2f;
        
        //Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip projectingSound;
        
        //Helpers
        bool allowProjectile = true;
        AudioSource audioSource;
        bool fireProjectile;
        PlayerDependencies playerDependencies;
        Rigidbody spawnedProjectile;
        
        Transform spawnPoint;
        
        //--------------------------
        
        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }
        
        void Start() {
            Setup(); //- Line68
        }
        
        void Update() {
            if (!Application.isFocused) return;
            
            CreateProjectile(); //- Line 113
        }
        
        void FixedUpdate() {
            if (!Application.isFocused) return;
            ShootProjectile(); //- Line 142
        }
        
        //--------------------------
        
        void Setup() {
            //Setup playerDependencies
            spawnPoint = playerDependencies.spawnPointRight;
            audioSource = playerDependencies.audioSourceTop;
        }
        
        //Create projectile
        void CreateProjectile() {
            //Initiate projectile
            if (Input.GetKey(shootKey) && allowProjectile && !playerDependencies.isInspecting) StartCoroutine(ProjectAtRate());
            
            //Project at specified rate
            IEnumerator ProjectAtRate() {
                allowProjectile = false;
                
                //Instantiate and add force to the projectile
                spawnedProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.localRotation).GetComponent<Rigidbody>();
                spawnedProjectile.transform.LeanScale(new Vector3(size, size, size), .1f);
                fireProjectile = true;
                
                //Audio
                audioSource.PlayOneShot(projectingSound);
                
                //Proceed after projectile rate
                yield return new WaitForSeconds(projectRate);
                allowProjectile = true;
            }
        }
        
        //Add force to projectile
        void ShootProjectile() {
            if (fireProjectile) {
                fireProjectile = false;
                spawnedProjectile.AddForce(playerDependencies.spawnPointRight.transform.forward * force, ForceMode.Impulse);
            }
        }
    }
}