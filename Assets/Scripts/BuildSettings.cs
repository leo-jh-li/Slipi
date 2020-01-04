using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BuildTarget {
    PC,
    Android
}

public class BuildSettings : MonoBehaviour
{
    [SerializeField] private BuildTarget buildTarget;

    [Header("Build Dependent Elements")]
    [SerializeField] private TextMeshProUGUI m_instructionsText;
    
    private void OnValidate() {
        if (buildTarget == BuildTarget.PC) {
            m_instructionsText.text = "use the arrow keys to move pieces around the level\n\n" +
                                      "pieces move until they collide with something\n\n" +
                                      "move all pieces to their respective destinations to complete the level\n\n" +
                                      "try to use as few moves as possible";
        } else if (buildTarget == BuildTarget.Android) {
            m_instructionsText.text = "swipe to move pieces around the level\n\n" +
                                      "pieces move until they collide with something\n\n" +
                                      "move all pieces to their respective destinations to complete the level\n\n" +
                                      "try to use as few moves as possible";
        }
    }
}
