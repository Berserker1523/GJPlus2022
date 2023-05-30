using Kitchen;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager 
{
    public static void SavePlayerData(StarsData starsData)
    {
        GameData gameData = new GameData(starsData);
        string dataPath = Application.persistentDataPath + "/kumuData29_05.save";
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, gameData);
        fileStream.Close();
    }

    public static GameData LoadStarsData()
    {
        string dataPath = Application.persistentDataPath + "/kumuData29_05.save";
        if(File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GameData starsData = (GameData) binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return starsData;
        }
        else
        {
            return new GameData();
        }
    }
}
                                                