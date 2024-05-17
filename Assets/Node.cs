#region

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class Node : MonoBehaviour
{
    public TMP_Text idText;
    public Image iconImage;
    Button button;
    NodeData nodeData;
    
    void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    
    void Start() {
        Populate(nodeData);
    }
    
    public void Populate(NodeData _nodeData) {
        nodeData = _nodeData;
        idText.text = nodeData.id;
        iconImage.sprite = nodeData.icon;
    }
    
    void OnClick() {
        Debug.Log($"{nodeData.id} clicked!");
    }
}