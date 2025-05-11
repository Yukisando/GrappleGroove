#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace PrototypeFPC
{
    public class Plank : MonoBehaviour
    {
        [SerializeField] [ReadOnly] int ropeIndex = -1;

        GrapplingHook grapplingHook;

        public void Initialize(GrapplingHook hook, int index) {
            grapplingHook = hook;
            ropeIndex = index;
        }

        public void Cut() {
            if (ropeIndex >= 0 && grapplingHook != null) grapplingHook.DestroyRope(ropeIndex);
        }
    }
}