#region

using UnityEngine;

#endregion

[RequireComponent(typeof(Rigidbody))]
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