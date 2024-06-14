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
    [SerializeField] Image inspect;
    [SerializeField] Image grapple;
    
    Sprite defaultSprite;
    
    void Update() {
        if (Physics.Raycast(playerDependencies.cam.transform.position, playerDependencies.cam.transform.forward, out var hit, math.INFINITY)) {
            if (hit.collider.CompareTag("Grab") && playerDependencies.GetComponent<GrabThrow>().maxGrabDistance >= hit.distance) {
                SetCrosshair(grab);
            }
            else if (hit.collider.CompareTag("Inspect") && playerDependencies.GetComponent<Inspect>().maxPickupDistance >= hit.distance) {
                SetCrosshair(inspect);
            }
            else if (hit.collider.CompareTag("Grapple") && playerDependencies.GetComponent<GrapplingHook>().hookDistance >= hit.distance) {
                SetCrosshair(grapple);
            }
            else {
                SetCrosshair(normal);
            }
        }
        else {
            SetCrosshair(normal);
        }
    }
    
    void SetCrosshair(Image activeCrosshair = null) {
        normal.enabled = activeCrosshair == normal;
        grab.enabled = activeCrosshair == grab;
        inspect.enabled = activeCrosshair == inspect;
        grapple.enabled = activeCrosshair == grapple;
        normal.enabled = activeCrosshair == null || activeCrosshair == normal;
    }
}