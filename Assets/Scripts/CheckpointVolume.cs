#region

using System;
using UnityEngine;

#endregion

[Serializable]
public class CheckpointVolume : MonoBehaviour
{
    [SerializeField] Vector3 spawnOffset = Vector3.zero;
    public Action<Vector3> onEnterVolume;

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, transform.localScale);
    }

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;

        onEnterVolume?.Invoke(transform.position + spawnOffset);
    }
}