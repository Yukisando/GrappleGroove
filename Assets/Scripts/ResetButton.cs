#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class ResetButton : GameButton
{
    [SerializeField] List<GameObject> objetsToReset;

    // Store initial transforms
    readonly Dictionary<GameObject, TransformData> initialTransforms = new Dictionary<GameObject, TransformData>();

    // Class to store position and rotation
    class TransformData
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;

        public TransformData(Vector3 position, Quaternion rotation) {
            Position = position;
            Rotation = rotation;
        }
    }

    void Start() {
        StoreInitialTransforms();
    }

    void StoreInitialTransforms() {
        foreach (var obj in objetsToReset) {
            if (obj != null)
                initialTransforms[obj] = new TransformData(
                    obj.transform.position,
                    obj.transform.rotation
                );
        }
    }

    public void ResetObjects() {
        foreach (var obj in objetsToReset) {
            if (obj != null && initialTransforms.ContainsKey(obj)) {
                // Reset position and rotation
                var rb = obj.GetComponent<Rigidbody>();
                if (rb != null) {
                    // Reset physics if object has Rigidbody
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.MovePosition(initialTransforms[obj].Position);
                    rb.MoveRotation(initialTransforms[obj].Rotation);
                }
                else {
                    // Direct transform reset if no Rigidbody
                    obj.transform.position = initialTransforms[obj].Position;
                    obj.transform.rotation = initialTransforms[obj].Rotation;
                }
            }
        }
    }
}