using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private Transform m_pagesContainer;
    [SerializeField] private GameObject m_pagePrefab;
    [SerializeField] private GameObject m_selectButtonPrefab;
    private List<GameObject> m_selectButtons = new List<GameObject>();
    private int m_currPageIndex;
    private List<GameObject> m_pages = new List<GameObject>();

    [Header("Page Layout")]
    [SerializeField] private int m_gridWidth;
    [SerializeField] private int m_gridHeight;
    private int m_levelsPerPage;        // The number of levels shown in a single grid of levels
    private int m_activePage;
    [SerializeField] private float m_xMargin;
    [SerializeField] private float m_yMargin;
    
    [Header("Levels")]
    [SerializeField] private LevelData[] m_levelData;

    [Header("Preview Generation")]
    [SerializeField] private Color[] inColours;
    [SerializeField] private Color[] outColours;

    private void Start() {
        GenerateGrid();
    }

    public void GenerateGrid() {
        DestroyLevelGrid();
        m_levelsPerPage = m_gridWidth * m_gridHeight;
        Vector2 topLeft = new Vector2(transform.position.x - ((m_gridWidth / 2f) - 0.5f) * m_xMargin, transform.position.y + ((m_gridHeight / 2f) - 0.5f) * m_yMargin);
        int x = 0;
        int y = 0;
        for (int i = 0; i < m_levelData.Length; i++) {
            int page = GetPageForIndex(i);

            // Make new page if it does not exist
            if (page >= m_pages.Count) {
                GameObject newPage = Instantiate(m_pagePrefab, m_pagesContainer);
                newPage.SetActive(false);
                m_pages.Add(newPage);
            }
            GameObject button = Instantiate(m_selectButtonPrefab);
            button.transform.SetParent(m_pages[page].transform, false);
            button.GetComponent<RectTransform>().position = topLeft + new Vector2(m_xMargin * (x % m_levelsPerPage), -m_yMargin * (y % m_levelsPerPage));
            button.GetComponent<LevelSelectButton>().Initialize(i, GenerateLevelPreview(m_levelData[i].map));
            int tempI = i;
            button.GetComponent<Button>().onClick.AddListener(()=> m_gameManager.StartLevel(tempI));
            m_selectButtons.Add(button);
            if (++x >= m_gridWidth) {
                x = 0;
                if (++y >= m_gridHeight) {
                    y = 0;
                }
            }
        }
        if (m_pages.Count > 0) {
            m_pages[0].SetActive(true);
            m_activePage = 0;
        }
    }

    // Destroys currently instantiated level grid if it exists
    public void DestroyLevelGrid() {
        foreach(GameObject obj in m_pages) {
            DestroyImmediate(obj);
        }
        m_pages.Clear();
        m_selectButtons.Clear();
    }

    // Returns the index of the page that contains the levelIndex-th level
    private int GetPageForIndex(int levelIndex) {
        if (m_levelsPerPage == 0) {
            Debug.LogWarning("Invalid grid dimensions.");
            return 0;
        }
        return (int) Mathf.Ceil(levelIndex / m_levelsPerPage);
    }

    public void CyclePages(int numToCycle) {
        m_pages[m_activePage].SetActive(false);
        m_activePage += numToCycle;
        m_activePage = Utilities.Modulo(m_activePage, m_pages.Count);
        m_pages[m_activePage].SetActive(true);
    }

    // Returns a simplified Texture2D representation of a given level map
    public Texture2D GenerateLevelPreview(Texture2D map) {
        Texture2D texture = new Texture2D(map.width / 2 + 1, map.height / 2 + 1, TextureFormat.ARGB32, false);
        Color[] colourMap = new Color[texture.width * texture.height];
        for (int y = 0; y < texture.height; y++) {
            for (int x = 0; x < texture.width; x++) {
                colourMap[texture.width * y + x] = TranslateColour(map.GetPixel(x * 2, y * 2));
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    // Returns colour associated with the given colour if there is one, or no white otherwise
    private Color TranslateColour(Color colour) {
        if (colour == Color.clear) {
            return Color.clear;
        }
        for (int i = 0; i < inColours.Length; i++) {
            if (colour == inColours[i]) {
                return outColours[i];
            }
        }
        return Color.white;
    }

    public void UpdateClearInfo(int levelId) {
        LevelSelectButton levelSelectButton = m_selectButtons[levelId].GetComponent<LevelSelectButton>();
        int best = m_levelData[levelId].playerBestMoves;
        levelSelectButton.UpdateColour(LevelData.GetClearRank(m_levelData[levelId], best));
        levelSelectButton.UpdateBest(best.ToString());
    }

    public void SavePlayerBest(int levelIndex, int moves) {
        if (m_levelData[levelIndex].playerBestMoves == 0 || moves < m_levelData[levelIndex].playerBestMoves) {
            m_levelData[levelIndex].playerBestMoves = moves;
        }
    }
    
    public LevelData GetLevelData(int index) {
        return m_levelData[index];
    }

    public bool AllLevelsComplete() {
        foreach (LevelData levelData in m_levelData) {
            if (levelData.playerBestMoves == 0) {
                return false;
            }
        }
        return true;
    }
    
    public bool AllLevelsPerfect() {
        foreach (LevelData levelData in m_levelData) {
            ClearRank rank = LevelData.GetClearRank(levelData, levelData.playerBestMoves);
            if (rank != ClearRank.GOLD_CLEAR && rank != ClearRank.PLATINUM_CLEAR) {
                return false;
            }
        }
        return true;
    }
}
