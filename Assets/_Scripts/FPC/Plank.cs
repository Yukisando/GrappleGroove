using UnityEngine;
using System;

namespace PrototypeFPC
{
    public class Plank : MonoBehaviour
    {
        [SerializeField] private int ropeIndex = -1;
        [SerializeField] private GrapplingHook grapplingHook;
        
        public int RopeIndex => ropeIndex;
        
        public void Initialize(GrapplingHook hook, int index)
        {
            grapplingHook = hook;
            ropeIndex = index;
        }
        
        public void Cut()
        {
            if (ropeIndex >= 0 && grapplingHook != null)
            {
                grapplingHook.DestroyRope(ropeIndex);
            }
        }
    }
}