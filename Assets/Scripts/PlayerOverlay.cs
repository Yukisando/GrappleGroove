#region

using TMPro;
using UnityEngine;

#endregion

public class PlayerOverlay : MonoBehaviour
{
    public static PlayerOverlay I;

    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI timerText;

    void Awake() {
        if (I == null) I = this;
        else Destroy(gameObject);
    }

    void Start() {
        timerText.enabled = false;
    }

    public void SetSpeed(float velocity) {
        speedText.text = Mathf.RoundToInt(velocity) + " m/s";
    }

    public void SetTimer(float time) {
        timerText.enabled = true;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt(time % 1f * 1000f);
        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}