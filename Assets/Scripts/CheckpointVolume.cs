#region

using UnityEngine;

#endregion

public class CheckpointVolume : MonoBehaviour
{
    public Transform checkpoint;
    
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            checkpoint.position = transform.position;
            Debug.Log("Checkpoint!");
            gameObject.SetActive(false);
        }
    }
}