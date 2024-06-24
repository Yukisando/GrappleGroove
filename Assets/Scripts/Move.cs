#region

using UnityEngine;

#endregion

public class Move : MonoBehaviour
{
    public Vector3 destination; // Offset to the target position
    public bool useLocalPosition = true;
    public bool loop = true;
    public float duration = 5f; // Duration of the loop
    [Range(0, 1)] public float startOffset; // Range from 0 (start) to 1 (destination)
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 initialPosition;
    LTDescr tween;

    void Awake() {
        initialPosition = useLocalPosition ? transform.localPosition : transform.position;

        InitOffset();
    }

    public void Reset() {
        if (tween != null) {
            LeanTween.cancel(tween.uniqueId);
            InitOffset();
            StartMoving();
        }
    }

    void Start() {
        StartMoving();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;

        var gizmoStartPoint = useLocalPosition ? transform.localPosition : transform.position;
        var gizmoEndPoint = useLocalPosition ? gizmoStartPoint + transform.TransformDirection(destination) : gizmoStartPoint + destination;

        Gizmos.DrawLine(gizmoStartPoint, gizmoEndPoint);

        var gizmoStartPosition = Vector3.Lerp(gizmoStartPoint, gizmoEndPoint, startOffset);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(gizmoStartPosition, 0.2f);
    }

    void StartMoving() {
        if (useLocalPosition) {
            tween = LeanTween.moveLocal(gameObject, initialPosition + transform.TransformDirection(destination), duration)
                .setEase(animationCurve)
                .setLoopPingPong()
                .setOnComplete(() => {
                    if (!loop) LeanTween.cancel(gameObject);
                });
        }
        else {
            LeanTween.move(gameObject, initialPosition + destination, duration)
                .setEase(animationCurve)
                .setLoopPingPong()
                .setOnComplete(() => {
                    if (!loop) LeanTween.cancel(gameObject);
                });
        }
    }

    void InitOffset() {
        // Set the initial position based on the startOffset value
        if (useLocalPosition) {
            transform.localPosition = Vector3.Lerp(initialPosition, initialPosition + transform.TransformDirection(destination), startOffset);
        }
        else {
            transform.position = Vector3.Lerp(initialPosition, initialPosition + destination, startOffset);
        }
    }
}