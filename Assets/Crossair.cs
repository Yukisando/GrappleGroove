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
        if (Physics.Raycast(hook.dependencies.cam.transform.position, hook.dependencies.cam.transform.forward, out var hit, hook.hookDistance, 1 << 0)) {
            crossair.color = Color.blue;
        }
        
//If the raycast distance from the playMovement to the object is less than the interactable distance, change the crossair to the interactable sprite
        else if (Physics.Raycast(inspect.dependencies.cam.transform.position, inspect.dependencies.cam.transform.forward, out var hit2, inspect.maxPickupDistance, 1 << 0)) {
            crossair.color = Color.green;
        }
        
//If the raycast distance from the playMovement to the object is greater than the grapple distance and the interactable distance, change the crossair to the normal sprite
        else {
            crossair.color = Color.white;
        }
    }
}