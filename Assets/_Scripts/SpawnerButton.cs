#region

using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("Spawns objects when button in pressed")]
public class SpawnerButton : GameButton
{
    [SerializeField] ID prefab;
    [SerializeField] string idOnSpawn;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int maxSpawnCount = 1;
    [SerializeField] bool spawnSound = true;

    readonly List<ID> spawnedIDs = new List<ID>();

    ID spawnedObject;

    void OnValidate() {
        if (prefab && idOnSpawn == "") idOnSpawn = prefab.id;
    }

    public void Spawn() {
        if (spawnedIDs.Count >= maxSpawnCount) {
            var objToDelete = spawnedIDs.First();
            spawnedIDs.Remove(objToDelete);
            if (objToDelete) objToDelete.Despawn();
        }
        spawnedObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        spawnedObject.gameObject.AddComponent<SoundOnActivate>();
        spawnedObject.id = idOnSpawn;
        spawnedObject.spawned = true;
        spawnedIDs.Add(spawnedObject);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, spawnPoint.position);
    }
}