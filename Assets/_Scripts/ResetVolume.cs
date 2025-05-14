#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class ResetVolume : MonoBehaviour
{
    static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

    [SerializeField] List<string> ids = new List<string> {
        "PlayerHitbox",
    };

    [Header("Visuals")]
    [SerializeField] float yMovement = .2f;
    [SerializeField] float xMovement = .1f;
    [SerializeField] int updateFrameInterval = 2;
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
        if (sharedMaterial != null) Destroy(sharedMaterial);
    }

    void LateUpdate() {
        if (rd == null || sharedMaterial == null || !sharedMaterial.HasProperty(BaseMap)) return;


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
        if (!_other.CompareTag("PlayerHitbox")) return;
        onPlayerEntered?.Invoke(true);
    }
}