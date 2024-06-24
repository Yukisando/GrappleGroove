#region

using UnityEngine;

#endregion

public class Move : MonoBehaviour
{
    public Vector3 offset; // Offset to the target position
    public bool useLocalPosition = true;
    public bool loop = true;
    public float duration = 5f; // Duration of the loop
    [Range(0, 1)] public float startPosition; // Range from 0 (start) to 1 (destination)
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 initialPosition;
    LTDescr tween;

    public void Reset() {
        if (tween != null) {
            LeanTween.cancel(tween.uniqueId);
            MovePlatform();
        }
    }

    void Start() {
        Initialize();
        MovePlatform();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;

        var gizmoStartPoint = useLocalPosition ? transform.localPosition : transform.position;
        var gizmoEndPoint = useLocalPosition ? gizmoStartPoint + transform.TransformDirection(offset) : gizmoStartPoint + offset;

        Gizmos.DrawLine(gizmoStartPoint, gizmoEndPoint);

        var gizmoStartPosition = Vector3.Lerp(gizmoStartPoint, gizmoEndPoint, startPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(gizmoStartPosition, 0.2f);
    }

    void MovePlatform() {
        if (useLocalPosition) {
            tween = LeanTween.moveLocal(gameObject, initialPosition + transform.TransformDirection(offset), duration)
                .setEase(animationCurve)
                .setLoopPingPong()
                .setOnComplete(() => {
                    if (!loop) LeanTween.cancel(gameObject);
                });
        }
        else {
            LeanTween.move(gameObject, initialPosition + offset, duration)
                .setEase(animationCurve)
                .setLoopPingPong()
                .setOnComplete(() => {
                    if (!loop) LeanTween.cancel(gameObject);
                });
        }
    }

    void Initialize() {
        initialPosition = useLocalPosition ? transform.localPosition : transform.position;

        // Set the initial position based on the startPosition value
        if (useLocalPosition) {
            transform.localPosition = Vector3.Lerp(initialPosition, initialPosition + transform.TransformDirection(offset), startPosition);
        }
        else {
            transform.position = Vector3.Lerp(initialPosition, initialPosition + offset, startPosition);
        }
    }
}