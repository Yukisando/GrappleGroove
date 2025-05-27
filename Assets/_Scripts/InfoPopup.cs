#region

using TMPro;
using UnityEngine;

#endregion

public class InfoPopup : MonoBehaviour
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
        startY = rectTransform.anchoredPosition.y;
    }

    void Init() {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startY);
    }

    public void ShowPopup(string _info = "New info!", bool playAudio = true) {
        Init();
        text.text = _info;
        Debug.Log($"Popup: {_info}");
        rectTransform.LeanMoveY(height, duration).setEaseOutBounce().setOnComplete(Init);
        if (playAudio) AssetManager.I.PlayClip();
    }
}