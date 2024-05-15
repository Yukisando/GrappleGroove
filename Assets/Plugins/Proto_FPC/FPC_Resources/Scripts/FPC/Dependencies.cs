//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using UnityEngine;

namespace PrototypeFPC
{
    public class Dependencies : MonoBehaviour
    {
        //Dependencies for Prototype FPC scripts
        [SerializeField] public float worldGravity = -20f;
        [SerializeField] public int physicsIterations = 12;
        [SerializeField] public float timeStep = 0.003f;
        
        [SerializeField] public Rigidbody rb;
        [SerializeField] public CapsuleCollider cc;
        [SerializeField] public Camera cam;
        [SerializeField] public Transform orientation;
        [SerializeField] public Transform vaultPoint;
        [SerializeField] public Transform spawnPoint;
        [SerializeField] public Transform inspectPoint;
        [SerializeField] public Transform grabPoint;
        [SerializeField] public Transform swayPivot;
        [SerializeField] public AudioSource audioSourceTop;
        [SerializeField] public AudioSource audioSourceBottom;

        [SerializeField] public bool isGrounded { get; set; }
        [SerializeField] public bool isSliding { get; set; }
        [SerializeField] public bool isWallRunning { get; set; }
        [SerializeField] public bool isInspecting { get; set; }
        [SerializeField] public bool isGrabbing { get; set; }
        [SerializeField] public bool isVaulting { get; set; }
        [SerializeField] public float tilt { get; set; }

        //----------
        void Awake()
        {
            //Physics & Timestep setup
            Physics.gravity = new Vector3(0, worldGravity, 0);
            Physics.defaultSolverIterations = physicsIterations;
            Time.fixedDeltaTime = timeStep;
        }
    }
}
