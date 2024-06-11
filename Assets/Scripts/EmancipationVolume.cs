#region

using System;
using PrototypeFPC;
using UnityEngine;
using UnityEngine.Serialization;

#endregion

public class EmancipationVolume : MonoBehaviour
{
    [FormerlySerializedAs("hookTypeToDestroy")]
    public RopeType ropeTypeToDestroy;
    public Action<RopeType> onEnterVolume;

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        onEnterVolume?.Invoke(ropeTypeToDestroy);
    }
}