#region

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

#endregion

public class GameButton : MonoBehaviour
{
    [FoldoutGroup("Assignements")]
    [SerializeField] Transform button;
    [FoldoutGroup("Assignements")]
    [SerializeField] public float interactionDistance = 3f;
    [FoldoutGroup("Assignements")]
    [SerializeField] AudioClip buttonPressSound;

    Camera playerCamera;
    int raycastMask;

    public UnityEvent onPress;
    Vector3 buttonInitPosition;

    void Awake() {
        playerCamera = GameManager.I.playerDependencies.cam;
        raycastMask = ~LayerMask.GetMask("IgnoreRaycast", "Player", "PlayerHitbox", "Plank"); // Fallback mask
        if (button) buttonInitPosition = button.localPosition;
    }

    void Update() {
        CheckButtonInteraction();
    }

    void CheckButtonInteraction() {
        // Use the same ray origin and direction as the Crosshair
        var ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out var hit, interactionDistance, raycastMask, QueryTriggerInteraction.Ignore))

            // Check if we hit this button
            if (hit.collider.gameObject == gameObject)

                // If player left-clicks
                if (Input.GetMouseButtonDown(0)) {
                    PressButton();
                    onPress?.Invoke();
                }
    }

    void PressButton() {
        if (buttonPressSound) AudioSource.PlayClipAtPoint(buttonPressSound, transform.position);

        // Press animation
        if (!button) return;
        button.localPosition = buttonInitPosition;
        button.LeanMoveLocalX(.3f, 0.1f).setEaseInQuad().setLoopPingPong(1);
    }
}