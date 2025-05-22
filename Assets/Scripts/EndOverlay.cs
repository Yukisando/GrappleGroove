#region

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class EndOverlay : MonoBehaviour
{
    public static EndOverlay I;
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI bestTime;

    void Awake() {
        if (I == null) I = this;
        else Destroy(gameObject);
    }

    public void Populate() {
        time.text = "Current: " + PlayerOverlay.I.GetTimeString();
        bestTime.text = "Best: " + (PlayerPrefs.HasKey($"best_{SceneManager.GetActiveScene().name}") ? PlayerPrefs.GetFloat($"best_{SceneManager.GetActiveScene().name}") : "NaN");
    }
}