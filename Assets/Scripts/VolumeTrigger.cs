#region

using UnityEngine;
using UnityEngine.Events;

#endregion

public class VolumeTrigger : MonoBehaviour
{
    public UnityEvent onEnter;
    public string tagMask = "Player";
    
    void OnTriggerEnter(Collider _other) {
        if (_other.CompareTag(tagMask)) {
            onEnter?.Invoke();
        }
    }
}