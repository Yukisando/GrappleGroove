#region

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class KillVolume : MonoBehaviour
{
    
    public Action onEnterVolume;
    
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            onEnterVolume?.Invoke();
    }
}