#region

using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class KillZone : MonoBehaviour
{
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}