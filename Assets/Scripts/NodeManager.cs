#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class NodeManager : MonoBehaviour
{
    readonly List<NodeData> nodes = new List<NodeData>();
    
    void Update() {
        foreach (var node in nodes) {
            if (node.id == "Gun") {
                Debug.Log("Gun node found");
            }
        }
    }
}