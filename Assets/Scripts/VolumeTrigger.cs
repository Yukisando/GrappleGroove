#region

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

#endregion

[InfoBox("Trigger specified events when objects with specified IDs enter")]
public class VolumeTrigger : MonoBehaviour
{
    public bool destroyOnTrigger;
    public string id = "Player";
    public UnityEvent onEnter;

    [FoldoutGroup("Material settings")]
    [SerializeField] float yMovement = 0.2f;
    [FoldoutGroup("Material settings")]
    [SerializeField] float xMovement = 0.1f;
    [FoldoutGroup("Material settings")]
    [SerializeField] int updateFrameInterval = 2;

    static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    int frameCounter;
    Material sharedMaterial;
    MeshRenderer meshRenderer;

    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();


        if (meshRenderer != null && meshRenderer.material != null) {
            sharedMaterial = new Material(meshRenderer.material);
            meshRenderer.material = sharedMaterial;
        }
    }

    void OnDestroy() {
        // Clean up the material instance
        if (sharedMaterial != null) Destroy(sharedMaterial);
    }

    void LateUpdate() {
        if (meshRenderer == null || sharedMaterial == null || !sharedMaterial.HasProperty(BaseMap)) return;


        frameCounter++;
        if (frameCounter % updateFrameInterval != 0) return;

        var offset = sharedMaterial.GetTextureOffset(BaseMap);
        offset.y += yMovement * Time.deltaTime * updateFrameInterval;
        offset.x += xMovement * Time.deltaTime * updateFrameInterval;
        sharedMaterial.SetTextureOffset(BaseMap, offset);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null) Gizmos.DrawWireMesh(meshFilter.sharedMesh, transform.position, transform.rotation, transform.localScale);
    }

    void OnTriggerEnter(Collider other) {
        other.TryGetComponent<ID>(out var triggerObject);
        if (triggerObject == null || triggerObject.id != id) return;
        onEnter?.Invoke();
        if (destroyOnTrigger) Destroy(gameObject);
    }
}