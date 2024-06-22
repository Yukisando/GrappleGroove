#region

using System;
using UnityEngine;

#endregion

public class ResetVolume : MonoBehaviour
{
    static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

    [Header("Extras")]
    [SerializeField] bool deactivateOnEnter = true;
    [SerializeField] float yMovement = .2f;
    [SerializeField] float xMovement = .1f;
    public Action<bool> onEnterVolume;

    MeshRenderer rd;

    void Awake() {
        rd = GetComponent<MeshRenderer>();
    }

    void LateUpdate() {
        if (rd.material.HasProperty(BaseMap) == false) return;
        var offset = rd.material.GetTextureOffset(BaseMap);
        offset.y += yMovement * Time.deltaTime;
        offset.x += xMovement * Time.deltaTime;
        rd.material.SetTextureOffset(BaseMap, offset);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, transform.localScale);
    }

    void OnTriggerEnter(Collider _other) {
        if (!_other.CompareTag("PlayerHitbox")) return;
        if (deactivateOnEnter) gameObject.SetActive(false);
        onEnterVolume?.Invoke(true);
    }
}