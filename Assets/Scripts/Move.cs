#region

using UnityEngine;

#endregion

[RequireComponent(typeof(AudioSource))]
public class Move : MonoBehaviour
{
    [SerializeField] Vector3 localMove = Vector3.one;
    [SerializeField] float duration = 5f;
    [SerializeField] AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Using AnimationCurve instead of LeanTweenType
    [SerializeField] [Range(0f, 1f)] float startOffset = .1f;
    [Header("Audio")]
    public float maxVolume = 1f;
    bool isReversing;

    float maxSpeed;
    Vector3 previousPosition;
    AudioSource source;
    Vector3 startPos;
    float timeElapsed;

    void Awake() {
        source = GetComponent<AudioSource>();
        source.loop = true;
        source.spatialBlend = 1f;
        source.playOnAwake = true;
        source.volume = 0f;
        source.maxDistance = 120f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.clip = GameManager.I.platformSound;
    }

    public void Reset() { }

    void Start() {
        startPos = transform.position;

        // Set initial position based on startOffset
        transform.position = startPos + localMove * startOffset;
        previousPosition = transform.position;

        // Calculate the maximum speed
        maxSpeed = (localMove / duration).magnitude;

        // Initialize timeElapsed based on startOffset
        timeElapsed = duration * startOffset;
    }

    void Update() {
        timeElapsed += Time.deltaTime;

        // Determine current phase of movement
        float cycleTime = timeElapsed % (2 * duration); // Ping-pong duration is twice the single duration
        if (cycleTime >= duration) {
            isReversing = true;
            cycleTime -= duration;
        }
        else {
            isReversing = false;
        }

        float normalizedTime = cycleTime / duration;
        float easedTime = easeCurve.Evaluate(normalizedTime);

        // Ping-pong logic
        if (isReversing) {
            transform.position = Vector3.Lerp(startPos + localMove, startPos, easedTime);
        }
        else {
            transform.position = Vector3.Lerp(startPos, startPos + localMove, easedTime);
        }

        // Calculate the current speed
        float speed = (transform.position - previousPosition).magnitude / Time.deltaTime;
        previousPosition = transform.position;

        // Adjust pitch based on speed
        source.pitch = Mathf.Clamp(speed / maxSpeed, 0f, 1f);

        // Adjust volume based on speed
        source.volume = Mathf.Clamp(speed / maxSpeed, 0.3f, maxVolume);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position + localMove, 1f);
        Gizmos.DrawLine(transform.position, transform.position + localMove);
        Gizmos.DrawWireSphere(transform.position + localMove * startOffset, 1f);
    }
}