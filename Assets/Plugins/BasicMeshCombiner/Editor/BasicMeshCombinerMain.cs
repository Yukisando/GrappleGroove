using DeTools.BasicMeshRenderer.Data;
using DeTools.BasicMeshRenderer.Manager;
using DeTools.BasicMeshRenderer.Merge;
using DeTools.BasicMeshRenderer.Window;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DeTools.BasicMeshRenderer.Panel
{
    public class BasicMeshCombinerMain
    {
        /// <summary>
        /// Random scroll position for boxes.
        /// </summary>
        private static Vector2 scrollPosForRand = Vector2.zero;

        /// <summary>
        /// Scroll position for the item box.
        /// </summary>
        private static Vector2 scrollPosForItems = Vector2.zero;

        /// <summary>
        /// All selected gameobjects.
        /// </summary>
        private static GameObject[] selectedObjects;

        /// <summary>
        /// All vert count of all selected gameobjects.
        /// </summary>
        private static float alVertsAmount = 0;

        /// <summary>
        /// The approximatly drawcall count.
        /// </summary>
        private static int approximatelyDrawCalls = 0;

        /// <summary>
        /// al selected objects for selected gameobjects.
        /// </summary>
        private static List<BasicGameObjectData> objectData = new List<BasicGameObjectData>();

        /// <summary>
        /// Max gameobject charachter name.
        /// </summary>
        private const int maxCharachters = 15;
        
        /// <summary>
        /// The name of the advanced mesh combiner link.
        /// </summary>
        private const string advancedMeshCombinerLink = "https://assetstore.unity.com/packages/slug/252290";
        
        /// <summary>
        /// Filter for componants.
        /// </summary>
        private const string scriptFilter = "Callbacks";

        /// <summary>
        /// Filter for colliders.
        /// </summary>
        private const string colliderFilter = "Collider";
        

        /// <summary>
        /// Draws al gui.
        /// </summary>
        public static void DrawGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            PreferencesBox();
            Mergebox();
            GUILayout.EndVertical();
            ItemBox();
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            ButtonBox();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the preferences box top left.
        /// </summary>
        private static void PreferencesBox()
        {
            GUILayout.BeginHorizontal("box", GUILayout.Width(330));
            scrollPosForRand = EditorGUILayout.BeginScrollView(scrollPosForRand, GUILayout.Width(298), GUILayout.Height(400));
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Preferences", BasicMeshCombinerFronts.SubTitulo);
            GUILayout.EndHorizontal();
            BasicMeshCombinerEditor.AddHorizontalLine(Color.white);
            BasicMeshCombinerPreferences.SetAddExistingComponents(PreferencesInfo("Add existing components", BasicMeshCombinerPreferences.AddExistingComponents));
            GUILayout.Space(15);
            BasicMeshCombinerPreferences.SetAddMeshCollider(PreferencesInfo("Add New Mesh Collider", BasicMeshCombinerPreferences.AddMeshCollider));
            GUILayout.Space(15);
            BasicMeshCombinerPreferences.SetAddBoxColliders(PreferencesInfo("Add Box Colliders", BasicMeshCombinerPreferences.AddBoxColliders));
            GUILayout.Space(15);
            BasicMeshCombinerPreferences.SetMakeObjectStatic(PreferencesInfo("Make Object Static", BasicMeshCombinerPreferences.MakeObjectStatic));
            GUILayout.Space(60);
            GUILayout.Label("Missing some settings? maybe try the advanced tool.", BasicMeshCombinerFronts.SubTitulo4);
            GUILayout.Space(15);
            if (GUILayout.Button("Go To Page", GUILayout.Height(35)))
            {
                Application.OpenURL(advancedMeshCombinerLink);
            }
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
            GUIStyle noGameObjectsStats = new GUIStyle();
            noGameObjectsStats.alignment = TextAnchor.MiddleCenter;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Draws the bottom left box.
        /// </summary>
        private static void Mergebox()
        {
            GUILayout.BeginHorizontal("box", GUILayout.Width(330));
            scrollPosForRand = EditorGUILayout.BeginScrollView(scrollPosForRand, GUILayout.Width(298), GUILayout.Height(207));
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("New Object Info", BasicMeshCombinerFronts.SubTitulo);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            BasicMeshCombinerEditor.AddHorizontalLine(Color.white);
            GUILayout.EndHorizontal();
            List<GameObject> objectsToCombine = BasicGameObjectManager.GetObjectDependencies(objectData);
            GUILayout.BeginVertical();
            ShowMergeInfo("Amount of Materials", BasicGameObjectManager.GetAmountOfSelectedMaterials(objectsToCombine).ToString());
            GUILayout.Space(5);
            ShowMergeInfo("verts", alVertsAmount.ToString());
            GUILayout.Space(5);
            ShowMergeInfo("DrawCalls", approximatelyDrawCalls.ToString());
            GUILayout.Space(5);
            List<Mesh> filterMeshes = new List<Mesh>();
            foreach (GameObject sceneObject in objectsToCombine)
            {
                MeshFilter filter = sceneObject.GetComponent<MeshFilter>();

                if (filter != null)
                {
                    filterMeshes.Add(filter.sharedMesh);
                }
            }

            ShowMergeInfo("amount of submeshes", BasicGameObjectManager.GetAmountOfSubMeshes(filterMeshes).ToString());
            GUILayout.EndVertical();
            GUIStyle noGameObjectsStats = new GUIStyle();
            noGameObjectsStats.alignment = TextAnchor.MiddleCenter;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }


        /// <summary>
        /// Draws the large item box rightside.
        /// </summary>
        private static void ItemBox()
        {
            GUILayout.BeginHorizontal("box", GUILayout.Width(330));
            scrollPosForItems = EditorGUILayout.BeginScrollView(scrollPosForItems, GUILayout.Width(298), GUILayout.Height(619));
            GUILayout.BeginVertical();
            GUIStyle noGameObjectsStats = new GUIStyle();
            noGameObjectsStats.alignment = TextAnchor.MiddleCenter;
            selectedObjects = Selection.gameObjects;

            if (GUILayout.Button("Enable all"))
            {
                ToggleAll(true);
            }
            if (GUILayout.Button("Disable all"))
            {
                ToggleAll(false);
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            GUILayout.BeginVertical();
            GUILayout.Label("Selected Gameobjects", BasicMeshCombinerFronts.SubTitulo);

            if (selectedObjects.Length <= 0)
            {
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                alVertsAmount = 0;
                approximatelyDrawCalls = 0;
                objectData = new List<BasicGameObjectData>();
                return;
            }
            GUILayout.Label($"All verts: {alVertsAmount}", BasicMeshCombinerFronts.SubTitulo2);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            ClearList(selectedObjects);
            foreach (var obj in selectedObjects)
            {
                string name = obj.name;
                if (name.Length > maxCharachters)
                {
                    int amountToRemove = name.Length - maxCharachters;
                    name = name.Substring(0, name.Length - amountToRemove);
                }
                int verts = BasicGameObjectManager.GetVerts(obj);
                if (!CheckIfInList(obj.name))
                {
                    objectData.Add(new BasicGameObjectData(obj, name, verts));
                }
            }
            approximatelyDrawCalls = BasicGameObjectManager.CalculateDrawCalls(objectData);
            ShowGameobjects(objectData);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }


        /// <summary>
        /// Draws the gui for the the button box bottom
        /// </summary>
        private static void ButtonBox()
        {
            GUILayout.BeginHorizontal("box", GUILayout.Width(665));
            scrollPosForRand = EditorGUILayout.BeginScrollView(scrollPosForRand, GUILayout.Width(550), GUILayout.Height(100));
            GUILayout.BeginVertical();
            GUIStyle noGameObjectsStats = new GUIStyle();
            noGameObjectsStats.alignment = TextAnchor.MiddleCenter;
            GUILayout.Space(30);
            GUILayout.BeginHorizontal();
            GUILayout.Space(130);
            if (GUILayout.Button("Combine", GUILayout.Height(50)))
            {
                CombineMeshes();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Show al gameobjects Gui part.
        /// </summary>
        /// <param name="gameObjectDatas"></param>
        private static void ShowGameobjects(List<BasicGameObjectData> gameObjectDatas)
        {
            alVertsAmount = 0;
            foreach (BasicGameObjectData data in gameObjectDatas)
            {
                GUILayout.BeginHorizontal("box", GUILayout.Width(200));
                scrollPosForRand = EditorGUILayout.BeginScrollView(scrollPosForRand, GUILayout.Width(265), GUILayout.Height(25));
                GUILayout.BeginHorizontal();
                GUILayout.Label(data.ObjName);
                data.SetActiveState(GUILayout.Toggle(data.IsEnabled, ""));
                GUILayout.Label(data.Verts.ToString() + " Verts");
                GUILayout.EndHorizontal();
                GUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
                if(data.IsEnabled)
                    alVertsAmount += data.Verts;
            }
        }

        /// <summary>
        /// Check if in list
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns>if in list or not</returns>
        private static bool CheckIfInList(string objectName)
        {
            foreach (var data in objectData)
            {
                if (data.ObjName == objectName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Clears the list
        /// </summary>
        /// <param name="objectToCheck"></param>
        private static void ClearList(GameObject[] objectToCheck)
        {
            List<BasicGameObjectData> dataToRemove = new List<BasicGameObjectData>();
            foreach (var data in objectData)
            {
                bool isSelected = false;
                foreach (var obj in objectToCheck)
                {
                    if (data.ObjName == obj.name)
                    {
                        isSelected = true;
                    }
                }
                if (isSelected)
                {
                    continue;
                }
                else
                {
                    dataToRemove.Add(data);
                }
            }

            foreach (var oldData in dataToRemove)
            {
                objectData.Remove(oldData);
            }
        }

        /// <summary>
        /// Combines meshes only with basic way.
        /// </summary>
        private static void CombineMeshes()
        {
            List<GameObject> objectsToCombine = BasicGameObjectManager.GetMeshRenderers(objectData);

            BasicMergedObject mergeObject = BasicMergingManager.DoCombineMeshes_OneMeshPerMaterial(objectsToCombine);


            GameObject combinedObject = new GameObject("Combined Mesh");
            combinedObject.AddComponent<MeshFilter>().sharedMesh = mergeObject.Mesh;
            combinedObject.AddComponent<MeshRenderer>().sharedMaterials = mergeObject.MaterialList;

            if (BasicMeshCombinerPreferences.AddExistingComponents)
            {
                foreach (var mergeObj in objectsToCombine)
                {
                    Component[] sourceComponents = mergeObj.GetComponents<Component>();
                    foreach (Component sourceComponent in sourceComponents)
                    {
                        Component targetComponent = combinedObject.GetComponent(sourceComponent.GetType());
                        if (targetComponent == null || sourceComponent.GetType().UnderlyingSystemType.ToString().Contains(scriptFilter) || sourceComponent.GetType().ToString().Contains(colliderFilter))
                        {
                            if (sourceComponent.GetType().ToString().Contains(colliderFilter))
                            {
                                return;
                            }

                            combinedObject.AddComponent(sourceComponent.GetType());
                        }
                    }
                }
            }

            if (BasicMeshCombinerPreferences.AddMeshCollider)
            {
                combinedObject.AddComponent<MeshCollider>().sharedMesh = mergeObject.Mesh;
            }

            if (BasicMeshCombinerPreferences.AddBoxColliders)
            {
                Mesh mergedMesh = mergeObject.Mesh;
                // Get the submesh count and the submesh bounds
                int submeshCount = mergedMesh.subMeshCount;
                Bounds[] submeshBounds = new Bounds[submeshCount];
                mergedMesh.RecalculateBounds();
                for (int i = 0; i < submeshCount; i++)
                {
                    int[] triangles = mergedMesh.GetTriangles(i);
                    if (triangles.Length > 0)
                    {
                        submeshBounds[i] = new Bounds(mergedMesh.vertices[triangles[0]], Vector3.zero);
                        for (int j = 0; j < triangles.Length; j++)
                        {
                            submeshBounds[i].Encapsulate(mergedMesh.vertices[triangles[j]]);
                        }


                        BoxCollider boxCollider = combinedObject.AddComponent<BoxCollider>();
                        boxCollider.center = submeshBounds[i].center - combinedObject.transform.position;
                        boxCollider.size = submeshBounds[i].size;
                    }
                }
            }

            if (BasicMeshCombinerPreferences.MakeObjectStatic)
            {
                combinedObject.isStatic = true;
            }
        }

        /// <summary>
        /// Toggle al gameobject status to an sertan state.
        /// </summary>
        /// <param name="_enabled">The new state of the gameobject.</param>
        private static void ToggleAll(bool _enabled)
        {
            foreach (var data in objectData)
            {
                data.SetActiveState(_enabled);
            }
        }

        /// <summary>
        /// Shows the merge info in an compact box.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_value"></param>
        private static void ShowMergeInfo(string _name, string _value)
        {
            GUILayout.BeginHorizontal("box", GUILayout.Width(200));
            scrollPosForRand = EditorGUILayout.BeginScrollView(scrollPosForRand, GUILayout.Width(265), GUILayout.Height(25));
            GUILayout.BeginHorizontal();
            GUILayout.Label(_value, BasicMeshCombinerFronts.LeftText);
            GUILayout.Label(_name, BasicMeshCombinerFronts.RightText);
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Shows the preferences and returns trigger bool.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        private static bool PreferencesInfo(string _name, bool _value)
        {
            bool returnbool = _value;
            GUILayout.BeginHorizontal("box", GUILayout.Width(200));
            scrollPosForRand = EditorGUILayout.BeginScrollView(scrollPosForRand, GUILayout.Width(265), GUILayout.Height(25));
            GUILayout.BeginHorizontal();
            returnbool = GUILayout.Toggle(returnbool, "");
            GUILayout.Label(_name, BasicMeshCombinerFronts.RightText);
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();

            return returnbool;
        }
    }
}