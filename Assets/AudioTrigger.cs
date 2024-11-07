#region

using UnityEngine;

#endregion

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    [SerializeField] bool playOnlyOnce;
    AudioSource audioSource;

    bool played;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        if (played && playOnlyOnce) return;

        played = true;
        Debug.Log($"Playing recording: {audioClip.name}");
        audioSource.PlayOneShot(audioClip);
    }
}