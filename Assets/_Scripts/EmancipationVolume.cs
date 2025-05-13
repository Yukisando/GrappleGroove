#region

using System;
using PrototypeFPC;
using UnityEngine;

#endregion

public class EmancipationVolume : MonoBehaviour
{
    static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    [SerializeField] RopeType ropeTypeToDestroy;

    [Header("Extras")]
    [SerializeField] float yMovement = .2f;
    [SerializeField] float xMovement = .1f;
    public Action<RopeType> onEnterVolume;

    [Header("Performance")]
    [SerializeField] int updateFrameInterval = 2; // Only update every X frames
    int frameCounter;
    Material sharedMaterial;

    MeshRenderer rd;

    void Awake() {
        rd = GetComponent<MeshRenderer>();

        // Create a material instance to avoid affecting other objects
        if (rd != null && rd.material != null) {
            sharedMaterial = new Material(rd.material);
            rd.material = sharedMaterial;
        }
    }

    void OnDestroy() {
        // Clean up the material instance
        if (sharedMaterial != null) Destroy(sharedMaterial);
    }

    void Start() {
        SetVolumeColor();
    }

    void LateUpdate() {
        // Only update every X frames
        if (frameCounter++ % updateFrameInterval != 0) return;

        if (sharedMaterial.HasProperty(BaseMap)) {
            var offset = sharedMaterial.GetTextureOffset(BaseMap);
            offset += new Vector2(xMovement, yMovement) * (Time.deltaTime * updateFrameInterval);
            sharedMaterial.SetTextureOffset(BaseMap, offset);
        }
    }

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        onEnterVolume?.Invoke(ropeTypeToDestroy);
    }

    void SetVolumeColor() {
        sharedMaterial.EnableKeyword("_BaseMap");

        switch (ropeTypeToDestroy) {
            case RopeType.BOTH:
                sharedMaterial.SetColor(BaseColor, Color.magenta);
                break;
            case RopeType.LEFT:
                sharedMaterial.SetColor(BaseColor, Color.blue);
                break;
            case RopeType.RIGHT:
                sharedMaterial.SetColor(BaseColor, Color.red);
                break;
        }
    }
}