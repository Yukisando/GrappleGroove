#region

using UnityEngine;
using UnityEngine.Events;

#endregion

public class VolumeTrigger : MonoBehaviour
{
    static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    public string id = "Player";
    public bool destroyOnTrigger;
    public UnityEvent onEnter;

    [Header("Material Offset")]
    [SerializeField] float yMovement = 0.2f;
    [SerializeField] float xMovement = 0.1f;
    [SerializeField] int updateFrameInterval = 2; // Only update every X frames
    int frameCounter;
    Material sharedMaterial;
    MeshRenderer meshRenderer;

    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();

        // Create a material instance to avoid affecting other objects
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

        // Only update every X frames
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
        other.TryGetComponent<TriggerObject>(out var triggerObject);
        if (triggerObject == null || triggerObject.id != id) return;
        onEnter?.Invoke();
        if (destroyOnTrigger) Destroy(gameObject);
    }
}