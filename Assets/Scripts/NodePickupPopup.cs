#region

using UnityEngine;

#endregion

public class NodePickupPopup : MonoBehaviour
{
    public float height = 100f;
    public float duration = 2f;
    
    void Start() {
        gameObject.SetActive(false);
        ScratchManager.I.onNewNode += ShowPopup;
    }
    
    void ShowPopup(NodeData nodeData) {
        gameObject.SetActive(true);
        transform.LeanMoveY(height, duration).setEaseOutBounce().setOnComplete(() => Destroy(gameObject));
        Debug.Log($"Picked up {nodeData.id}!");
    }
}