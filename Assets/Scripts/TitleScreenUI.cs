using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenUI : MonoBehaviour
{
    public GameObject instructions;
    private bool m_instructionsOpen;
    public GameObject about;
    private bool m_aboutOpen;

    public void OnTitleScreenOpen() {
        m_instructionsOpen = false;
        m_aboutOpen = false;
    }

    public void OnClickInstructions() {
        if (m_instructionsOpen) {
            m_instructionsOpen = false;
            instructions.SetActive(false);
        } else {
            m_aboutOpen = false;
            about.SetActive(false);
            m_instructionsOpen = true;
            instructions.SetActive(true);
        }
    }

    public void OnClickAbout() {
        if (m_aboutOpen) {
            m_aboutOpen = false;
            about.SetActive(false);
        } else {
            m_instructionsOpen = false;
            instructions.SetActive(false);
            m_aboutOpen = true;
            about.SetActive(true);
        }
    }
}
