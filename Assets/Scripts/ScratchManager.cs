#region

using System.Collections.Generic;
using PrototypeFPC;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

public class ScratchManager : MonoBehaviour
{
    [SerializeField] PlayerDependencies playerDependencies;
    [SerializeField] Canvas scratchpad;
    [SerializeField] Node nodePrefab;
    [SerializeField] Transform nodeListParent;
    
    [Header("Audio")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip scratchpadSoundOnClip;
    [SerializeField] AudioClip scratchpadSoundOffClip;
    
    [Space(10)] [ReadOnly] public List<NodeData> nodes = new List<NodeData>();
    public KeyCode scratchpadKey = KeyCode.Tab;
    
    Movement playerMovement;
    
    void Awake() {
        source = GetComponent<AudioSource>();
        source.ignoreListenerPause = true;
        
        scratchpad.enabled = false;
        playerMovement = playerDependencies.GetComponent<Movement>();
    }
    
    void Start() {
        LoadNodes();
    }
    
    void Update() {
        if (Input.GetKeyDown(scratchpadKey)) ToggleScratchpad();
    }
    
    void LoadNodes() {
        foreach (var nodeData in nodes) {
            AddNode(nodeData);
        }
    }
    
    void ToggleScratchpad() {
        scratchpad.enabled = !scratchpad.enabled;
        playerMovement.enabled = !scratchpad.enabled;
        source.PlayOneShot(scratchpad.enabled ? scratchpadSoundOnClip : scratchpadSoundOffClip);
        Cursor.lockState = scratchpad.enabled ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = scratchpad.enabled;
    }
    
    public void AddNode(NodeData _nodeData) {
        if (_nodeData.unique && nodes.Contains(_nodeData)) return;
        nodes.Add(_nodeData);
        var node = Instantiate(nodePrefab, nodeListParent);
        node.Populate(_nodeData);
        Debug.Log($"Picked up {_nodeData.id}!");
    }
}