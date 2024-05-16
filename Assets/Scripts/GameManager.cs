#region

using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class GameManager : MonoBehaviour
{
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        }
        
        if (Input.GetKeyDown(KeyCode.F5)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}