using System.Collections;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor (typeof (LevelSelect))]
public class LevelSelectEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        LevelSelect levelSelect = (LevelSelect) target;

        if (GUILayout.Button("Preview Layout")) {
            levelSelect.GenerateGrid();
        }

        if (GUILayout.Button("Delete Buttons")) {
            levelSelect.DestroyLevelGrid();
        }
    }
}
#endif