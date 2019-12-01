using System.Collections;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor (typeof (TitleArt))]
public class TileArtEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        TitleArt generator = (TitleArt) target;
        
        if (GUILayout.Button("Generate")) {
            generator.GenerateArt();
        }

        if (GUILayout.Button("Delete Art")) {
            for (int i = generator.transform.childCount - 1; i >= 0; i--) {
                DestroyImmediate(generator.transform.GetChild(i).gameObject);
            }
        }
    }
}
#endif