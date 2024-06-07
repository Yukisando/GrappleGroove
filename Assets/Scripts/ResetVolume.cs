using System;
using UnityEngine;

public class ResetVolume : MonoBehaviour
{
    public Action onEnterVolume;
    
    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        
        onEnterVolume?.Invoke();
    }
}
