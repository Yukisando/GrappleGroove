#region

using UnityEngine;

#endregion

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

        [SerializeField] public Transform spawnPointRight;
        [SerializeField] public Transform spawnPointLeft;
        [SerializeField] public Transform inspectPoint;
        [SerializeField] public Transform grabPoint;
        public Transform swayPivotRight;
        public Transform swayPivotLeft;
        [SerializeField] public AudioSource audioSourceTop;
        [SerializeField] public AudioSource audioSourceBottom;

        public bool isGrounded { get; set; }
        public bool isSliding { get; set; }
        public bool isWallRunning { get; set; }
        public bool isInspecting { get; set; }
        public bool isGrabbing { get; set; }
        public bool isVaulting { get; set; }
        public float tilt { get; set; }

        //----------
        void Awake() {
            //Physics & Timestep setup
            Physics.gravity = new Vector3(0, worldGravity, 0);
            Physics.defaultSolverIterations = physicsIterations;
            Time.fixedDeltaTime = timeStep;
        }
    }
}