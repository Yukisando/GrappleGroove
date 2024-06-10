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
        [SerializeField] float size = 0.1f;
        [SerializeField] float force = 500f;
        [Range(0.1f, 0.5f)]
        [SerializeField] float projectRate = 0.2f;

        //Audio properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip projectingSound;
        [SerializeField] AudioClip scrollSound;
        [SerializeField] AudioClip minMaxSound;

        //Helpers
        bool allowProjectile = true;
        AudioSource audioSource;
        bool fireProjectile;
        [Header("PlayerDependencies")]
        PlayerDependencies playerDependencies;
        Rigidbody spawnedProjectile;

        Transform spawnPoint;

        //--------------------------

        //Functions
        ///////////////

        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }

        void Start() {
            Setup(); //- Line68
        }

        void Update() {
            if (!Application.isFocused) return;

            ControlRate(); //- Line 77
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

        //Control projectile rate
        void ControlRate() {
            //Increase and decrease projecting rate with scroll wheel
            if (Mathf.Clamp(projectRate, 0.1f, 0.5f) == projectRate && Input.mouseScrollDelta.y != 0 && !playerDependencies.isInspecting) {
                //Set rate
                projectRate += Input.mouseScrollDelta.y * 0.01f;

                if (projectRate > 0.1f && projectRate < 0.5f)

                    //Audio
                    audioSource.PlayOneShot(scrollSound);
            }

            //Clamp minimum rate
            else if (projectRate < 0.1f) {
                projectRate = 0.1f;

                //Audio
                audioSource.PlayOneShot(minMaxSound);
            }

            //Clamp maximum rate
            else if (projectRate > 0.5f) {
                projectRate = 0.5f;

                //Audio
                audioSource.PlayOneShot(minMaxSound);
            }
        }

        //Create projectile
        void CreateProjectile() {
            //Initiate projectile
            if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl) && allowProjectile && !playerDependencies.isInspecting) StartCoroutine(ProjectAtRate());

            //Project at specified rate
            IEnumerator ProjectAtRate() {
                allowProjectile = false;

                //Instantiate and add force to the projectile
                spawnedProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.localRotation).GetComponent<Rigidbody>();
                spawnedProjectile.transform.localScale = new Vector3(size, size, size);
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