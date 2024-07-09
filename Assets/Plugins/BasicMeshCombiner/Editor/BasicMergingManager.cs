using DeTools.BasicMeshRenderer.Data;
using System.Collections.Generic;
using UnityEngine;

namespace DeTools.BasicMeshRenderer.Merge
{
    /// <summary>
    /// An class that keeps track of al the merge ways.
    /// </summary>
    public class BasicMergingManager
    {
        /// <summary>
        /// The max verts for 16bits.
        /// </summary>
        private const int maxVerts = 50000;

        /// <summary>
        /// An way of combining meshes that usses one mesh per material.
        /// </summary>
        /// <param name="objectsToCombine">al of the objects that will be combined.</param>
        /// <returns>Returns an data type where the merged materials and also meshes can be gatherd from.</returns>
        public static BasicMergedObject DoCombineMeshes_OneMeshPerMaterial(List<GameObject> objectsToCombine)
        {
            bool lightMapSupport = false;
            List<MeshRenderer> listOfAllMeshesThatWasProcessedsInThisMerge = new List<MeshRenderer>();

            Dictionary<Material, List<BasicSubMeshToCombine>> subMeshesPerMaterial = new Dictionary<Material, List<BasicSubMeshToCombine>>();

            for (int i = 0; i < objectsToCombine.Count; i++)
            {
                GameObject thisGoWithMesh = objectsToCombine[i];
                for (int x = 0; x < thisGoWithMesh.GetComponent<MeshFilter>().sharedMesh.subMeshCount; x++)
                {
                    Material currentMaterial = thisGoWithMesh.GetComponent<MeshRenderer>().sharedMaterials[x];
                    if (subMeshesPerMaterial.ContainsKey(currentMaterial) == true)
                        subMeshesPerMaterial[currentMaterial].Add(new BasicSubMeshToCombine(thisGoWithMesh.gameObject.transform, thisGoWithMesh.GetComponent<MeshFilter>(), thisGoWithMesh.GetComponent<MeshRenderer>(), x));
                    if (subMeshesPerMaterial.ContainsKey(currentMaterial) == false)
                        subMeshesPerMaterial.Add(currentMaterial, new List<BasicSubMeshToCombine>() { new BasicSubMeshToCombine(thisGoWithMesh.gameObject.transform, thisGoWithMesh.GetComponent<MeshFilter>(), thisGoWithMesh.GetComponent<MeshRenderer>(), x) });
                }

                listOfAllMeshesThatWasProcessedsInThisMerge.Add(thisGoWithMesh.GetComponent<MeshRenderer>());
            }

            List<Mesh> combinedSubmehesPerMaterial = new List<Mesh>();
            foreach (var key in subMeshesPerMaterial)
            {
                List<BasicSubMeshToCombine> subMeshesOfCurrentMaterial = key.Value;

                List<CombineInstance> combineInstancesOfCurrentMaterial = new List<CombineInstance>();

                int totalVerticesCount = 0;

                for (int i = 0; i < subMeshesOfCurrentMaterial.Count; i++)
                {
                    CombineInstance combineInstance = new CombineInstance();
                    combineInstance.mesh = subMeshesOfCurrentMaterial[i].MeshFilter.sharedMesh;
                    combineInstance.subMeshIndex = subMeshesOfCurrentMaterial[i].SubMeshIndex;
                    combineInstance.transform = subMeshesOfCurrentMaterial[i].Transform.localToWorldMatrix;
                    combineInstancesOfCurrentMaterial.Add(combineInstance);
                    totalVerticesCount += combineInstance.mesh.vertexCount;
                }

                Mesh mesh = new Mesh();
                if (totalVerticesCount <= maxVerts)
                    mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
                if (totalVerticesCount > maxVerts)
                    mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                mesh.CombineMeshes(combineInstancesOfCurrentMaterial.ToArray(), true, true, lightMapSupport);

                combinedSubmehesPerMaterial.Add(mesh);
            }

            List<CombineInstance> finalCombineInstances = new List<CombineInstance>();
            int totalFinalVerticesCount = 0;
            foreach (Mesh mesh in combinedSubmehesPerMaterial)
            {
                CombineInstance combineInstanceOfThisSubMesh = new CombineInstance();
                combineInstanceOfThisSubMesh.mesh = mesh;
                combineInstanceOfThisSubMesh.subMeshIndex = 0;
                combineInstanceOfThisSubMesh.transform = Matrix4x4.identity;
                finalCombineInstances.Add(combineInstanceOfThisSubMesh);
                totalFinalVerticesCount += combineInstanceOfThisSubMesh.mesh.vertexCount;
            }

            Mesh finalMesh = new Mesh();
            if (totalFinalVerticesCount <= maxVerts)
                finalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
            if (totalFinalVerticesCount > maxVerts)
                finalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            finalMesh.CombineMeshes(finalCombineInstances.ToArray(), false);
            finalMesh.RecalculateBounds();


            List<Material> materialsForSubMeshes = new List<Material>();
            foreach (var key in subMeshesPerMaterial)
                materialsForSubMeshes.Add(key.Key);

            Material[] matList = materialsForSubMeshes.ToArray();

            BasicMergedObject returnObject = new BasicMergedObject(matList, finalMesh);

            return returnObject;
        }
    }
}