#region

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#endregion

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] bool followOnStart = true;
    [SerializeField] float followSpeed = 2f;
    [SerializeField] Vector3 offset;
    [SerializeField] UnityEvent onPlayerReached;

    Transform player;
    bool following;

    void Awake() {
        player = GameManager.I.playerDependencies.transform;
    }

    void Start() {
        if (followOnStart) StartFollowing();
    }

    public void StartFollowing() {
        if (following) return; // Already following
        StartCoroutine(FollowPlayer_());
    }

    IEnumerator FollowPlayer_() {
        following = true;
        while (Vector3.Distance(transform.position, player.position + offset) > 1f) {
            transform.LookAt(player.position + offset);
            transform.position = Vector3.MoveTowards(transform.position, player.position + offset, followSpeed * Time.deltaTime);
            yield return null;
        }
        onPlayerReached?.Invoke();
        following = false;
    }
}