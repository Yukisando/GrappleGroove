using UnityEngine;

namespace DeTools.BasicMeshRenderer.Data
{
    /// <summary>
    /// BasicSubMeshToCombine data type.
    /// </summary>
    public class BasicSubMeshToCombine
    {
        /// <summary>
        /// The private transform variable
        /// </summary>
        private Transform transform;

        /// <summary>
        /// The private meshFilter variable
        /// </summary>
        private MeshFilter meshFilter;

        /// <summary>
        /// The private meshRenderer variable
        /// </summary>
        private MeshRenderer meshRenderer;

        /// <summary>
        /// The private subMeshIndex variable
        /// </summary>
        private int subMeshIndex;


        /// <summary>
        /// The public Transform variable
        /// </summary>
        public Transform Transform => transform;

        /// <summary>
        /// The public MeshFilter variable
        /// </summary>
        public MeshFilter MeshFilter => meshFilter;

        /// <summary>
        /// The public MeshRenderer variable
        /// </summary>
        public MeshRenderer MeshRenderer => meshRenderer;

        /// <summary>
        /// The public SubMeshIndex variable
        /// </summary>
        public int SubMeshIndex => subMeshIndex;

        /// <summary>
        /// The Constructor of the BasicSubMeshToCombine class
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="meshFilter"></param>
        /// <param name="meshRenderer"></param>
        /// <param name="subMeshIndex"></param>
        public BasicSubMeshToCombine(Transform transform, MeshFilter meshFilter, MeshRenderer meshRenderer, int subMeshIndex)
        {
            this.transform = transform;
            this.meshFilter = meshFilter;
            this.meshRenderer = meshRenderer;
            this.subMeshIndex = subMeshIndex;
        }
    }
}