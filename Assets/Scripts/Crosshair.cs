#region

using PrototypeFPC;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class Crosshair : NetworkBehaviour
{
    [Header("Crosshair Images")]
    [SerializeField] Image normal;
    [SerializeField] Image grab;
    [SerializeField] Image grabbing;
    [SerializeField] Image inspect;
    [SerializeField] Image inspecting;
    [SerializeField] Image grapple;
    [SerializeField] Image snipsnip;

    [Header("Debug")]
    [SerializeField] bool showDebugLogs = true;

    // Cached components
    Inspect inspectComponent;
    GrabThrow grabThrowComponent;
    GrapplingHook grapplingHookComponent;
    Camera playerCamera;
    PlayerDependencies playerDependencies;

    // Current active crosshair
    Image currentCrosshair;

    // Layer mask for raycasting
    int raycastMask;

    void Awake() {
        NetworkManager.Singleton.OnClientConnectedCallback += _ => {
            if (IsOwner) Setup();
        };
    }

    void Setup() {
        // Initialize dependencies
        InitializeDependencies();

        // Set up raycast mask (everything except Ignore Raycast and Player layers)
        raycastMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Player"));

        // Hide system cursor when game starts
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Set default crosshair
        SetCrosshair(normal);
    }

    void InitializeDependencies() {
        // Cache camera reference
        playerDependencies = FindAnyObjectByType<PlayerDependencies>();

        playerCamera = playerDependencies.cam;

        // Cache component references
        inspectComponent = playerDependencies.GetComponent<Inspect>();
        grabThrowComponent = playerDependencies.GetComponent<GrabThrow>();
        grapplingHookComponent = playerDependencies.GetComponent<GrapplingHook>();
    }

    void LateUpdate() {
        if (!IsOwner) return;
        if (playerDependencies == null || playerCamera == null) return;

        // Default to normal crosshair
        var targetCrosshair = normal;

        // Cast ray from camera
        var ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        bool hitSomething = Physics.Raycast(ray, out hit, math.INFINITY, raycastMask, QueryTriggerInteraction.Ignore);

        if (hitSomething)

            // Check for different types of interactive objects
            CheckForInteractiveObjects(hit, ref targetCrosshair);

        // Override with action states
        if (playerDependencies.isGrabbing) targetCrosshair = grabbing;

        if (playerDependencies.isInspecting) targetCrosshair = inspecting;

        // Apply the crosshair change
        SetCrosshair(targetCrosshair);

        // Update cursor visibility
        UpdateCursorVisibility();
    }

    void CheckForInteractiveObjects(RaycastHit hit, ref Image targetCrosshair) {
        // Check for inspectable objects
        var inspectable = hit.collider.GetComponent<Inspectable>();
        if (inspectable != null && inspectComponent != null && inspectComponent.maxPickupDistance >= hit.distance) {
            targetCrosshair = inspect;
            return;
        }

        // Check for grabbable objects
        var grabbable = hit.collider.GetComponent<Grabbable>();
        if (grabbable != null && grabThrowComponent != null && grabThrowComponent.maxGrabDistance >= hit.distance) {
            targetCrosshair = grab;
            return;
        }

        // Check for hookable objects
        var hookable = hit.collider.GetComponent<Hookable>();
        if (hookable != null && grapplingHookComponent != null && grapplingHookComponent.hookDistance >= hit.distance) {
            targetCrosshair = grapple;
            return;
        }

        // Check for rope objects (this can override previous crosshairs)
        if (hit.collider.CompareTag("Rope")) targetCrosshair = snipsnip;
    }

    void SetCrosshair(Image newCrosshair) {
        // Skip if it's the same crosshair
        if (currentCrosshair == newCrosshair) return;

        // Disable all crosshairs
        normal.enabled = false;
        grab.enabled = false;
        grabbing.enabled = false;
        inspect.enabled = false;
        inspecting.enabled = false;
        grapple.enabled = false;
        snipsnip.enabled = false;

        // Enable only the new one
        if (newCrosshair != null) newCrosshair.enabled = true;

        // Update current crosshair reference
        currentCrosshair = newCrosshair;
    }

    void UpdateCursorVisibility() {
        bool shouldShowCursor = Time.timeScale == 0;

        if (Cursor.visible != shouldShowCursor) {
            Cursor.visible = shouldShowCursor;
            Cursor.lockState = shouldShowCursor ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}