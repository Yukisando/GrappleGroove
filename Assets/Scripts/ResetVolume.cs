#region

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("Resets specified objects (including player) to their initial state when entered")]
public class ResetVolume : MonoBehaviour
{
    [SerializeField] List<string> idsToReset = new List<string> {
        "Player",
    };

    [FoldoutGroup("Material settings")]
    [SerializeField] float yMovement = .2f;
    [FoldoutGroup("Material settings")]
    [SerializeField] float xMovement = .1f;
    [FoldoutGroup("Material settings")]
    [SerializeField] int updateFrameInterval = 2;

    static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    int frameCounter;
    Material sharedMaterial;
    public Action<bool> onPlayerEntered;

    MeshRenderer rd;

    void Awake() {
        rd = GetComponent<MeshRenderer>();


        sharedMaterial = new Material(rd.material);
        rd.material = sharedMaterial;
    }

    void OnDestroy() {
        // Clean up the material instance
        if (sharedMaterial) Destroy(sharedMaterial);
    }

    void LateUpdate() {
        if (!rd || !sharedMaterial || !sharedMaterial.HasProperty(BaseMap)) return;


        frameCounter++;
        if (frameCounter % updateFrameInterval != 0) return;

        var offset = sharedMaterial.GetTextureOffset(BaseMap);
        offset.y += yMovement * Time.deltaTime * updateFrameInterval;
        offset.x += xMovement * Time.deltaTime * updateFrameInterval;
        sharedMaterial.SetTextureOffset(BaseMap, offset);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, transform.localScale);
    }

    void OnTriggerEnter(Collider _other) {
        HandleObjectEnter(_other.gameObject);
    }

    void OnCollisionEnter(Collision _collision) {
        HandleObjectEnter(_collision.gameObject);
    }

    void HandleObjectEnter(GameObject enteringObject) {
        if (!enteringObject.TryGetComponent<ID>(out var idObject)) return;

        if (idObject.id == "Player") {
            onPlayerEntered?.Invoke(true);
            return;
        }

        if (idsToReset.Contains(idObject.id)) idObject.ResetObject();
    }
}