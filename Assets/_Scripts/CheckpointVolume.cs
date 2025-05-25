#region

using System;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("Sets a checkpoint for the player when they enter the volume")]
public class CheckpointVolume : MonoBehaviour
{
    [SerializeField] Vector3 spawnOffset = Vector3.zero;
    [SerializeField] bool deactivateOnEnter = true;
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
        if (deactivateOnEnter) gameObject.SetActive(false);
        onEnterVolume?.Invoke(spawnPoint);
    }
}