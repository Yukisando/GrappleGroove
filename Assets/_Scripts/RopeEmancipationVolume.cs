#region

using System;
using PrototypeFPC;
using UnityEngine;

#endregion

public class RopeEmancipationVolume : MonoBehaviour
{
    static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    [SerializeField] RopeType ropeTypeToDestroy;

    public Action<RopeType> onEnterVolume;

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

    void Start() {
        SetVolumeColor();
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