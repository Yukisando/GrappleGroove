#region

using UnityEngine;

#endregion

public class AwakeActivate : MonoBehaviour
{
    [SerializeField] bool startActivate;

    void Awake() {
        gameObject.SetActive(startActivate);
    }
}