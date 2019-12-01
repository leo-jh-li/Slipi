using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleArt : MonoBehaviour
{
    [SerializeField] private GameObject m_tile;
    [SerializeField] private float m_tileGap;
    [SerializeField] private float m_tileWidth;
    [SerializeField] Vector2 m_artDimensions;
    [SerializeField] Vector2 m_titleDimensions;
    private List<Vector2> m_titleTiles;
    [SerializeField] private float m_alphaLossPerDistance;

    public void GenerateArt() {
        InitializeTitleTiles();
        float tileDist = m_tileWidth + m_tileGap;
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 topLeftPos = new Vector2(rect.position.x - tileDist * (m_artDimensions.x / 2 - 0.5f), rect.position.y + tileDist * (m_artDimensions.y / 2 - 0.5f));
        for (int y = 0; y < m_artDimensions.y; y++) {
            for (int x = 0; x < m_artDimensions.x; x++) {
                Image tile = Instantiate(m_tile, topLeftPos + new Vector2(x * tileDist, y * -tileDist), Quaternion.identity, transform).GetComponent<Image>();
                Vector2 coord = new Vector2(x, y);
                if (m_titleTiles.Contains(coord)) {
                    tile.color = Color.black;
                } else {
                    Color newAlpha = tile.color;
                    newAlpha.a -= GetDistFromTitleTiles(coord) * m_alphaLossPerDistance;
                    tile.color = newAlpha;
                }
            }
        }
    }

    private void InitializeTitleTiles() {
        m_titleTiles = new List<Vector2>();
        Vector2 centreTile = new Vector2((m_artDimensions.x - 1) / 2, (m_artDimensions.y - 1) / 2);
        Vector2 topLeftOfTitle = new Vector2(Mathf.Ceil(centreTile.x - (m_titleDimensions.x / 2)), Mathf.Ceil(centreTile.y - (m_titleDimensions.y / 2)));
        for (int y = 0; y < m_titleDimensions.y; y++) {
            for (int x = 0; x < m_titleDimensions.x; x++) {
                m_titleTiles.Add(new Vector2(topLeftOfTitle.x + x, topLeftOfTitle.y + y));
            }
        }
    }

    private int GetDistFromTitleTiles(Vector2 coord) {
        return GetXDistance(coord.x) + GetYDistance(coord.y);
    }

    private int GetXDistance(float x) {
        foreach (Vector2 v in m_titleTiles) {
            if (v.x == x) {
                return 0;
            }
        }
        return (int) Mathf.Min(Mathf.Abs(m_titleTiles[0].x - x), Mathf.Abs(m_titleTiles[m_titleTiles.Count - 1].x - x));
    }

    private int GetYDistance(float y) {
        foreach (Vector2 v in m_titleTiles) {
            if (v.y == y) {
                return 0;
            }
        }
        return (int) Mathf.Min(Mathf.Abs(m_titleTiles[0].y - y), Mathf.Abs(m_titleTiles[m_titleTiles.Count - 1].y - y));
    }
}
