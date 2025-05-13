#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class ObjectEmancipationVolume : MonoBehaviour
{
    static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

    [SerializeField] List<string> idsToDestroy = new List<string>();

    [Header("Visuals")]
    [SerializeField] float yMovement = .2f;
    [SerializeField] float xMovement = .1f;
    [SerializeField] int updateFrameInterval = 2;
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
        foreach (string o in idsToDestroy) {
            if (!enteringObject.TryGetComponent<ID>(out var idObject)) continue;

            if (idObject.id == o) {
                Destroy(enteringObject);
                Debug.Log($"{enteringObject.name} destroyed by {gameObject.name}");
            }
        }
    }
}