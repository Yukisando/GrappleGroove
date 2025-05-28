#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

public class SoundOnActivate : MonoBehaviour
{
    [SerializeField] [InfoBox("Optional")] AudioClip clip;

    void OnEnable() {
        var clipPoint = new ClipPoint {
            clip = clip ?? AssetManager.I.objectSpawn,
            transform = transform,
        };

        AssetManager.I.PlayClipAt(clipPoint);
    }
}