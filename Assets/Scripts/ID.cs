#region

using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

#endregion

[InfoBox("Needed to connect objects to other logic objects (Stack IDs with a comma)")] [RequireComponent(typeof(Collider))]
public class ID : MonoBehaviour
{
    public string id;

    public List<string> IDs => id?.Contains(",") == true
        ? id.Split(',').Select(s => s.Trim()).ToList()
        : new List<string> {
            id,
        };

    [ReadOnly] public bool spawned;

    TransformData transformData;
    public event Action<bool> onReset;

    void Awake() {
        if (id.IsNullOrWhitespace()) id = name;
    }

    void Start() {
        transformData = new TransformData(transform.position, transform.rotation, transform.localScale);
    }

    void OnValidate() {
        if (id.IsNullOrWhitespace()) id = name;
    }

    public void ResetObject() {
        if (GetComponent<Move>()) return;
        onReset?.Invoke(spawned);


        transform.LeanScale(Vector3.zero, .2f).setOnComplete(() => {
            if (spawned) {
                onReset?.Invoke(spawned);
                Destroy(gameObject);
                return;
            }

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
            transform.LeanScale(transformData.Scale, .2f);
        });
    }

    public void Despawn() {
        transform.LeanScale(Vector3.zero, .2f).setOnComplete(() => {
            onReset?.Invoke(spawned);
            Destroy(gameObject);
        });
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