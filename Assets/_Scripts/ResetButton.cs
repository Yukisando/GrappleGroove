#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class ResetButton : MonoBehaviour
{
    [SerializeField] List<GameObject> objetsToReset;
    [SerializeField] Transform button;
    [SerializeField] KeyCode activationKey = KeyCode.E;
    [SerializeField] public float interactionDistance = 3f;
    [SerializeField] AudioClip buttonPressSound;

    Camera playerCamera;
    AudioSource audioSource;

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

    void Awake() {
        playerCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

        // Store initial positions and rotations
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

    void Update() {
        CheckButtonInteraction();
    }

    void CheckButtonInteraction() {
        // Cast ray from camera center
        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))

            // Check if we hit this button
            if (hit.collider.gameObject == gameObject || hit.collider.transform == button)

                // If player presses the activation key
                if (Input.GetKeyDown(activationKey)) {
                    PressButton();
                    ResetObjects();
                }
    }

    void PressButton() {
        if (buttonPressSound) AudioSource.PlayClipAtPoint(buttonPressSound, transform.position);

        // Press animation
        button.LeanMoveLocalZ(-0.1f, 0.1f).setEaseInQuad().setOnComplete(ReleaseButton);

        // Play sound if available
        if (audioSource != null && buttonPressSound != null) audioSource.PlayOneShot(buttonPressSound);
    }

    void ReleaseButton() {
        // Release animation
        button.LeanMoveLocalZ(0f, 0.2f).setEaseOutQuad();
    }

    void ResetObjects() {
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