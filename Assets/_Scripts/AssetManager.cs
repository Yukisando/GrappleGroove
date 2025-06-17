#region

using UnityEngine;
using Random = UnityEngine.Random;

#endregion

public class AssetManager : MonoBehaviour
{
    public static AssetManager I;

    public AudioClip notificationClip;
    public AudioClip spawnClip;
    public AudioClip onClip;
    public AudioClip offClip;
    public AudioClip defaultRaceClip;
    public AudioClip checkpointSound;
    public AudioClip startTimerSound;
    public AudioClip stopTimerSound;
    public AudioClip timerTickSound;
    public AudioClip objectSpawn;
    public AudioClip splashClip;

    void Awake() {
        I = this;
    }

    public void PlayClip(AudioClip clip = null) {
        PlayClipAt(clip ?? notificationClip, GameManager.I.playerDependencies.transform.position);
    }

    public void PlayClipAt(AudioClip clip, Vector3 position) {
        var sourceGo = new GameObject("TempAudio") {
            transform = {
                position = position,
            },
        };

        var audioSource = sourceGo.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = 1f;
        audioSource.pitch = Random.Range(0.95f, 1.05f); // Random pitch for variation
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.minDistance = 3f; // Louder when close
        audioSource.maxDistance = 50f; // Still audible farther away
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.Play();

        Destroy(sourceGo, clip.length);
    }
}