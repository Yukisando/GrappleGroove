#region

using System;
using UnityEngine;

#endregion

public class NodePickupVolume : MonoBehaviour
{
    public NodeData nodeData;
    public Action<NodeData> onEnterVolume;

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        onEnterVolume?.Invoke(nodeData);
    }
}