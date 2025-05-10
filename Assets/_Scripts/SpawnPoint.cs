#region

using UnityEngine;

#endregion

public class SpawnPoint : MonoBehaviour
{
    void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 1f);
    }
}