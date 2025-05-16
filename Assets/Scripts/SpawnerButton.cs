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

    readonly List<ID> spawnedIDs = new List<ID>();

    ID spawnedObject;

    void OnValidate() {
        if (prefab && idOnSpawn == "") idOnSpawn = prefab.id;
    }

    public void Spawn() {
        if (spawnedIDs.Count >= maxSpawnCount) Destroy(spawnedIDs.Last().gameObject);
        spawnedObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        spawnedObject.id = idOnSpawn;
        spawnedIDs.Add(spawnedObject);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, spawnPoint.position);
    }
}