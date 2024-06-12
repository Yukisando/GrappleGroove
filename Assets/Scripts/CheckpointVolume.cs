#region

using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

#endregion

[Serializable]
public class CheckpointVolume : MonoBehaviour
{
    [FormerlySerializedAs("idd")] [ReadOnly]
    public string id;
    public Action<CheckpointVolume> onEnterVolume;

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;

        PlayerPrefs.SetString("lastCheckpoint", id);
        onEnterVolume?.Invoke(this);
    }
}