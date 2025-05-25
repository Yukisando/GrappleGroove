#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

public class SoundOnActivate : MonoBehaviour
{
    [Optional] AudioClip clip;
    void OnEnable() {
        AssetManager.I.PlayClip(clip?? AssetManager.I.objectSpawn);
    }
}