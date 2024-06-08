#region

using System;
using UnityEngine;

#endregion

public class CheckpointVolume : MonoBehaviour
{
    public Action<Transform> onEnterVolume;
    
    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        
        onEnterVolume?.Invoke(transform);
        gameObject.SetActive(false);
    }
}