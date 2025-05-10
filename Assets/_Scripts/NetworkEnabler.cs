#region

using Unity.Netcode;
using UnityEngine;

#endregion

public class NetworkEnabler : NetworkBehaviour
{
    [SerializeField] bool ifOwner = true;
    [SerializeField] Behaviour component;

    void Start() {
        component.enabled = IsOwner == ifOwner;
    }
}