#region

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#endregion

public class MenuOverlay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bestTimeText;

    [SerializeField] Toggle checkpointToggle;

    public void Populate() {
        if (PlayerPrefs.HasKey($"best_{SceneManager.GetActiveScene().name}")) {
            bestTimeText.enabled = true;
            bestTimeText.text = "Best on track: " + PlayerPrefs.GetFloat($"best_{SceneManager.GetActiveScene().name}");
        }
        else
            bestTimeText.enabled = false;

        checkpointToggle.SetIsOnWithoutNotify(GameManager.I.checkpointsActive);
    }
}