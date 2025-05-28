#region

using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random; // Explicitly use UnityEngine.Random

#endregion

[InfoBox("Sets a checkpoint for the player when they enter the volume")]
public class CheckpointVolume : MonoBehaviour
{
    [ReadOnly] public string checkpointId; // Unique ID for this checkpoint
    [SerializeField] Vector3 spawnOffset = Vector3.zero;
    [SerializeField] bool deactivateOnEnter = true;
    public Action<CheckpointVolume> onEnterVolume;

    void Awake() {
        if (string.IsNullOrEmpty(checkpointId)) checkpointId = "checkpoint_" + Random.Range(100000, 999999);
    }

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
        if (deactivateOnEnter) gameObject.SetActive(false);
        onEnterVolume?.Invoke(this);
    }
}