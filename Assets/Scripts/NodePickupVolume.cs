#region

using UnityEngine;

#endregion

public class NodePickupVolume : MonoBehaviour
{
    public NodeData nodeData;
    
    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        
        Debug.Log($"Picked up {nodeData.id}!");
        ScratchManager.I.AddNode(nodeData);
    }
}