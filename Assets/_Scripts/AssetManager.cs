#region

using System;
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
    public AudioClip resetSound;
    public AudioClip checkpointSound;
    public AudioClip startTimerSound;
    public AudioClip stopTimerSound;
    public AudioClip timerTickSound;
    public AudioClip objectSpawn;

    void Awake() {
        I = this;
    }

    public void PlayClip(AudioClip _clip = null) {
        PlayClipAt(new ClipPoint {
            clip = _clip ?? notificationClip,
            transform = GameManager.I.playerDependencies.transform,
        });
    }

    public void PlayClipAt(ClipPoint clipPoint = null) {
        clipPoint ??= new ClipPoint {
            clip = notificationClip,
            transform = GameManager.I.playerDependencies.transform,
        };

        var sourceGo = new GameObject("TempAudio") {
            transform = {
                position = clipPoint.transform.position,
            },
        };

        var audioSource = sourceGo.AddComponent<AudioSource>();
        audioSource.clip = clipPoint.clip;
        audioSource.volume = 1f;
        audioSource.pitch = Random.Range(0.95f, 1.05f); // Random pitch for variation
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.minDistance = 3f; // Louder when close
        audioSource.maxDistance = 50f; // Still audible farther away
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.Play();

        Destroy(sourceGo, clipPoint.clip.length);
    }
}

[Serializable]
public class ClipPoint
{
    public AudioClip clip;
    public Transform transform;
}