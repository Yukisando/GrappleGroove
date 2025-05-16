#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class ResetButton : GameButton
{
    [SerializeField] List<ID> objetsToReset;

    public void ResetObjects() {
        if (objetsToReset == null || objetsToReset.Count == 0) return;

        foreach (var obj in objetsToReset) {
            obj.ResetObject();
        }
    }
}