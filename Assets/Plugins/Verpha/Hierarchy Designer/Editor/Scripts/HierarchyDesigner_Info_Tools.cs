#if UNITY_EDITOR
using System;

namespace Verpha.HierarchyDesigner
{
    public enum HierarchyDesigner_ToolCategory
    {
        Counting,
        Locking,
        Renaming,
        Selecting,
        Sorting
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class HierarchyDesigner_ToolAttribute : Attribute
    {
        public HierarchyDesigner_ToolCategory Category { get; private set; }

        public HierarchyDesigner_ToolAttribute(HierarchyDesigner_ToolCategory category)
        {
            Category = category;
        }
    }
}
#endif