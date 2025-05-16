#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("The model will only be visible while in the editor")]
public class ShowRendererInEditor : MonoBehaviour
{
    void Start() {
        GetComponent<MeshRenderer>().enabled = Application.isEditor;
    }
}