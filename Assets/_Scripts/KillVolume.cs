#region

using System;
using UnityEngine;

#endregion

public class KillVolume : MonoBehaviour
{
    public Action onEnterVolume;

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    void OnTriggerEnter(Collider _other) {
        if (_other.CompareTag("PlayerHitbox"))
            onEnterVolume?.Invoke();
    }
}