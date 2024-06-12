#region

using System;
using UnityEngine;

#endregion

[Serializable]
public class CheckpointVolume : MonoBehaviour
{
    public Action<CheckpointVolume> onEnterVolume;

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;


        onEnterVolume?.Invoke(this);
    }
}