#region

using System;
using PrototypeFPC;
using UnityEngine;

#endregion

public class EmancipationVolume : MonoBehaviour
{
    static readonly int MainTex = Shader.PropertyToID("_MainTex");
    static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    public RopeType ropeTypeToDestroy;
    public Action<RopeType> onEnterVolume;

    MeshRenderer rd;

    void Awake() {
        rd = GetComponent<MeshRenderer>();
    }

    void Start() {
        SetVolumeColor();
    }

    void LateUpdate() {
        var offset = rd.material.GetTextureOffset(MainTex);
        offset.y += 5 * Time.deltaTime;
        offset.x += 2 * Time.deltaTime;
        rd.material.SetTextureOffset(MainTex, offset);
    }

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        onEnterVolume?.Invoke(ropeTypeToDestroy);
    }

    void SetVolumeColor() {
        // Enable emission
        rd.material.EnableKeyword("_EMISSION");

        switch (ropeTypeToDestroy) {
            case RopeType.BOTH:
                rd.material.SetColor(EmissionColor, Color.magenta);
                break;
            case RopeType.LEFT:
                rd.material.SetColor(EmissionColor, Color.blue);
                break;
            case RopeType.RIGHT:
                rd.material.SetColor(EmissionColor, Color.red);
                break;
        }
    }
}