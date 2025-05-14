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

        void Awake() {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 20f;
        }

        void OnCollisionEnter(Collision col) {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(hitSound);
        }
    }
}