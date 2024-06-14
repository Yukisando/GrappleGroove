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
    
    Sprite defaultSprite;
    int mask;
    
    void Awake() {
        mask = ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Player"));
    }
    
    void LateUpdate() {
        SetCrosshair(normal);
        if (Physics.Raycast(playerDependencies.cam.transform.position, playerDependencies.cam.transform.forward, out var hit, math.INFINITY, mask, QueryTriggerInteraction.Ignore)) {
            if (hit.collider.CompareTag("Grab") && playerDependencies.GetComponent<GrabThrow>().maxGrabDistance >= hit.distance) {
                SetCrosshair(grab);
            }
            else if (hit.collider.CompareTag("Inspect") && playerDependencies.GetComponent<Inspect>().maxPickupDistance >= hit.distance) {
                SetCrosshair(inspect);
            }
            else if (hit.collider.CompareTag("Grapple") && playerDependencies.GetComponent<GrapplingHook>().hookDistance >= hit.distance) {
                SetCrosshair(grapple);
            }
        }
        
        if (playerDependencies.isGrabbing) SetCrosshair(grabbing);
        if (playerDependencies.isInspecting) SetCrosshair(inspecting);
    }
    
    void SetCrosshair(Image activeCrosshair) {
        normal.enabled = activeCrosshair == normal;
        grab.enabled = activeCrosshair == grab;
        inspect.enabled = activeCrosshair == inspect;
        grapple.enabled = activeCrosshair == grapple;
        normal.enabled = activeCrosshair == normal;
    }
}