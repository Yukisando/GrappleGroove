#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("Moves an object from A to B (-1 = A->B->A forever, 0 = A->B, 1+ = ping-pong loops)")]
public class Move : MonoBehaviour
{
    public bool showGizmos = true;
    public bool useLocalPosition = true;
    public Vector3 destination = new Vector3(1, 0, 0);

    [SerializeField] int loopCount = -1;
    [SerializeField] float duration = 5f;
    [SerializeField] float delayBetweenLoops;
    [SerializeField] bool startMovingOnStart;
    [SerializeField] [Range(0, 1)] float startOffset;
    [SerializeField] AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 initialPosition;
    Vector3 offsetStartPosition;
    LTDescr tween;
    int loopsRemaining;
    bool isFirstRun = true;

    void Awake() {
        initialPosition = GetCurrentPosition();
    }

    void Start() {
        if (startMovingOnStart)
            StartMoving();
    }

    public void Pause() {
        if (tween != null && LeanTween.isTweening(tween.uniqueId)) LeanTween.pause(tween.uniqueId);
    }

    public void Resume() {
        if (tween != null && LeanTween.isTweening(tween.uniqueId)) LeanTween.resume(tween.uniqueId);
    }

    public void StartMoving() {
        Pause();
        isFirstRun = true;

        CacheOffsetStartPosition();
        ApplyStartOffset();

        if (loopCount == 0)
            MoveOnce();
        else {
            if (loopCount > 0)
                loopsRemaining = loopCount * 2;

            MovePingPong();
        }
    }

    public void ResetObject() {
        Pause();
        CacheOffsetStartPosition();
        ApplyStartOffset();

        if (startMovingOnStart)
            StartMoving();
    }

    void MoveOnce() {
        var end = GetTargetPosition();
        tween = CreateMoveTween(end, duration);
    }

    void MovePingPong() {
        var currentPos = GetCurrentPosition();
        var target = isFirstRun ? GetTargetPosition() : GetNextPingPongTarget(currentPos);

        float totalDist = Vector3.Distance(initialPosition, GetTargetPosition());
        float remainDist = Vector3.Distance(currentPos, target);
        float adjustedDuration = totalDist > 0 ? duration * (remainDist / totalDist) : duration;

        tween = CreateMoveTween(target, adjustedDuration).setOnComplete(() => {
            if (!isFirstRun && loopCount > 0)
                loopsRemaining--;

            if (loopCount == -1 || loopsRemaining > 0) {
                if (delayBetweenLoops > 0)
                    LeanTween.delayedCall(gameObject, delayBetweenLoops, MovePingPong);
                else
                    MovePingPong();
            }
        });

        isFirstRun = false;
    }

    LTDescr CreateMoveTween(Vector3 target, float time) {
        return useLocalPosition
            ? LeanTween.moveLocal(gameObject, target, time).setEase(animationCurve)
            : LeanTween.move(gameObject, target, time).setEase(animationCurve);
    }

    void CacheOffsetStartPosition() {
        var direction = useLocalPosition
            ? transform.TransformDirection(destination)
            : destination;

        offsetStartPosition = Vector3.Lerp(initialPosition, initialPosition + direction, startOffset);
    }

    void ApplyStartOffset() {
        if (useLocalPosition)
            transform.localPosition = offsetStartPosition;
        else
            transform.position = offsetStartPosition;
    }

    Vector3 GetCurrentPosition() {
        return useLocalPosition ? transform.localPosition : transform.position;
    }

    Vector3 GetTargetPosition() {
        return useLocalPosition
            ? initialPosition + transform.TransformDirection(destination)
            : initialPosition + destination;
    }

    Vector3 GetNextPingPongTarget(Vector3 current) {
        var end = GetTargetPosition();
        float distToStart = Vector3.Distance(current, initialPosition);
        float distToEnd = Vector3.Distance(current, end);

        return distToStart < distToEnd ? end : initialPosition;
    }

    void OnDrawGizmos() {
        if (!showGizmos) return;

        var start = useLocalPosition ? transform.localPosition : transform.position;
        var end = useLocalPosition
            ? start + transform.TransformDirection(destination)
            : start + destination;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(start, end);

        var offset = Vector3.Lerp(start, end, startOffset);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(offset, 0.2f);
    }
}