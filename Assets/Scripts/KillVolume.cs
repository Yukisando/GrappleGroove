#region

using System;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("Restarts the entire level whent he player enters the volume")]
public class KillVolume : MonoBehaviour
{
    public Action onEnterVolume;

    void OnTriggerEnter(Collider _other) {
        if (_other.CompareTag("PlayerHitbox"))
            onEnterVolume?.Invoke();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = Matrix4x4.identity;
    }
}