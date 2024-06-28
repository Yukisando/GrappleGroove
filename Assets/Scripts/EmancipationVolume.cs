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

    MeshRenderer rd;

    void Awake() {
        rd = GetComponent<MeshRenderer>();
    }

    void Start() {
        SetVolumeColor();
    }

    void LateUpdate() {
        var offset = rd.material.GetTextureOffset(BaseMap);
        offset.y += yMovement * Time.deltaTime;
        offset.x += xMovement * Time.deltaTime;
        rd.material.SetTextureOffset(BaseMap, offset);
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