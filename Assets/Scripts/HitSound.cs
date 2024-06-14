//-------------------------------
//--- Prototype FPC
//--- Version 1.0
//--- © The Famous Mouse™
//-------------------------------

using UnityEngine;
using PrototypeFPC;

namespace PrototypeFPC
{
    public class HitSound : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip hitSound;

        void OnCollisionEnter(Collision col)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}
