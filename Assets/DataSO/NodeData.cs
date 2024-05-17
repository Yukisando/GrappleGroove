#region

using UnityEngine;

#endregion

[CreateAssetMenu(fileName = "New node", menuName = "Node data", order = 0)]
public class NodeData : ScriptableObject
{
    public string id;
    public Sprite icon;
    public bool unique = true;
}