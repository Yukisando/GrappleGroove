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
        var wasInRange = false;
        while (player) {
            bool inRange = Vector3.Distance(transform.position, player.position + offset) < followDistance;
            if (inRange && !wasInRange) {
                onPlayerReached?.Invoke();
                wasInRange = true;
            }
            else if (!inRange) {
                following = true;
                transform.LookAt(player.position + offset);
                transform.position = Vector3.MoveTowards(transform.position, player.position + offset, followSpeed * Time.deltaTime);
                wasInRange = false;
            }
            yield return null;
        }
    }
}