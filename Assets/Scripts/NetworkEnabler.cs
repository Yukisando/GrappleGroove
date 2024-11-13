#region

using Unity.Netcode;
using UnityEngine;

#endregion

public class NetworkEnabler : NetworkBehaviour
{
    [SerializeField] bool ifOwner = true;

    void Start() {
        gameObject.SetActive(IsOwner == ifOwner);
    }
}