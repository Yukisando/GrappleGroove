#region

using PrototypeFPC;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

#endregion

class Crosshair : MonoBehaviour
{
    [SerializeField] PlayerDependencies playerDependencies;
    [SerializeField] Image normal;
    [SerializeField] Image grab;
    [SerializeField] Image grabbing;
    [SerializeField] Image inspect;
    [SerializeField] Image inspecting;
    [SerializeField] Image grapple;
    [SerializeField] Image snipsnip;

    Sprite defaultSprite;

    void LateUpdate() {
        playerDependencies = FindAnyObjectByType<PlayerDependencies>();
        if (playerDependencies == null) return;
        SetCrosshair(normal);
        if (Physics.Raycast(playerDependencies.cam.transform.position, playerDependencies.cam.transform.forward, out var hit, math.INFINITY, ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore)) {
            if (hit.collider.GetComponent<Inspectable>() != null && playerDependencies.GetComponent<Inspect>().maxPickupDistance >= hit.distance)
                SetCrosshair(inspect);
            else if (hit.collider.GetComponent<Grabbable>() != null && playerDependencies.GetComponent<GrabThrow>().maxGrabDistance >= hit.distance)
                SetCrosshair(grab);
            else if (hit.collider.GetComponent<Hookable>() != null && playerDependencies.GetComponent<GrapplingHook>().hookDistance >= hit.distance) SetCrosshair(grapple);

            if (hit.collider.CompareTag("Rope")) SetCrosshair(snipsnip);
        }

        if (playerDependencies.isGrabbing) SetCrosshair(grabbing);
        if (playerDependencies.isInspecting) SetCrosshair(inspecting);
    }

    void SetCrosshair(Image activeCrosshair) {
        normal.enabled = activeCrosshair == normal;

        grab.enabled = activeCrosshair == grab;
        grabbing.enabled = activeCrosshair == grabbing;
        inspect.enabled = activeCrosshair == inspect;
        inspecting.enabled = activeCrosshair == inspecting;
        grapple.enabled = activeCrosshair == grapple;
        snipsnip.enabled = activeCrosshair == snipsnip;
    }
}