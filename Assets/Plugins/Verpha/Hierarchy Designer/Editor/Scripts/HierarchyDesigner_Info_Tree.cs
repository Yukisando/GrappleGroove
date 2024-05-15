#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    [System.Serializable]
    public class HierarchyDesigner_Info_Tree
    {
        #region Properties
        private Color color = Color.white;
        #endregion

        #region Accessors
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        #endregion

        public HierarchyDesigner_Info_Tree(Color color)
        {
            this.color = color;
        }
    }
}
#endif