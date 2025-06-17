#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

[InfoBox("The model will only be visible while in the editor")]
public class HideInBuild : MonoBehaviour
{
    void Start() {
        HideAllRenderers();
    }

    void HideAllRenderers() {
        var renderers = GetComponentsInChildren<Renderer>(true);
        foreach (var r in renderers) {
            r.enabled = Application.isEditor;
        }
    }
}