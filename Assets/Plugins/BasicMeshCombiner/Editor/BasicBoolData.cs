using System;
using UnityEngine;

namespace DeTools.BasicMeshRenderer.Data
{
    /// <summary>
    /// Data that can be converted from json to this class.
    /// </summary>
    [Serializable]
    public class BasicBoolData
    {
        /// <summary>
        /// BasicBoolData constructor
        /// </summary>
        /// <param name="_value"> value of the bool.</param>
        public BasicBoolData(bool _value)
        {
            value = _value;
        }

        /// <summary>
        /// Private bool that keeps track of the class his value.
        /// </summary>
        [SerializeField]
        private bool value;

        /// <summary>
        /// public bool that shows the private variable as public.
        /// </summary>
        public bool Value=>value;
    }
}