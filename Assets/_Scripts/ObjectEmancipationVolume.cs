#region

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("Destroys objects with the corresponding IDs")]
public class ObjectEmancipationVolume : MonoBehaviour
{
    [SerializeField] List<string> idsToDestroy = new List<string>();

    [FoldoutGroup("Material settings")]
    [SerializeField] float yMovement = .2f;
    [FoldoutGroup("Material settings")]
    [SerializeField] float xMovement = .1f;
    [FoldoutGroup("Material settings")]
    [SerializeField] int updateFrameInterval = 2;

    static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    int frameCounter;
    Material sharedMaterial;
    MeshRenderer rd;

    void Awake() {
        rd = GetComponent<MeshRenderer>();
        sharedMaterial = new Material(rd.material);
        rd.material = sharedMaterial;
    }

    void OnDestroy() {
        // Clean up the material instance
        if (sharedMaterial != null) Destroy(sharedMaterial);
    }

    void LateUpdate() {
        if (frameCounter++ % updateFrameInterval != 0) return;

        if (sharedMaterial.HasProperty(BaseMap)) {
            var offset = sharedMaterial.GetTextureOffset(BaseMap);
            offset += new Vector2(xMovement, yMovement) * (Time.deltaTime * updateFrameInterval);
            sharedMaterial.SetTextureOffset(BaseMap, offset);
        }
    }

    void OnTriggerEnter(Collider _other) {
        HandleObjectEnter(_other.gameObject);
    }

    void OnCollisionEnter(Collision _collision) {
        HandleObjectEnter(_collision.gameObject);
    }

    void HandleObjectEnter(GameObject enteringObject) {
        if (!enteringObject.TryGetComponent<ID>(out var idObject)) return;

        if (idsToDestroy.Contains(idObject.id)) idObject.Despawn();
    }
}