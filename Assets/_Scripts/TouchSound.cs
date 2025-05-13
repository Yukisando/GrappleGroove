#region

using UnityEngine;

#endregion

public class TouchSound : MonoBehaviour
{
    [SerializeField] AudioClip clip;

    void OnCollisionEnter(Collision other) {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}