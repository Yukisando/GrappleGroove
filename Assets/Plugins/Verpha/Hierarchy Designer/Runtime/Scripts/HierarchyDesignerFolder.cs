namespace Verpha.HierarchyDesigner
{
    using UnityEngine;

    public class HierarchyDesignerFolder : MonoBehaviour
    {
        [SerializeField] private bool flattenFolder = true;
        public bool ShouldFlatten => flattenFolder;

        private void Start()
        {
            FlattenFolder();
        }

        private void FlattenFolder()
        {
            if (flattenFolder)
            {
                RecursivelyFlatten(transform);
            }
        }

        private void RecursivelyFlatten(Transform folderTransform)
        {
            for (int i = folderTransform.childCount - 1; i >= 0; i--)
            {
                Transform child = folderTransform.GetChild(i);
                HierarchyDesignerFolder childFolder = child.GetComponent<HierarchyDesignerFolder>();

                if (childFolder != null)
                {
                    if (childFolder.ShouldFlatten)
                    {
                        childFolder.RecursivelyFlatten(child);
                        Destroy(child.gameObject);
                    }
                    else
                    {
                        child.SetParent(null);
                    }
                }
                else
                {
                    child.SetParent(null);
                }
            }

            Destroy(gameObject);
        }
    }
}