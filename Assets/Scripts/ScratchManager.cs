#region

using System;
using System.Collections.Generic;
using PrototypeFPC;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

public class ScratchManager : MonoBehaviour
{
    public static ScratchManager I;
    public Canvas scratchpad;
    public Node nodePrefab;
    public Transform nodeListParent;
    public AudioClip scratchpadSoundOnClip;
    public AudioClip scratchpadSoundOffClip;
    
    [Title(" ")]
    [ReadOnly] public List<NodeData> nodes = new List<NodeData>();
    public KeyCode scratchpadKey = KeyCode.Tab;
    
    public Action<string> onNewNode;
    
    Movement playerMovement;
    
    void Awake() {
        I = this;
        scratchpad.enabled = false;
        playerMovement = FindAnyObjectByType<Movement>();
    }
    
    void Start() {
        LoadNodes();
    }
    
    void Update() {
        if (Input.GetKeyDown(scratchpadKey)) {
            ToggleScratchpad();
        }
    }
    
    void LoadNodes() {
        foreach (var nodeData in nodes) {
            AddNode(nodeData);
        }
    }
    
    void ToggleScratchpad() {
        scratchpad.enabled = !scratchpad.enabled;
        playerMovement.enabled = !scratchpad.enabled;
        AudioSource.PlayClipAtPoint(scratchpad.enabled ? scratchpadSoundOnClip : scratchpadSoundOffClip, transform.position);
        Cursor.lockState = scratchpad.enabled ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = scratchpad.enabled;
    }
    
    public void AddNode(NodeData nodeData) {
        if (nodeData.unique && nodes.Contains(nodeData)) return;
        nodes.Add(nodeData);
        var node = Instantiate(nodePrefab, nodeListParent);
        node.Populate(nodeData);
        Debug.Log($"Picked up {nodeData.id}!");
        onNewNode?.Invoke($"{nodeData.id} added to scratchpad!");
    }
}