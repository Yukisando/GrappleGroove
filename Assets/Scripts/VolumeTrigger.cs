#region

using UnityEngine;
using UnityEngine.Events;

#endregion

public class VolumeTrigger : MonoBehaviour
{
    public UnityEvent onEnter;
    public string tagMask = "Player";
    public bool destroyObjectPostTrigger;

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    void OnTriggerEnter(Collider _other) {
        if (_other.CompareTag(tagMask)) {
            if (destroyObjectPostTrigger) Destroy(gameObject);
            onEnter?.Invoke();
        }
    }
}