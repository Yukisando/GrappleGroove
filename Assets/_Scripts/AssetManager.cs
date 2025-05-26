#region

using UnityEngine;

#endregion

public class AssetManager : MonoBehaviour
{
    public static AssetManager I;

    public AudioClip notificationClip;
    public AudioClip spawnClip;
    public AudioClip defaultRaceClip;
    public AudioClip resetSound;
    public AudioClip checkpointSound;
    public AudioClip startTimerSound;
    public AudioClip stopTimerSound;
    public AudioClip timerTickSound;
    public AudioClip bumpSound;
    public AudioClip objectSpawn;

    void Awake() {
        I = this;
    }

    public void PlayClip(AudioClip clip = null) {
        if (clip == null) clip = notificationClip;

        var sourceGo = new GameObject("TempAudio") {
            transform = {
                position = GameManager.I.playerDependencies.transform.position,
            },
        };

        var audioSource = sourceGo.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = 1f;
        audioSource.pitch = Random.Range(0.9f, 1.1f); // Random pitch for variation
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.minDistance = 3f; // Louder when close
        audioSource.maxDistance = 50f; // Still audible farther away
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.Play();

        Destroy(sourceGo, clip.length);
    }
}