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
        time.text = "Time: " + PlayerOverlay.I.GetTimeString();

        if (PlayerPrefs.HasKey($"best_{SceneManager.GetActiveScene().name}")) {
            bestTime.enabled = true;
            bestTime.text = "Best: " + PlayerPrefs.GetFloat($"best_{SceneManager.GetActiveScene().name}");
        }
        else bestTime.enabled = false;
    }
}