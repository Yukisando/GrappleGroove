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

    void Start() {
        ScratchManager.I.onNewNode += ShowPopup;
        startY = rectTransform.anchoredPosition.y;
    }

    void Init() {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startY);
    }

    [Button]
    void ShowPopup(string _info = "New info!") {
        Init();
        text.text = _info;
        Debug.Log($"{_info}");
        rectTransform.LeanMoveY(height, duration).setEaseOutBounce().setOnComplete(Init);
    }
}