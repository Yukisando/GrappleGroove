#region

using UnityEngine;

#endregion

[RequireComponent(typeof(AudioSource))]
public class Move : MonoBehaviour
{
    [SerializeField] Vector3 destination; // Offset to the target position
    [SerializeField] bool useLocalPosition = true;
    [SerializeField] bool loop = true;
    [SerializeField] float duration = 5f; // Duration of the loop
    [SerializeField] [Range(0, 1)] float startOffset; // Range from 0 (start) to 1 (destination)
    [SerializeField] AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 initialPosition;
    bool isFirstLoop = true;
    Vector3 offsetStartPosition;
    AudioSource source;
    LTDescr tween;

    void Awake() {
        SetupAudio();
        initialPosition = useLocalPosition ? transform.localPosition : transform.position;
        offsetStartPosition = Vector3.Lerp(initialPosition, initialPosition + (useLocalPosition ? transform.TransformDirection(destination) : destination), startOffset);
    }

    public void Reset() {
        if (tween != null) {
            LeanTween.cancel(tween.uniqueId);
            isFirstLoop = true;
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

    void SetupAudio() {
        source = GetComponent<AudioSource>();
        source.loop = true;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.clip = GameManager.I.platformSound;
    }

    void StartMoving() {
        if (isFirstLoop) {
            ApplyStartOffset();
            isFirstLoop = false;
            StartTweenFromOffset();
        }
        else {
            StartTween();
        }
    }

    void StartTween() {
        var endPosition = useLocalPosition ? initialPosition + transform.TransformDirection(destination) : initialPosition + destination;

        if (useLocalPosition) {
            tween = LeanTween.moveLocal(gameObject, endPosition, duration)
                .setEase(animationCurve)
                .setLoopPingPong()
                .setOnComplete(OnTweenComplete);
        }
        else {
            tween = LeanTween.move(gameObject, endPosition, duration)
                .setEase(animationCurve)
                .setLoopPingPong()
                .setOnComplete(OnTweenComplete);
        }
    }

    void StartTweenFromOffset() {
        var endPosition = useLocalPosition ? initialPosition + transform.TransformDirection(destination) : initialPosition + destination;
        float remainingDuration = duration * (1 - startOffset);

        LeanTween.move(gameObject, endPosition, remainingDuration)
            .setEase(animationCurve)
            .setOnComplete(() => {
                // Start the ping-pong tween after reaching the destination
                if (useLocalPosition) {
                    tween = LeanTween.moveLocal(gameObject, initialPosition, duration)
                        .setEase(animationCurve)
                        .setLoopPingPong()
                        .setOnComplete(OnTweenComplete);
                }
                else {
                    tween = LeanTween.move(gameObject, initialPosition, duration)
                        .setEase(animationCurve)
                        .setLoopPingPong()
                        .setOnComplete(OnTweenComplete);
                }
            });
    }

    void OnTweenComplete() {
        if (!loop) {
            LeanTween.cancel(gameObject);
        }
    }

    void ApplyStartOffset() {
        if (useLocalPosition) {
            transform.localPosition = offsetStartPosition;
        }
        else {
            transform.position = offsetStartPosition;
        }
    }
}