#region

using UnityEngine;

#endregion

public class Move : MonoBehaviour
{
    public Vector3 localMove = Vector3.forward * 5;
    public float speed = 5f;
    Vector3 startPos;

    void Start() {
        startPos = transform.position;
        transform.LeanMove(startPos + localMove, 10 / speed).setEaseInOutSine().setLoopPingPong();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + localMove, 0.5f);
    }
}