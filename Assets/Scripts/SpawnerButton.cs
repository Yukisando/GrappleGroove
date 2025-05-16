#region

using UnityEngine;

#endregion

public class SpawnerButton : GameButton
{
    [SerializeField] ID prefab;
    [SerializeField] string idOnSpawn;
    [SerializeField] Transform spawnPoint;

    ID spawnedObject;

    void OnValidate() {
        if (prefab && idOnSpawn == "") idOnSpawn = prefab.id;
    }

    public void Spawn() {
        if (spawnedObject != null) Destroy(spawnedObject.gameObject);
        spawnedObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        spawnedObject.id = idOnSpawn;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, spawnPoint.position);
    }
}