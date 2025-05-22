#region

using TMPro;
using UnityEngine;

#endregion

public class PlayerOverlay : MonoBehaviour
{
    public static PlayerOverlay I;

    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI timerText;

    string timeString = "NaN";

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
        timeString = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
        timerText.text = timeString;
    }

    public string GetTimeString() {
        return timeString;
    }
}