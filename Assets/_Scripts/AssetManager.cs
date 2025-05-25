#region

using UnityEngine;

#endregion

public class AssetManager : MonoBehaviour
{
    public static AssetManager I;

    public AudioClip spawnClip;
    public AudioClip defaultRaceClip;
    public AudioClip resetSound;
    public AudioClip checkpointSound;
    public AudioClip startTimerSound;
    public AudioClip stopTimerSound;
    public AudioClip timerTickSound;
    public AudioClip splashSound;
    public AudioClip bumpSound;

    void Awake() {
        I = this;
    }

    public void PlayClip(AudioClip clip) {
        AudioSource.PlayClipAtPoint(clip, GameManager.I.playerDependencies.transform.position, 1.0f);
    }
}