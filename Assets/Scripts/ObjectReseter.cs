#region

using UnityEngine;

#endregion

public class ObjectReseter : MonoBehaviour
{
    public Transform respawnPoint;
    public string tagMask = "Player";
    
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(tagMask)) {
            other.gameObject.transform.position = respawnPoint.position;
        }
    }
}