using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameCompleteType {
    INCOMPLETE,
    BRONZE_AND_SILVER,
    PERFECT
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image m_fadeScreen;
    public float defaultFadeTime;
    [SerializeField] private TitleScreenUI m_titleScreenUI;
    [SerializeField] private GameObject m_levelSelectContainer;
    [SerializeField] private LevelSelect m_levelSelect;
    [SerializeField] private LevelUI m_levelUI;
    public static Color[] rankColours;     // Background colours for platinum clear, gold clear, silver clear, clear, and no clear
    private GameCompleteType m_bestCompleteTypeDisplayed;     // The highest level type of game complete shown to the player
    [SerializeField] private GameObject m_gameCompleteScreen;
    [SerializeField] private GameObject[] m_completeTypeScreens;

    public IEnumerator FadeScreen() {
        StartCoroutine(FadeScreen(defaultFadeTime));
        yield return null;
    }

    public IEnumerator FadeScreen(float duration) {
        m_fadeScreen.gameObject.SetActive(true);
        Color newColour = m_fadeScreen.color;
        float halfDuration = duration / 2;
        float timeElapsed = 0;

        // Fade in
        while (timeElapsed < halfDuration) {
            timeElapsed += Time.deltaTime;
            newColour.a = Mathf.Lerp(0, 1, timeElapsed / halfDuration);
            m_fadeScreen.color = newColour;
            yield return null;
        }
        newColour.a = 1;
        m_fadeScreen.color = newColour;

        // Fade out
        timeElapsed = 0;
        while (timeElapsed < halfDuration) {
            timeElapsed += Time.deltaTime;
            newColour.a = Mathf.Lerp(1, 0, timeElapsed / halfDuration);
            m_fadeScreen.color = newColour;
            yield return null;
        }
        newColour.a = 0;
        m_fadeScreen.color = newColour;
        m_fadeScreen.gameObject.SetActive(false);
    }

    private void ClearUI() {
        m_titleScreenUI.gameObject.SetActive(false);
        m_levelSelectContainer.SetActive(false);
        m_levelUI.gameObject.SetActive(false);
        m_levelUI.CloseLevelCompleteWindow();
    }

    public void OpenTitleScreen() {
        StartCoroutine(OpenTitleScreenAfterFade());
    }

    private IEnumerator OpenTitleScreenAfterFade() {
        StartCoroutine(FadeScreen());
        yield return new WaitForSeconds(defaultFadeTime / 2);
        ClearUI();
        m_titleScreenUI.instructions.SetActive(false);
        m_titleScreenUI.about.SetActive(false);
        m_titleScreenUI.gameObject.SetActive(true);
        m_titleScreenUI.OnTitleScreenOpen();
    }

    // Switch to level select screen
    public void OpenLevelSelect() {
        StartCoroutine(OpenLevelSelectAfterFade());
    }

    public IEnumerator OpenLevelSelectAfterFade() {
        StartCoroutine(FadeScreen());
        yield return new WaitForSeconds(defaultFadeTime / 2);
        ClearUI();
        m_levelSelectContainer.SetActive(true);
    }

    // Update the info of the level with the given levelId and open the level complete window
    public void FinishLevel(int finishedLevel, ClearRank rank) {
        m_levelSelect.UpdateClearInfo(finishedLevel);
        m_levelUI.OpenLevelCompleteWindow(rank);
    }

    public void OpenLevelUI() {
        ClearUI();
        m_levelUI.gameObject.SetActive(true);
        m_levelUI.UpdateMoves(0);
    }

    public void UpdateLevelNumber(int levelId) {
        m_levelUI.UpdateLevelNumber(levelId);
    }

    // Update the display of the player's best moves # in the game UI
    public void LoadPersonalBest(int best) {
        m_levelUI.UpdatePersonalBest(best);
    }

    public void UpdateMoves(int moves) {
        m_levelUI.UpdateMoves(moves);
    }

    public void SetPersonalBest(int levelIndex, int best) {
        m_levelSelect.SavePlayerBest(levelIndex, best);
    }

    public void ShowGameComplete(GameCompleteType gameCompleteType) {
        if (m_bestCompleteTypeDisplayed >= gameCompleteType) {
            // Don't display game complete if player has already seen it or a better one before
            return;
        }
        if (gameCompleteType == GameCompleteType.PERFECT || gameCompleteType == GameCompleteType.BRONZE_AND_SILVER) {
            m_gameCompleteScreen.SetActive(true);
            m_completeTypeScreens[(int) gameCompleteType].SetActive(true);
            m_bestCompleteTypeDisplayed = gameCompleteType;
        }
    }
}
