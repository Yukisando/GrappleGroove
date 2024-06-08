#region

using System;
using UnityEngine;

#endregion

public class ResetVolume : MonoBehaviour
{
    public Action onEnterVolume;

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        onEnterVolume?.Invoke();
    }
}