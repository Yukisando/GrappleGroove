#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("Makes this object grabbable")] [RequireComponent(typeof(Rigidbody))] [RequireComponent(typeof(ID))]
public class Grabbable : MonoBehaviour
{
    Vector3 initialPosition;
    Quaternion initialRotation;

    Rigidbody rb;

    void Awake() {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void ResetObject() {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}