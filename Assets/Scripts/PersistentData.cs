using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentData
{
    public static void InitializeData(int levelQuantity) {
        // Best moves
        for (int i = 0; i < levelQuantity; i++) {
            string key = i.ToString();
            if (!PlayerPrefs.HasKey(key)) {
                PlayerPrefs.SetInt(key, 0);
            }
        }

        if (!PlayerPrefs.HasKey(Constants.instance.HIGHEST_CLEAR_TYPE_KEY)) {
            PlayerPrefs.SetInt(Constants.instance.HIGHEST_CLEAR_TYPE_KEY, 0);
        }
    }

    public static int GetBest(int levelId) {
        return PlayerPrefs.GetInt(levelId.ToString());
    }

    public static void UpdateBest(int levelId, int moves) {
        int currBest = PlayerPrefs.GetInt(levelId.ToString());
        if (currBest == 0 || moves < currBest) {
            PlayerPrefs.SetInt(levelId.ToString(), moves);
        }
    }

    // Return t highest level type of game complete shown to the player
    public static int GetScreenSeen() {
        return PlayerPrefs.GetInt(Constants.instance.HIGHEST_CLEAR_TYPE_KEY);
    }

    public static void SetScreenSeen(GameCompleteType gameCompleteType) {
        PlayerPrefs.SetInt(Constants.instance.HIGHEST_CLEAR_TYPE_KEY, (int) gameCompleteType);
    }
}
