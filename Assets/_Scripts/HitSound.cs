#region

using UnityEngine;

#endregion

namespace PrototypeFPC
{
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
            audioSource.pitch = Random.Range(1 - pitchRange, 1 + pitchRange);
            audioSource.PlayOneShot(hitSound);
        }
    }
}