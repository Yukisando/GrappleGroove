#region

using Sirenix.Utilities;
using UnityEngine;

#endregion

public class TriggerObject : MonoBehaviour
{
    public string id;

    void Awake() {
        if (id.IsNullOrWhitespace()) id = name;
    }

    void OnValidate() {
        if (id.IsNullOrWhitespace()) id = name;
    }
}