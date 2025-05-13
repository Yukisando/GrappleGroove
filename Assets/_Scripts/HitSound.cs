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
        }

        void OnCollisionEnter(Collision col) {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(hitSound);
        }
    }
}