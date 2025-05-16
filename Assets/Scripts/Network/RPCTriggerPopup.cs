#region

using Unity.Netcode;

#endregion

public class RPCTriggerPopup : NetworkBehaviour
{
    [Rpc(SendTo.Everyone)]
    public void PopupMessageToEveryoneRpc(string message) {
        GameManager.I.PopupMessage(message);
    }
}