#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("Moves an object from A to B (-1 = A->B->A for ever, 0 = A->B, 1+ = loop cycles")]
public class Move : MonoBehaviour
{
    public bool showGizmos = true;
    public bool useLocalPosition = true;
    public Vector3 destination = new Vector3(1, 0, 0); // Offset to the target position
    [SerializeField] int loopCount = -1; // -1 = infinite loop, 0 = A to B once, 1+ = ping-pong loops
    [SerializeField] float duration = 5f;
    [SerializeField] float delayBetweenLoops;
    [SerializeField] bool startMovingOnStart;
    [SerializeField] [Range(0, 1)] float startOffset;
    [SerializeField] AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 initialPosition;
    Vector3 offsetStartPosition;
    AudioSource source;
    LTDescr tween;
    int loopsRemaining;
    bool isFirstRun = true;

    void Awake() {
        initialPosition = useLocalPosition ? transform.localPosition : transform.position;
        offsetStartPosition = Vector3.Lerp(
            initialPosition,
            initialPosition + (useLocalPosition ? transform.TransformDirection(destination) : destination),
            startOffset
        );
    }

    void Start() {
        if (startMovingOnStart)
            StartMoving();
    }

    public void StartMoving() {
        if (loopCount > 0)
            loopsRemaining = loopCount * 2; // A→B→A = 2 steps per loop

        ApplyStartOffset();

        if (loopCount == 0)
            MoveOnce();
        else
            MovePingPong();
    }

    void MoveOnce() {
        var endPos = useLocalPosition
            ? initialPosition + transform.TransformDirection(destination)
            : initialPosition + destination;

        var move = useLocalPosition
            ? LeanTween.moveLocal(gameObject, endPos, duration)
            : LeanTween.move(gameObject, endPos, duration);

        tween = move.setEase(animationCurve);
    }

    void MovePingPong() {
        var targetPos = isFirstRun
            ? useLocalPosition ? initialPosition + transform.TransformDirection(destination) : initialPosition + destination
            : GetNextPingPongTarget();

        float dur = isFirstRun ? duration * (1 - startOffset) : duration;

        var move = useLocalPosition
            ? LeanTween.moveLocal(gameObject, targetPos, dur)
            : LeanTween.move(gameObject, targetPos, dur);

        tween = move.setEase(animationCurve).setOnComplete(OnPingPongStepComplete);

        isFirstRun = false;
    }

    void OnPingPongStepComplete() {
        if (loopCount == -1)
            MovePingPong(); // infinite
        else {
            loopsRemaining--;
            if (loopsRemaining > 0)
                MovePingPong();
        }
    }

    Vector3 GetNextPingPongTarget() {
        var end = useLocalPosition ? initialPosition + transform.TransformDirection(destination) : initialPosition + destination;
        var current = useLocalPosition ? transform.localPosition : transform.position;

        float distToStart = Vector3.Distance(current, initialPosition);
        float distToEnd = Vector3.Distance(current, end);

        return distToStart < distToEnd ? end : initialPosition;
    }

    void ApplyStartOffset() {
        if (useLocalPosition)
            transform.localPosition = offsetStartPosition;
        else
            transform.position = offsetStartPosition;
    }

    public void ResetObject() {
        if (tween != null)
            LeanTween.cancel(tween.uniqueId);

        // Reset position to initial position
        if (useLocalPosition)
            transform.localPosition = initialPosition;
        else
            transform.position = initialPosition;

        isFirstRun = true;
        if (startMovingOnStart)
            StartMoving();
    }

    void OnDrawGizmos() {
        if (!showGizmos) return;

        Gizmos.color = Color.green;

        var startPoint = useLocalPosition ? transform.localPosition : transform.position;
        var endPoint = useLocalPosition
            ? startPoint + transform.TransformDirection(destination)
            : startPoint + destination;

        Gizmos.DrawLine(startPoint, endPoint);

        var offset = Vector3.Lerp(startPoint, endPoint, startOffset);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(offset, 0.2f);
    }
}