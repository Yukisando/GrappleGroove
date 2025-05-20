#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class ResetButton : GameButton
{
    [SerializeField] List<string> idsToReset;

    public void ResetObjects() {
        if (idsToReset == null || idsToReset.Count == 0) return;

        foreach (string id in idsToReset) {
            GameManager.I.ResetObjectByID(id);
        }
    }
}