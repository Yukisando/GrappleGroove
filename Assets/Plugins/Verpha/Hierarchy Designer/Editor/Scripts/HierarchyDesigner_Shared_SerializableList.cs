#if UNITY_EDITOR
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    [System.Serializable]
    public class HierarchyDesigner_SerializableList<T>
    {
        public List<T> items;

        public HierarchyDesigner_SerializableList(List<T> items)
        {
            this.items = items;
        }
    }
}
#endif