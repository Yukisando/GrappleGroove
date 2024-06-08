#region

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class KillVolume : MonoBehaviour
{
    
    public Action onEnterVolume;
    
    void OnTriggerEnter(Collider _other) {
        if (_other.CompareTag("PlayerHitbox"))
            onEnterVolume?.Invoke();
    }
}