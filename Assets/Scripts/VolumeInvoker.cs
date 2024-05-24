#region

using UnityEngine;
using UnityEngine.Events;

#endregion

public class VolumeInvoker : MonoBehaviour
{
    public UnityEvent onEnter;
    public string tagMask = "Player";
    
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(tagMask)) {
            onEnter?.Invoke();
        }
    }
}