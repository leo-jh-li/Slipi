using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClearRank {
    NO_CLEAR,
    BRONZE_CLEAR,
    SILVER_CLEAR,
    GOLD_CLEAR,
    PLATINUM_CLEAR
}

[System.Serializable]
public struct LevelData
{
    [HideInInspector] public int levelId;
    public Texture2D map;
    public int minMoves;        // The minimum number of moves required to clear the level
    public int silverMoves;     // The greatest number moves acceptable to attain a silver ranking

    public static ClearRank GetClearRank(LevelData levelData, int moves) {
        if (moves == 0) {
            return ClearRank.NO_CLEAR;
        } else {
            if (moves < levelData.minMoves) {
                return ClearRank.PLATINUM_CLEAR;
            } else if (moves == levelData.minMoves) {
                return ClearRank.GOLD_CLEAR;
            } else if (moves <= levelData.silverMoves) {
                return ClearRank.SILVER_CLEAR;
            } else {
                return ClearRank.BRONZE_CLEAR;
            }
        }
    }
    
    public static Color GetColourFromRank(ClearRank rank) {
        return Constants.instance.rankColours[(int) rank];
    }
}
