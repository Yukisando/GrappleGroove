using System;
using UnityEngine;

namespace DeTools.BasicMeshRenderer.Data
{
    /// <summary>
    /// Gameobject data that keeps track of the gameobject, name,verts,enabled in tool.
    /// </summary>
    [Serializable]
    public class BasicGameObjectData
    {
        /// <summary>
        /// Constructor of BasicGameObjectData.
        /// </summary>
        /// <param name="_obj">Gameobject.</param>
        /// <param name="_objName">Object name.</param>
        /// <param name="_verts">Verts that the object has.</param>
        /// <param name="_isEnabled">Is enabled.</param>
        public BasicGameObjectData(GameObject _obj, string _objName = "Error", int _verts = 0, bool _isEnabled = true)
        {
            obj = _obj;
            objName = _objName;
            verts = _verts;
            isEnabled = _isEnabled;
        }

        /// <summary>
        /// The Gameobject that can be set but only private or in constructor.
        /// </summary>
        [SerializeField]
        private GameObject obj;

        /// <summary>
        /// The Gameobject name that can be set but only private or in constructor.
        /// </summary>
        [SerializeField]
        private string objName;

        /// <summary>
        /// The Gameobject Vertesies that can be set but only private or in constructor.
        /// </summary>
        [SerializeField]
        private int verts;

        /// <summary>
        /// An bool that is enabled or disabled that can be set but only private or in constructor or in the SetActiveState function.
        /// </summary>
        [SerializeField]
        private bool isEnabled;

        /// <summary>
        /// The public gameobject that is set private
        /// </summary>
        public GameObject Obj => obj;

        /// <summary>
        /// The public gameobject name that is set private
        /// </summary>
        public string ObjName => objName;

        /// <summary>
        /// The public gameobject Vert count that is set private
        /// </summary>
        public int Verts => verts;

        /// <summary>
        /// An public bool that is enabled or disabled that can be set but only private or in constructor or in the SetActiveState function.
        /// </summary>
        public bool IsEnabled => isEnabled;

        /// <summary>
        /// An void that keeps count of setting IsEnabled to an newState.
        /// </summary>
        /// <param name="newState">The new state of isEnabled.</param>
        public void SetActiveState(bool newState)
        {
            isEnabled = newState;
        }
    }
}
