#region

using UnityEngine;

#endregion

public class NodePickupPopup : MonoBehaviour
{
    public float height;
    public float speed;
    
    void Awake() {
        gameObject.SetActive(false);
    }
    
    public void ShowPopup(NodeData nodeData) {
        gameObject.SetActive(true);
        transform.LeanMoveY(height, speed).setEaseOutBounce().setOnComplete(() => Destroy(gameObject));
        Debug.Log($"Picked up {nodeData.id}!");
    }
}