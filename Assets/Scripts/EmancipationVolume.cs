#region

using System;
using PrototypeFPC;
using UnityEngine;

#endregion

public class EmancipationVolume : MonoBehaviour
{
    static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    [SerializeField] RopeType ropeTypeToDestroy;
    
    [Header("Extras")]
    [SerializeField] float yMovement = .2f;
    [SerializeField] float xMovement = .1f;
    public Action<RopeType> onEnterVolume;
    
    MeshRenderer rd;
    
    void Awake() {
        rd = GetComponent<MeshRenderer>();
        if (rd == null) {
            Debug.LogError("MeshRenderer component missing.");
        }
    }
    
    void Start() {
        SetVolumeColor();
    }
    
    void LateUpdate() {
        if (rd != null && rd.material != null) {
            var offset = rd.material.GetTextureOffset(BaseMap);
            offset.y += yMovement * Time.deltaTime;
            offset.x += xMovement * Time.deltaTime;
            rd.material.SetTextureOffset(BaseMap, offset);
            
            // Debug logs to check offset values
            Debug.Log($"Offset X: {offset.x}, Offset Y: {offset.y}");
        }
    }
    
    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        onEnterVolume?.Invoke(ropeTypeToDestroy);
    }
    
    void SetVolumeColor() {
        // Enable emission
        if (rd != null && rd.material != null) {
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
}