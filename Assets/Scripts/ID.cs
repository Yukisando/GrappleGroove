#region

using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

#endregion

[InfoBox("Needed to connect objects to other logic objects")] [RequireComponent(typeof(Collider))]
public class ID : MonoBehaviour
{
    public string id;

    TransformData transformData;

    void Awake() {
        if (id.IsNullOrWhitespace()) id = name;
    }

    void Start() {
        transformData = new TransformData(transform.position, transform.rotation);
    }

    void OnValidate() {
        if (id.IsNullOrWhitespace()) id = name;
    }

    public void ResetObject() {
        // Reset position and rotation
        var rb = GetComponent<Rigidbody>();
        if (rb) {
            // Reset physics if object has Rigidbody
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.MovePosition(transformData.Position);
            rb.MoveRotation(transformData.Rotation);
        }
        else {
            // Direct transform reset if no Rigidbody
            transform.position = transformData.Position;
            transform.rotation = transformData.Rotation;
        }
    }

    public void Despawn() {
        transform.LeanScale(Vector3.zero, .2f).setOnComplete(() => Destroy(gameObject));
    }

    class TransformData
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;

        public TransformData(Vector3 position, Quaternion rotation) {
            Position = position;
            Rotation = rotation;
        }
    }
}