#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Flashlight : MonoBehaviour
    {
        //Input properties
        [Header("Input Properties")]
        [SerializeField] KeyCode flashlightKey = KeyCode.F;
        
        //Lights
        [Header("Lights")]
        [SerializeField] Light spotLight;
        [SerializeField] Light ambientLight;
        
        //Spot Light Properties
        [Header("Spot Light Properties")]
        [SerializeField] float spotRange = 10;
        [SerializeField] float spotAngle = 60;
        [SerializeField] float spotIntensity = 3;
        [SerializeField] Color spotLightColor = Color.white;
        [SerializeField] Texture2D spotCookie;
        
        //Ambient Light Properties
        [Header("Ambient Light Properties")]
        [SerializeField] float ambientRange = 4;
        [SerializeField] float ambientIntensity = 0.5f;
        [SerializeField] Color ambientLightColor = Color.white;
        
        //Audio Properties
        [Header("Audio Properties")]
        [SerializeField] AudioClip switchOnSound;
        [SerializeField] AudioClip switchOffSound;
        
        AudioSource audioSource;
        
        //Helpers
        bool flashLightOn;
        [Header("PlayerDependencies")]
        PlayerDependencies playerDependencies;
        
        //-----------------------
        
        void Awake() {
            playerDependencies = GetComponent<PlayerDependencies>();
        }
        
        void Start() {
            Setup(); //- 72
        }
        
        void Update() {
            LightControl(); //- 90
        }
        
        //-----------------------
        
        void Setup() {
            //Setup playerDependencies
            audioSource = playerDependencies.audioSourceTop;
            
            //Setup spot light
            spotLight.range = spotRange;
            spotLight.spotAngle = spotAngle;
            spotLight.intensity = spotIntensity;
            spotLight.color = spotLightColor;
            spotLight.cookie = spotCookie;
            
            //Setup ambient light
            ambientLight.range = ambientRange;
            ambientLight.intensity = ambientIntensity;
            ambientLight.color = ambientLightColor;
        }
        
        void LightControl() {
            if (Input.GetKeyDown(flashlightKey)) {
                //Disable flashlight
                if (flashLightOn) {
                    spotLight.enabled = false;
                    ambientLight.enabled = false;
                    
                    //Audio
                    audioSource.PlayOneShot(switchOffSound);
                    
                    flashLightOn = false;
                }
                
                //Enable flashlight
                else {
                    spotLight.enabled = true;
                    ambientLight.enabled = true;
                    
                    //Audio
                    audioSource.PlayOneShot(switchOnSound);
                    
                    flashLightOn = true;
                }
            }
        }
    }
}