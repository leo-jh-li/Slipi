using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    private static Constants s_instance = null;

    public Color[] rankColours;     // Background colours for platinum clear, gold clear, silver clear, clear, and no clear
    public string COLLISION_SFX_NAME;
    public string GOAL_SFX_NAME;
    public string LEVEL_COMPLETE_SFX_NAME;
    public string SLIDE_SFX_NAME;

    public static Constants instance {
        get {
            if (s_instance == null) {
                s_instance = FindObjectOfType(typeof(Constants)) as Constants;
            }
            if (s_instance == null) {
                GameObject obj = new GameObject("Constants");
                s_instance = obj.AddComponent<Constants>();
            }
            return s_instance;
        }
    }

    private void OnApplicationQuit() {
        s_instance = null;
    }
}
