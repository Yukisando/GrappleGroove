#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public static class SceneObjectTracker
{
    public static HashSet<GameObject> InactiveSceneObjects = new HashSet<GameObject>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CacheInactiveObjects() {
        InactiveSceneObjects.Clear();

        foreach (var id in Object.FindObjectsByType<ID>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
            if (!id.gameObject.activeInHierarchy) InactiveSceneObjects.Add(id.gameObject);
        }
    }

    public static bool WasOriginallyInactive(GameObject go) {
        return InactiveSceneObjects.Contains(go);
    }
}