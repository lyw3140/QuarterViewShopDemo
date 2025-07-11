using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public int loopCount;
    public int currentGold;
    public string lastScene;
}

public static class LoopData
{
    public static int loopCount = 0;
    public static int currentGold = 0;
    public static string lastScene = "BaseScene";

    public static void Save()
    {
        GameData data = new GameData
        {
            loopCount = loopCount,
            currentGold = currentGold,
            lastScene = SceneManager.GetActiveScene().name
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("GameData", json);
        PlayerPrefs.Save();
        Debug.Log("✅ 게임 저장 완료");
    }

    public static void Load()
    {
        if (!PlayerPrefs.HasKey("GameData")) return;

        string json = PlayerPrefs.GetString("GameData");
        GameData data = JsonUtility.FromJson<GameData>(json);

        loopCount = data.loopCount;
        currentGold = data.currentGold;
        lastScene = data.lastScene;
        Debug.Log("📥 게임 불러오기 완료");
    }
}
