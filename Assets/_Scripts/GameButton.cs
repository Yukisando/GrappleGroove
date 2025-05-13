#region

using UnityEngine;
using UnityEngine.Events;

#endregion

public class GameButton : MonoBehaviour
{
    [SerializeField] Transform button;
    [SerializeField] KeyCode activationKey = KeyCode.E;
    [SerializeField] public float interactionDistance = 3f;
    [SerializeField] AudioClip buttonPressSound;

    Camera playerCamera;

    public UnityEvent onPress;

    void Awake() {
        playerCamera = Camera.main;
    }

    void Update() {
        CheckButtonInteraction();
    }

    void CheckButtonInteraction() {
        // Cast ray from camera center
        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out var hit, interactionDistance))

            // Check if we hit this button
            if (hit.collider.gameObject == gameObject || hit.collider.transform == button)

                // If player presses the activation key
                if (Input.GetKeyDown(activationKey)) {
                    PressButton();
                    onPress?.Invoke();
                }
    }

    void PressButton() {
        if (buttonPressSound) AudioSource.PlayClipAtPoint(buttonPressSound, transform.position);

        // Press animation
        button.LeanMoveLocalX(.3f, 0.1f).setEaseInQuad().setLoopPingPong(1);

        // Play sound if available
        if (buttonPressSound) AudioSource.PlayClipAtPoint(buttonPressSound, button.transform.position);
    }
}