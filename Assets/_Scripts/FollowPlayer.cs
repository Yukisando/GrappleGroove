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
    [SerializeField] float followDistance = 3f;
    [SerializeField] float bufferDistance = 1f; // Distance the player must move away before following resumes
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
        var hasReached = false;
        while (player) {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position + offset);
            if (distanceToPlayer < followDistance) {
                if (!hasReached) {
                    onPlayerReached?.Invoke();
                    hasReached = true;
                    following = false;
                }
            }
            else if (hasReached && distanceToPlayer > followDistance + bufferDistance) hasReached = false;

            if (!hasReached) {
                following = true;
                transform.LookAt(player.position + offset);
                transform.position = Vector3.MoveTowards(transform.position, player.position + offset, followSpeed * Time.deltaTime);
            }

            yield return null;
        }
        following = false;
    }
}