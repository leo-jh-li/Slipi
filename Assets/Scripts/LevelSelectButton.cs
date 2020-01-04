using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private Image m_background;
    [SerializeField] private TextMeshProUGUI m_levelNumber;
    [SerializeField] private TextMeshProUGUI m_bestText;
    [SerializeField] private RawImage m_levelPreview;

    public void Initialize(int levelIndex, Texture2D m_levelPreviewTexture) {
        m_levelNumber.text = (levelIndex + 1).ToString();
        m_levelPreview.texture = m_levelPreviewTexture;
        // Scale down the smaller side
        float ratio = (float) m_levelPreviewTexture.width / m_levelPreviewTexture.height;
        if (ratio < 1) {
            m_levelPreview.rectTransform.sizeDelta = new Vector2(m_levelPreview.rectTransform.rect.width * ratio, m_levelPreview.rectTransform.rect.height);
        } else {
            m_levelPreview.rectTransform.sizeDelta = new Vector2(m_levelPreview.rectTransform.rect.width, m_levelPreview.rectTransform.rect.height * (1 / ratio));
        }
    }

    public void UpdateColour(ClearRank rank) {
        m_background.color = LevelData.GetColourFromRank(rank);
    }

    public void UpdateBest(string bestMoves) {
        m_bestText.gameObject.SetActive(true);
        m_bestText.text = "Best: " + bestMoves;
    }
}
