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
    [SerializeField] Image interact;
    [SerializeField] Image inspecting;
    [SerializeField] Image grapple;
    [SerializeField] Image snipsnip;

    Inspect inspectComponent;
    GrabThrow grabThrowComponent;
    GrapplingHook grapplingHookComponent;
    Camera playerCamera;
    PlayerDependencies playerDependencies;
    Image currentCrosshair;

    int raycastMask;

    void Awake() {
        Setup();
    }

    void Setup() {
        playerDependencies = FindAnyObjectByType<PlayerDependencies>();
        if (playerDependencies != null) {
            inspectComponent = playerDependencies.GetComponent<Inspect>();
            grabThrowComponent = playerDependencies.GetComponent<GrabThrow>();
            grapplingHookComponent = playerDependencies.GetComponent<GrapplingHook>();
            playerCamera = playerDependencies.cam;
        }

        currentCrosshair = normal;

        raycastMask = ~LayerMask.GetMask("IgnoreRaycast", "Player", "PlayerHitbox", "Plank");
    }

    void LateUpdate() {
        var targetCrosshair = normal;

        var ray = GetCameraRay();
        bool hitSomething = Physics.Raycast(ray, out var hit, math.INFINITY, raycastMask, QueryTriggerInteraction.Ignore);

        if (hitSomething)
            CheckForInteractiveObjects(hit, ref targetCrosshair);

        if (playerDependencies.isGrabbing) targetCrosshair = grabbing;
        if (playerDependencies.isInspecting) targetCrosshair = inspecting;

        SetCrosshair(targetCrosshair);
        UpdateCursorVisibility();
    }

    Ray GetCameraRay() {
        return new Ray(playerCamera.transform.position, playerCamera.transform.forward);
    }

    void CheckForInteractiveObjects(RaycastHit hit, ref Image targetCrosshair) {
        if (hit.collider.TryGetComponent(out Inspectable inspectable) &&
            inspectComponent != null &&
            inspectComponent.maxPickupDistance >= hit.distance) {
            targetCrosshair = inspect;
            return;
        }

        if (hit.collider.TryGetComponent(out Grabbable grabbable) &&
            grabThrowComponent != null &&
            grabThrowComponent.maxGrabDistance >= hit.distance) {
            targetCrosshair = grab;
            return;
        }

        if (hit.collider.TryGetComponent(out GameButton resetButton) &&
            resetButton.interactionDistance >= hit.distance) {
            targetCrosshair = interact;
            return;
        }

        if (hit.collider.TryGetComponent(out Hookable hookable) &&
            grapplingHookComponent.maxRopeLength >= hit.distance) {
            targetCrosshair = grapple;
            return;
        }

        if (hit.collider.TryGetComponent(out Plank plank)) targetCrosshair = snipsnip;
    }

    void SetCrosshair(Image newCrosshair) {
        if (currentCrosshair == newCrosshair) return;

        normal.enabled = grab.enabled = grabbing.enabled =
            inspect.enabled = inspecting.enabled = grapple.enabled = snipsnip.enabled = interact.enabled = false;

        if (newCrosshair != null) newCrosshair.enabled = true;

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