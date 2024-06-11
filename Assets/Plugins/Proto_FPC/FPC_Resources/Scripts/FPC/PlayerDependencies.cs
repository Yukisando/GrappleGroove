#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class PlayerDependencies : MonoBehaviour
    {
        public float worldGravity = -20f;
        public int physicsIterations = 12;
        public float timeStep = 0.003f;

        public Transform playerTransform;
        public Rigidbody rb;
        public CapsuleCollider cc;
        public Camera cam;
        public Transform orientation;
        public Transform vaultPoint;

        public Transform spawnPointRight;
        public Transform spawnPointLeft;
        public Transform inspectPoint;
        public Transform grabPoint;
        public Transform swayPivotRight;
        public Transform swayPivotLeft;
        public AudioSource audioSourceTop;
        public AudioSource audioSourceBottom;

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