#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("Play sound when colliding")]
[RequireComponent(typeof(AudioSource))]
public class HitSound : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip hitSound;
    [SerializeField] float pitchRange = 0.3f;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 20f;
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "Water") {
            AssetManager.I.PlayClipAt(AssetManager.I.splashClip, transform.position);
            return;
        }

        audioSource.pitch = Random.Range(1 - pitchRange, 1 + pitchRange);
        audioSource.PlayOneShot(hitSound);
    }
}