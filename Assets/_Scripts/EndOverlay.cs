#region

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class EndOverlay : MonoBehaviour
{
    public static EndOverlay I;

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI bestTimeText;

    void Awake() {
        I = this;
    }

    public void Populate() {
        timeText.text = $"Time: {PlayerOverlay.I.GetTimeString()}";

        var key = $"best_{SceneManager.GetActiveScene().name}";
        if (PlayerPrefs.HasKey(key)) {
            float bestTime = PlayerPrefs.GetFloat(key);
            bestTimeText.enabled = true;
            bestTimeText.text = $"Best: {FormatTime(bestTime)}";
        }
        else
            bestTimeText.enabled = false;
    }

    string FormatTime(float seconds) {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        float remainingSeconds = seconds % 60f;
        return $"{minutes:00}:{remainingSeconds:00.000}";
    }
}