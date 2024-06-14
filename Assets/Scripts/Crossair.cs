#region

using PrototypeFPC;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class Crossair : MonoBehaviour
{
    public GrapplingHook hook;
    public Inspect inspect;
    public Image crossair;
    
    void Update() {
        //If the raycast distance from the playMovement to the object is less than the grapple distance, change the crossair to the grapple sprite and the layer is default
        if (Physics.Raycast(hook.playerDependencies.cam.transform.position, hook.playerDependencies.cam.transform.forward, out var _hit, hook.hookDistance, hook.grappleLayerMask)) {
            if (_hit.transform.gameObject.layer == LayerMask.NameToLayer("Default") || _hit.transform.gameObject.layer == LayerMask.NameToLayer("Static")) {
                ResetCrossair();
                return;
            }
            crossair.color = Color.red;
        }
        
        //If the raycast distance from the playMovement to the object is less than the interactable distance, change the crossair to the interactable sprite
        else if (Physics.Raycast(inspect.playerDependencies.cam.transform.position, inspect.playerDependencies.cam.transform.forward, out var _, inspect.maxPickupDistance, inspect.inspectLayerMask))
            crossair.color = Color.cyan;
        
        //If the raycast distance from the playMovement to the object is greater than the grapple distance and the interactable distance, change the crossair to the normal sprite
        else
            ResetCrossair();
    }
    
    void ResetCrossair() {
        crossair.color = Color.white;
    }
}