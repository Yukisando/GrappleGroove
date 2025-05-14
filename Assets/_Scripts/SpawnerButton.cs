#region

using UnityEngine;

#endregion

public class SpawnerButton : GameButton
{
    [SerializeField] GameObject prefab;
    [SerializeField] string idOnSpawn;
    [SerializeField] Transform spawnPoint;

    GameObject spawnedObject;

    public void Spawn() {
        if (spawnedObject) Destroy(spawnedObject);
        spawnedObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        var IDComponent = spawnedObject.GetComponent<ID>();
        if (IDComponent) IDComponent.id = IDComponent.id == string.Empty ? idOnSpawn : IDComponent.id;
        else {
            IDComponent = spawnedObject.AddComponent<ID>();
            IDComponent.id = idOnSpawn;
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, spawnPoint.position);
    }
}