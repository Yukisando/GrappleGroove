#region

using System;
using UnityEngine;

#endregion

public class CheckpointVolume : MonoBehaviour
{
    public Action<Transform> onEnterVolume;
    
    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        
        onEnterVolume?.Invoke(transform);
        gameObject.SetActive(false);
    }
}