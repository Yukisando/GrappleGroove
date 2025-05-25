#region

using UnityEngine;

#endregion

[RequireComponent(typeof(AudioSource))]
public class SoundOnActivate : MonoBehaviour
{
    AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1.0f;
        audioSource.maxDistance = 20.0f;
    }

    void OnEnable() {
        audioSource.PlayOneShot(AssetManager.I.spawnClip);
    }
}