#region

using PrototypeFPC;
using UnityEngine;

#endregion

public class PlayerDependencies : MonoBehaviour
{
    public float worldGravity = -20f;
    public int physicsIterations = 12;
    public float timeStep = 0.003f;

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
    public AudioSource audioSourceWind;

    public bool isGrounded;
    public bool isSliding;
    public bool isWallRunning;
    public bool isInspecting;
    public bool isGrabbing;
    public bool isVaulting;
    [HideInInspector] public float tilt;

    public Perspective perspective;
    public GrapplingHook grapplingHook;

    //----------
    void Awake() {
        //Physics & Timestep setup
        Physics.gravity = new Vector3(0, worldGravity, 0);
        Physics.defaultSolverIterations = physicsIterations;
        Time.fixedDeltaTime = timeStep;
    }
}