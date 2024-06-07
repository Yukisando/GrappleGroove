#region

using System;
using UnityEngine;

#endregion

public class NodePickupVolume : MonoBehaviour
{
    public NodeData nodeData;
    public Action<NodeData> onEnterVolume;
    
    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        onEnterVolume?.Invoke(nodeData);
    }
}