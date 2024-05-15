#region

using UnityEngine;

#endregion

public class ObjectReseter : MonoBehaviour
{
    public Transform respawnPoint;
    
    void OnTriggerEnter(Collider other) {
        other.gameObject.transform.position = respawnPoint.position;
    }
}