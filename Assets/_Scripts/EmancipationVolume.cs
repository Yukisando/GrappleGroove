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
        if (rd == null || sharedMaterial == null || !sharedMaterial.HasProperty(BaseMap)) return;

        // Only update every X frames
        frameCounter++;
        if (frameCounter % updateFrameInterval != 0) return;

        var offset = sharedMaterial.GetTextureOffset(BaseMap);
        offset.y += yMovement * Time.deltaTime * updateFrameInterval; // Multiply by interval to maintain same speed
        offset.x += xMovement * Time.deltaTime * updateFrameInterval;
        sharedMaterial.SetTextureOffset(BaseMap, offset);
    }

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        onEnterVolume?.Invoke(ropeTypeToDestroy);
    }

    void SetVolumeColor() {
        rd.material.EnableKeyword("_BaseMap");

        switch (ropeTypeToDestroy) {
            case RopeType.BOTH:
                rd.material.SetColor(BaseColor, Color.magenta);
                break;
            case RopeType.LEFT:
                rd.material.SetColor(BaseColor, Color.blue);
                break;
            case RopeType.RIGHT:
                rd.material.SetColor(BaseColor, Color.red);
                break;
        }
    }
}