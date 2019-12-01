using System.Collections;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor (typeof (LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
        LevelGenerator levelGenerator = (LevelGenerator) target;

        if (DrawDefaultInspector()) {
            if (levelGenerator.t_autoUpdate) {
                levelGenerator.GenerateTestMap();
            }
        }

        if (GUILayout.Button("Generate")) {
            levelGenerator.GenerateTestMap();
        }

        if (GUILayout.Button("Delete Children")) {
            for (int i = levelGenerator.transform.childCount - 1; i >= 0; i--) {
                DestroyImmediate(levelGenerator.transform.GetChild(i).gameObject);
            }
        }
    }
}
#endif