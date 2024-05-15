#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    [System.Serializable]
    public class HierarchyDesigner_Info_Buttons
    {
        #region Properties
        [SerializeField] private ButtonsPositionType buttonsPositionType = ButtonsPositionType.Docked;
        #endregion

        #region Accessors
        public ButtonsPositionType ButtonsPosition
        {
            get => buttonsPositionType;
            set => buttonsPositionType = value;
        }

        public enum ButtonsPositionType
        {
            AfterProperties,
            Docked,
        }
        #endregion

        public HierarchyDesigner_Info_Buttons(ButtonsPositionType buttonsPositionType)
        {
            this.buttonsPositionType = buttonsPositionType;
        }
    }
}
#endif