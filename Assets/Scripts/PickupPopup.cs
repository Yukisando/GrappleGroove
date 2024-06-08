#region

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

#endregion

public class PickupPopup : MonoBehaviour
{
    public float height = 100f;
    public float duration = 2f;
    RectTransform rectTransform;
    
    float startY;
    TMP_Text text;
    
    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponent<TMP_Text>();
    }
    
    void Reset() {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startY);
    }
    
    void Start() {
        ScratchManager.I.onNewNode += ShowPopup;
        startY = rectTransform.anchoredPosition.y;
    }
    
    [Button]
    void ShowPopup(string _info = "New info!") {
        Reset();
        text.text = _info;
        Debug.Log($"{_info}");
        rectTransform.LeanMoveY(height, duration).setEaseOutBounce().setOnComplete(Reset);
    }
}