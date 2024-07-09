
using DeTools.BasicMeshRenderer.Data;
using System.Collections.Generic;
using UnityEngine;

namespace DeTools.BasicMeshRenderer.Manager
{
    /// <summary>
    /// The manager of the basic mesh combiner gameobjects this wil handle simple functions around an gameobject.
    /// </summary>
    public class BasicGameObjectManager
    {
        /// <summary>
        /// Gets al children of an gameobject and returns an list of al objects.
        /// </summary>
        /// <param name="data">Data is the basicgameobjectdata that will be collected by the main tool.</param>
        /// <returns>returns an list of gameobjects.</returns>
        public static List<GameObject> GetObjectDependencies(List<BasicGameObjectData> data)
        {
            List<GameObject> returnList = new List<GameObject>();
            foreach (var datapeace in data)
            {
                if (!datapeace.IsEnabled || datapeace.Obj == null)
                {
                    continue;
                }

                Transform[] children = datapeace.Obj.GetComponentsInChildren<Transform>();

                foreach (var child in children)
                {
                    returnList.Add(child.gameObject);
                }
            }

            return returnList;
        }

        /// <summary>
        /// Gets the amount of selected materials.
        /// </summary>
        /// <param name="gameobjects">Gameobjects is an list of gameobject that it needs to check.</param>
        /// <returns>Returns an int of the lenght of holdList.</returns>
        public static int GetAmountOfSelectedMaterials(List<GameObject> gameobjects)
        {
            int returnInt = 0;
            foreach (var gameobject in gameobjects)
            {
                if (gameobject.GetComponent<MeshRenderer>())
                {
                    Material[] holdList = gameobject.GetComponent<MeshRenderer>().sharedMaterials;
                    returnInt = holdList.Length;
                }
            }
            return returnInt;
        }

        /// <summary>
        /// Gets the amount of submeshes of al given meshes.
        /// </summary>
        /// <param name="filterMeshes">An list of meshesh.</param>
        /// <returns>Returns an int value of al mesh subcounts.</returns>
        public static int GetAmountOfSubMeshes(List<Mesh> filterMeshes)
        {
            int returnInt = 0;

            for (int i = 0; i < filterMeshes.Count; i++)
            {
                returnInt += filterMeshes[i].subMeshCount;
            }

            return returnInt;
        }

        /// <summary>
        /// Filters al nonmeshrenders from the data.
        /// </summary>
        /// <param name="objectData">The given data that will be filtert.</param>
        /// <returns>Returns an list of gameobjects that has an meshfilter</returns>
        public static List<GameObject> GetMeshRenderers(List<BasicGameObjectData> objectData)
        {
            List<GameObject> objectsToCombine = GetObjectDependencies(objectData);
            List<GameObject> objectsToRemove = new List<GameObject>();

            foreach (GameObject sceneObject in objectsToCombine)
            {
                MeshFilter filter = sceneObject.GetComponent<MeshFilter>();


                if (filter == null)
                {
                    objectsToRemove.Add(sceneObject);
                }
            }

            foreach (var itemToRemove in objectsToRemove)
            {
                objectsToCombine.Remove(itemToRemove);
            }
            return objectsToCombine;
        }

        /// <summary>
        /// gets the drawcallCount of currentselected objects.
        /// </summary>
        /// <param name="_objectData">The data of current selected objects.</param>
        /// <returns>returns materials.count</returns>
        public static int CalculateDrawCalls(List<BasicGameObjectData> _objectData)
        {
            List<GameObject> submeshes = BasicGameObjectManager.GetMeshRenderers(_objectData);
            List<Material> materials = new List<Material>();
            foreach (var submesh in submeshes)
            {
                for (int i = 0; i < submesh.GetComponent<MeshRenderer>().sharedMaterials.Length; i++)
                {
                    if (!materials.Contains(submesh.GetComponent<MeshRenderer>().sharedMaterials[i]))
                    {
                        materials.Add(submesh.GetComponent<MeshRenderer>().sharedMaterials[i]);
                    }
                }
            }
            return materials.Count;
        }

        /// <summary>
        /// Gets al the verts from an gameobject.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>The vert count</returns>
        public static int GetVerts(GameObject obj)
        {
            MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
            SkinnedMeshRenderer[] skinnedMeshRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

            int vertCount = 0;
            foreach (MeshFilter meshFilter in meshFilters)
            {
                Mesh mesh = meshFilter.sharedMesh;
                vertCount = mesh.vertexCount;
            }

            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
            {
                Mesh mesh = skinnedMeshRenderer.sharedMesh;
                vertCount = mesh.vertexCount;
            }

            return vertCount;
        }
    }
}