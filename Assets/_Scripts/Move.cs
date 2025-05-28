#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("Moves an object from A to B (-1 = infinite ping-pong, 0 = A->B, 1 = A->B->A, etc.)")][RequireComponent(typeof(ID))]
public class Move : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool showGizmos = true;
    public bool useLocalPosition = true;
    public Vector3 destination = new Vector3(1, 0, 0);

    [SerializeField] int loopCount = -1;
    [SerializeField] float duration = 5f;
    [SerializeField] float delayBetweenLoops;
    [SerializeField] float returnSpeedMultiplier = 1f;
    [SerializeField] bool startMovingOnStart;
    [SerializeField] [Range(0, 1)] float startOffset;
    [SerializeField] AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 startPos;
    Vector3 endPos;
    LTDescr tween;
    int currentLoop;
    bool isPaused;
    bool isReturning; // To track if the current movement is a return trip

    void Awake() {
        startPos = GetCurrentPosition();
        endPos = useLocalPosition ? startPos + transform.TransformDirection(destination) : startPos + destination;
        SetStartOffset();
    }

    void Start() {
        if (startMovingOnStart)
            StartMoving();
    }

    public void StartMoving() {
        if (isPaused) {
            Resume();
            return;
        }

        StopMovement();

        currentLoop = loopCount == -1 ? -1 : loopCount * 2;
        isReturning = false;
        MoveToTarget(endPos);
    }

    public void Pause() {
        isPaused = true;
        if (tween != null && LeanTween.isTweening(tween.uniqueId))
            LeanTween.pause(tween.uniqueId);
    }

    public void Resume() {
        isPaused = false;
        if (tween != null && LeanTween.isTweening(tween.uniqueId))
            LeanTween.resume(tween.uniqueId);
    }

    public void StopMovement() {
        isPaused = false;
        if (tween != null && LeanTween.isTweening(tween.uniqueId))
            LeanTween.cancel(tween.uniqueId);
    }

    void MoveToTarget(Vector3 target) {
        float distanceRatio = Vector3.Distance(GetCurrentPosition(), target) / Vector3.Distance(startPos, endPos);
        float currentDuration = duration * distanceRatio;

        // Apply speed multiplier for return trips
        if (isReturning) currentDuration /= returnSpeedMultiplier;

        tween = (useLocalPosition
                ? LeanTween.moveLocal(gameObject, target, currentDuration)
                : LeanTween.move(gameObject, target, currentDuration))
            .setEase(animationCurve)
            .setOnComplete(OnMoveComplete);
    }

    void OnMoveComplete() {
        if (loopCount == 0) return;

        if (loopCount > 0 && --currentLoop <= 0) return;

        LeanTween.delayedCall(gameObject, delayBetweenLoops, () => {
            if (GetCurrentPosition() == endPos) {
                isReturning = true; // Now we are returning
                MoveToTarget(startPos);
            }
            else {
                isReturning = false; // Now we are going to the destination
                MoveToTarget(endPos);
            }
        });
    }

    void SetStartOffset() {
        var offset = Vector3.Lerp(startPos, endPos, startOffset);
        if (useLocalPosition)
            transform.localPosition = offset;
        else
            transform.position = offset;
    }

    Vector3 GetCurrentPosition() {
        return useLocalPosition ? transform.localPosition : transform.position;
    }

    public void ResetObject() {
        StopMovement();

        SetStartOffset();

        currentLoop = loopCount == -1 ? -1 : loopCount * 2;

        if (startMovingOnStart)
            StartMoving();
    }

    void OnDrawGizmos() {
        if (!showGizmos) return;

        var a = useLocalPosition ? transform.localPosition : transform.position;
        var b = useLocalPosition ? a + transform.TransformDirection(destination) : a + destination;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(a, b);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Vector3.Lerp(a, b, startOffset), 0.2f);
    }
}