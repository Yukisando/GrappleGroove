#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

public class SoundOnActivate : MonoBehaviour
{
    [SerializeField] [InfoBox("Optional")] AudioClip clip;

    void OnEnable() {
        AssetManager.I.PlayClipAt(clip ?? AssetManager.I.objectSpawn, transform.position);
    }
}