using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{   
    [SerializeField] private GameObject m_navButtons;
    [SerializeField] private TextMeshProUGUI m_levelNumber;
    [SerializeField] private TextMeshProUGUI m_movesContent;
    [SerializeField] private GameObject m_best;
    [SerializeField] private int m_maxDisplayableMoves;    // The greatest number of moves that will be precisely displayed
    [SerializeField] private TextMeshProUGUI m_bestContent;
    [SerializeField] private GameObject m_levelCompleteWindow;
    [SerializeField] private TextMeshProUGUI m_levelCompleteText;

    public void UpdateLevelNumber(int levelId) {
        m_levelNumber.SetText((levelId + 1).ToString());
    }

    public void UpdateMoves(int moves) {
        m_movesContent.SetText(ApplyNumberCap(moves));
    }
    
    // Updates the value for the player's personal best, or hides the value if the player has no personal best
    public void UpdatePersonalBest(int best) {
        if (best == 0) {
            m_best.SetActive(false);
        } else {
            m_best.SetActive(true);
            m_bestContent.SetText(ApplyNumberCap(best));
        }
    }

    private string ApplyNumberCap(int number) {
        if (number > m_maxDisplayableMoves) {
            return m_maxDisplayableMoves.ToString() + "+";
        } else {
            return number.ToString();
        }
    }

    public void OpenLevelCompleteWindow(ClearRank rank) {
        m_navButtons.SetActive(false);
        m_levelCompleteWindow.SetActive(true);
        m_levelCompleteText.color = LevelData.GetColourFromRank(rank);
    }

    public void CloseLevelCompleteWindow() {
        m_navButtons.SetActive(true);
        m_levelCompleteWindow.SetActive(false);
    }
}
