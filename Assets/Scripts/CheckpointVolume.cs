#region

using System;
using UnityEngine;

#endregion

[Serializable]
public class CheckpointVolume : MonoBehaviour
{
    [SerializeField] Vector3 spawnOffset = Vector3.zero;
    public Action<Transform> onEnterVolume;

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, transform.localScale);
        Gizmos.color = Color.magenta;
        var point = transform.position + spawnOffset;
        Gizmos.DrawWireSphere(point, .4f);
        Gizmos.DrawLine(point, point + transform.forward * 4f);
    }

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        var spawnPoint = transform;
        spawnPoint.position += spawnOffset;
        onEnterVolume?.Invoke(spawnPoint);
    }
}