#region

using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}