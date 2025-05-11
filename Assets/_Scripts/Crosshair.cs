#region

using PrototypeFPC;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class Crosshair : MonoBehaviour
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
        Setup();
    }

    void Setup() {
        // Initialize components
        playerDependencies = FindAnyObjectByType<PlayerDependencies>();
        if (playerDependencies != null) {
            inspectComponent = playerDependencies.GetComponent<Inspect>();
            grabThrowComponent = playerDependencies.GetComponent<GrabThrow>();
            grapplingHookComponent = playerDependencies.GetComponent<GrapplingHook>();
            playerCamera = playerDependencies.cam;
        }

        // Set initial crosshair
        currentCrosshair = normal;

        // Setup raycast mask
        raycastMask = ~LayerMask.GetMask("Player");
    }

    void LateUpdate() {
        // Default to normal crosshair
        var targetCrosshair = normal;

        // Cast ray from camera
        var ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        bool hitSomething = Physics.Raycast(ray, out var hit, math.INFINITY, raycastMask, QueryTriggerInteraction.Ignore);

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
        if (inspectable && inspectComponent != null && inspectComponent.maxPickupDistance >= hit.distance) {
            targetCrosshair = inspect;
            return;
        }

        // Check for grabbable objects
        var grabbable = hit.collider.GetComponent<Grabbable>();
        if (grabbable && grabThrowComponent != null && grabThrowComponent.maxGrabDistance >= hit.distance) {
            targetCrosshair = grab;
            return;
        }

        // Check for reset buttons
        var resetButton = hit.collider.GetComponent<ResetButton>();
        if (resetButton && resetButton.interactionDistance >= hit.distance) {
            targetCrosshair = grab; // Use grab icon for reset buttons
            return;
        }

        // Check for hookable objects
        var hookable = hit.collider.GetComponent<Hookable>();
        if (hookable && grapplingHookComponent.hookDistance >= hit.distance) {
            targetCrosshair = grapple;
            return;
        }

        // Check for plank objects
        var plank = hit.collider.GetComponent<Plank>();
        if (plank) targetCrosshair = snipsnip;
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