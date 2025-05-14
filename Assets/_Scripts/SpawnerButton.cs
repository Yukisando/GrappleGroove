#region

using UnityEngine;

#endregion

public class SpawnerButton : GameButton
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform spawnPoint;

    GameObject spawnedObject;

    public void Spawn() {
        if (spawnedObject) Destroy(spawnedObject);
        spawnedObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, spawnPoint.position);
    }
}