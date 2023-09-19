using Kitchen;
using System.IO;
using UnityEngine;

public static class SaveManager 
{
    public static readonly string path = "/kumuData29_05.json";
    public static void SavePlayerData(GameData starsData)
    {

        string dataPath = Application.persistentDataPath + path;
        string json = JsonUtility.ToJson(starsData);
        File.WriteAllText(dataPath, json);
    }

    public static GameData LoadStarsData()
    {
        string dataPath = Application.persistentDataPath + path;
        if(File.Exists(dataPath))
        {

            string json = File.ReadAllText(dataPath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            return data;
        }
        else
        {
            return new GameData();
        }
    }
}                                                