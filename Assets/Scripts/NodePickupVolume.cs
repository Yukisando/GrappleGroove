#region

using UnityEngine;

#endregion

public class NodePickupVolume : MonoBehaviour
{
    public NodeData nodeData;
    
    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        
        ScratchManager.I.AddNode(nodeData);
    }
}