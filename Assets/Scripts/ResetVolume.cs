#region

using System;
using UnityEngine;

#endregion

public class ResetVolume : MonoBehaviour
{
    public Action onEnterVolume;

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        onEnterVolume?.Invoke();
    }
}