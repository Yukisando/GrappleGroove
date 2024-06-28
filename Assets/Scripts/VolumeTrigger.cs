#region

using UnityEngine;
using UnityEngine.Events;

#endregion

public class VolumeTrigger : MonoBehaviour
{
    static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    public UnityEvent onEnter;
    public string tagMask = "Player";
    public bool destroyObjectPostTrigger;

    [Header("Material Offset")]
    [SerializeField] float yMovement = 0.2f;
    [SerializeField] float xMovement = 0.1f;
    MeshRenderer meshRenderer;

    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void LateUpdate() {
        if (meshRenderer != null && meshRenderer.material != null) {
            var offset = meshRenderer.material.GetTextureOffset(BaseMap);
            offset.y += yMovement * Time.deltaTime;
            offset.x += xMovement * Time.deltaTime;
            meshRenderer.material.SetTextureOffset(BaseMap, offset);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null) {
            Gizmos.DrawWireMesh(meshFilter.sharedMesh, transform.position, transform.rotation, transform.localScale);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag(tagMask)) return;
        onEnter?.Invoke();
        if (destroyObjectPostTrigger) {
            Destroy(gameObject);
        }
    }
}