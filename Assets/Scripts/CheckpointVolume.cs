#region

using System;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[Serializable]
public class CheckpointVolume : MonoBehaviour
{
    [ReadOnly] public string idd;
    public Action<Transform> onEnterVolume;

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        PlayerPrefs.SetString("lastCheckpoint", idd);
        onEnterVolume?.Invoke(transform);
    }
}