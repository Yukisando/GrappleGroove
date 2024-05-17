#region

using UnityEngine;

#endregion

public class ScratchManager : MonoBehaviour
{
    public GameObject player;
    public Canvas scratchpad;
    public KeyCode scratchpadKey = KeyCode.Tab;
    public AudioClip scratchpadSoundOnClip;
    public AudioClip scratchpadSoundOffClip;
    Camera cam;
    
    void Awake() {
        cam = Camera.main;
        scratchpad.enabled = false;
    }
    
    void Update() {
        if (Input.GetKeyDown(scratchpadKey)) {
            ToggleScratchpad();
        }
    }
    
    void ToggleScratchpad() {
        scratchpad.enabled = !scratchpad.enabled;
        player.SetActive(!scratchpad.enabled);
        AudioSource.PlayClipAtPoint(scratchpad.enabled ? scratchpadSoundOnClip : scratchpadSoundOffClip, cam.transform.position);
        Cursor.lockState = scratchpad.enabled ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = scratchpad.enabled;
        
        // Time.timeScale = scratchpad.enabled ? 0f : 1;
    }
}