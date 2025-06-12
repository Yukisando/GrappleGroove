#region

using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

#endregion

[InfoBox("Needed to connect objects to other logic objects")] [RequireComponent(typeof(Collider))]
public class ID : MonoBehaviour
{
    public string id;

    [ReadOnly] public bool spawned;

    TransformData transformData;
    public event Action<bool> onReset;

    void Awake() {
        transformData = new TransformData(transform.position, transform.rotation, transform.localScale);
    }

    void OnValidate() {
        if (id.IsNullOrWhitespace()) id = name;
    }

    public void Despawn() {
        // Cancel any active tweens on this object
        LeanTween.cancel(gameObject);

        transform.LeanScale(Vector3.zero, .2f).setOnComplete(() => {
            onReset?.Invoke(spawned);
            gameObject.SetActive(false);
        });
    }

    public void ResetObject() {
        if (transformData == null) return;

        // Cancel any active tweens on this object
        LeanTween.cancel(gameObject);
        gameObject.SetActive(!SceneObjectTracker.WasOriginallyInactive(gameObject));

        if (TryGetComponent<Move>(out var moveComponent)) moveComponent.ResetObject(); // Special case for Move component
        onReset?.Invoke(spawned);

        if (spawned) {
            Destroy(gameObject);
            return;
        }

        // Reset position, rotation, and scale
        var rb = GetComponent<Rigidbody>();
        if (rb) {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.MovePosition(transformData.Position);
            rb.MoveRotation(transformData.Rotation);
        }
        else {
            transform.position = transformData.Position;
            transform.rotation = transformData.Rotation;
        }

        transform.localScale = transformData.Scale; // Explicitly restore scale
    }

    class TransformData
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        public readonly Vector3 Scale;

        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale) {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}