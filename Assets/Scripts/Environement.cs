#region

using UnityEngine;

#endregion

public class Environment : MonoBehaviour
{
    static readonly int Rotation = Shader.PropertyToID("_Rotation");
    public float rotationSpeed = 1.0f;
    
    void Update() {
        RenderSettings.skybox.SetFloat(Rotation, Time.time * rotationSpeed);
    }
}