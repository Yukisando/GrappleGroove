#region

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

#endregion

[InfoBox("Makes the object it's attached to able to be grappled with")] [RequireComponent(typeof(ID))]
public class Hookable : MonoBehaviour
{
    public bool connectable = true;
    [ReadOnly] public bool isHooked;

    public UnityEvent onHooked = new UnityEvent();
    public UnityEvent onUnhooked = new UnityEvent();

    bool hookState;

    void Update() {
        switch (isHooked) {
            case true when hookState != isHooked:
                hookState = isHooked;
                onHooked?.Invoke();
                break;
            case false when hookState != isHooked:
                hookState = isHooked;
                onUnhooked?.Invoke();
                break;
        }
    }
}