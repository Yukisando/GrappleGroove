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
    bool isShowing;
    LTDescr currentTween;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponent<TMP_Text>();
    }

    void Start() {
        startY = rectTransform.anchoredPosition.y;
        Hide();
    }

    void Init() {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startY);
    }

    void Hide() {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startY);
        isShowing = false;
    }

    public void ShowPopup(string _info = "Something happened", bool playAudio = true) {
        if (isShowing) {
            // Cancel the current tween if a popup is already showing
            if (currentTween != null) LeanTween.cancel(currentTween.id);
            Hide();
        }

        isShowing = true;
        Init();
        text.text = _info;
        Debug.Log($"Popup: {_info}");
        currentTween = rectTransform.LeanMoveY(height, duration).setEaseOutBounce().setOnComplete(() => {
            Hide();
        });

        if (playAudio) AssetManager.I.PlayClip();
    }
}