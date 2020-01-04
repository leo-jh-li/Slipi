using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelGenerator m_levelGenerator;
    [SerializeField] private UIManager m_uiManager;
    [SerializeField] private CameraBehaviour m_cameraBehaviour;
    [SerializeField] private LevelSelect m_levelSelect;
    private PlayerControls m_playerControls;
    private LevelData m_currLevelData;
    private Level m_currLevel;
    private int m_movesMade;

    public bool t_autoGenerate;     // Testing variable that indicates whether to automatically generate the test level on start

    private void Start() {
        m_playerControls = GetComponent<PlayerControls>();
        Piece.SetVictoryCheckCallback(OnPieceStop);
        PersistentData.InitializeData(m_levelSelect.GetLevelQuantity());
        if (t_autoGenerate) {
            GenerateLevel();
        }
    }

    private void GenerateLevel() {
        m_currLevel = Object.FindObjectOfType<LevelGenerator>().GenerateTestMap();
    }

    private void LoadLevel(int levelId) {
        m_currLevelData = m_levelSelect.GetLevelData(levelId);
        m_currLevelData.levelId = levelId;
    }

    public void StartLevel(int levelId) {
        LoadLevel(levelId);
        StartCoroutine(m_uiManager.FadeScreen());
        StartCoroutine(ActivateLevelAfterDelay(m_uiManager.defaultFadeTime));
    }

    private void GenerateFromCurrLevelData() {
        m_levelGenerator.ClearLevel();
        m_currLevel = m_levelGenerator.GenerateLevel(m_currLevelData.map);
        m_movesMade = 0;
        m_uiManager.OpenLevelUI();
    }

    public void ResetLevel() {
        m_playerControls.enabled = false;
        m_playerControls.onLevelCompleteScreen = false;
        GenerateFromCurrLevelData();
        StartCoroutine(EnableControlsAfterDelay(0.25f));
    }

    private IEnumerator ActivateLevelAfterDelay(float delay) {
        yield return new WaitForSeconds(delay / 2);
        GenerateFromCurrLevelData();
        m_cameraBehaviour.ResizeCamera(m_currLevel.GetWidth());
        m_uiManager.LoadPersonalBest(PersistentData.GetBest(m_currLevelData.levelId));
        m_uiManager.UpdateLevelNumber(m_currLevelData.levelId);
        yield return new WaitForSeconds(delay / 2);
        m_playerControls.enabled = true;
    }

    private IEnumerator EnableControlsAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        m_playerControls.enabled = true;
    }

    public void MakeMove(Direction dir) {
        if (m_currLevel.MoveAllPieces(dir)) {
            m_movesMade++;
            m_uiManager.UpdateMoves(m_movesMade);
        }
    }

    // Function to be run each time a piece stops moving; returns true and ends a level iff the current level is complete
    private bool OnPieceStop() {
        if (!m_currLevel.PiecesInMotion()) {
            // Slide sfx disabled
            // AudioManager.instance.Stop(Constants.instance.SLIDE_SFX_NAME);
        }
        if (m_currLevel.PiecesPlacedInGoal()) {
            // Slide sfx disabled
            // AudioManager.instance.Stop(Constants.instance.SLIDE_SFX_NAME);
            PersistentData.UpdateBest(m_currLevelData.levelId, m_movesMade);
            m_uiManager.LoadPersonalBest(PersistentData.GetBest(m_currLevelData.levelId));
            m_uiManager.FinishLevel(m_currLevelData.levelId, LevelData.GetClearRank(m_currLevelData, m_movesMade));
            m_playerControls.onLevelCompleteScreen = true;
            return true;
        }
        return false;
    }

    public void ReturnToLevelSelect() {
        m_playerControls.onLevelCompleteScreen = false;
        m_playerControls.enabled = false;
        m_uiManager.OpenLevelSelect();
        StartCoroutine(ActivateLevelSelectAfterDelay());
    }

    // Fades the screen and opens level select half way through the fade (when the fade screen is opaque)
    private IEnumerator ActivateLevelSelectAfterDelay() {
        yield return new WaitForSeconds(m_uiManager.defaultFadeTime / 2);
        m_levelGenerator.ClearLevel();
        CheckGameComplete();
    }

    private void CheckGameComplete() {
        if (m_levelSelect.AllLevelsPerfect()) {
            m_uiManager.ShowGameComplete(GameCompleteType.PERFECT);
        } else if (m_levelSelect.AllLevelsComplete()) {
            m_uiManager.ShowGameComplete(GameCompleteType.BRONZE_AND_SILVER);
        }
    }
}
