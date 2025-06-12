#region

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

#endregion

[InfoBox("Trigger specified events when objects with specified IDs enters or collides")] [RequireComponent(typeof(ID))]
public class TriggerVolume : MonoBehaviour
{
    [SerializeField] bool destroyVolumeOnTrigger;
    [SerializeField] bool destroyObjectOnTrigger;
    [SerializeField] List<string> ids = new List<string>();
    [SerializeField] string message = "Tada!";
    [SerializeField] UnityEvent onAnyEnter;
    [HideIf("@this.destroyObjectOnTrigger || this.destroyVolumeOnTrigger")]
    [SerializeField] UnityEvent onAllExit;

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

    readonly List<ID> idsInside = new List<ID>();

    void OnTriggerEnter(Collider _other) {
        HandleObjectEnter(_other.gameObject);
    }

    void OnCollisionEnter(Collision _collision) {
        HandleObjectEnter(_collision.gameObject);
    }

    void HandleObjectEnter(GameObject other) {
        other.TryGetComponent<ID>(out var triggerObject);
        if (triggerObject && ids.Contains(triggerObject.id)) {
            onAnyEnter?.Invoke();
            if (!string.IsNullOrEmpty(message)) GameManager.I.PopupMessage(message);
            if (destroyObjectOnTrigger) triggerObject.Despawn();
            else idsInside.Add(triggerObject);
            if (destroyVolumeOnTrigger) GetComponent<ID>().Despawn();
        }
    }

    void OnTriggerExit(Collider other) {
        other.TryGetComponent<ID>(out var triggerObject);
        if (triggerObject && idsInside.Contains(triggerObject)) {
            idsInside.Remove(triggerObject);
            if (idsInside.Count == 0) onAllExit?.Invoke();
        }
    }
}