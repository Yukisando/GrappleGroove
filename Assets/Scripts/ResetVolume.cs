using System;
using UnityEngine;

public class ResetVolume : MonoBehaviour
{
    public Action<GameObject> onEnterVolume;
    
    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        
        onEnterVolume?.Invoke(_other.gameObject);
    }
}
