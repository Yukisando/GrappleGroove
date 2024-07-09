using UnityEngine;

namespace DeTools.BasicMeshRenderer.Data
{
    /// <summary>
    /// Data of merged object.
    /// </summary>
    public class BasicMergedObject
    {
        /// <summary>
        /// Contructor of BasicMergedObject.
        /// </summary>
        /// <param name="_materialList">Material list of merged mesh.</param>
        /// <param name="_mesh">Merged mesh.</param>
        public BasicMergedObject(Material[] _materialList, Mesh _mesh)
        {
            materialList = _materialList;
            mesh = _mesh;
        }

        /// <summary>
        /// Private merged list of the materials.
        /// </summary>
        private Material[] materialList;


        /// <summary>
        /// Private mesh that will be the merged mesh.
        /// </summary>
        private Mesh mesh;

        /// <summary>
        /// public merged list of the materials.
        /// </summary>
        public Material[] MaterialList => materialList;

        /// <summary>
        /// public mesh that will be the merged mesh.
        /// </summary>
        public Mesh Mesh => mesh;
    }
}