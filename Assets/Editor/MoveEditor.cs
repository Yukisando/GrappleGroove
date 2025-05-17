#region

using UnityEditor;
using UnityEngine;

#endregion

[CustomEditor(typeof(Move))]
public class MoveEditor : Editor
{
    void OnSceneGUI() {
        var move = (Move)target;
        if (!move.showGizmos)
            return;

        var t = move.transform;
        var startPoint = move.useLocalPosition ? t.localPosition : t.position;
        var worldDestination = move.useLocalPosition
            ? t.TransformPoint(move.destination)
            : t.position + move.destination;

        // Draw the handle
        EditorGUI.BeginChangeCheck();
        var newWorldDestination = Handles.PositionHandle(worldDestination, Quaternion.identity);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(move, "Move Destination Handle");

            if (move.useLocalPosition)
                move.destination = t.InverseTransformPoint(newWorldDestination);
            else
                move.destination = newWorldDestination - t.position;

            EditorUtility.SetDirty(move);
        }
    }
}