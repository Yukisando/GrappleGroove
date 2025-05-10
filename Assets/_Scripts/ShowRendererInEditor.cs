#region

using UnityEngine;

#endregion

public class ShowRendererInEditor : MonoBehaviour
{
    void Start() {
        GetComponent<MeshRenderer>().enabled = Application.isEditor;
    }
}