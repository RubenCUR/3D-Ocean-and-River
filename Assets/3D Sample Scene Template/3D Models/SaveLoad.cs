using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad 
{
    public static void SaveData(Character character)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/New.weeklyhow";

        FileStream stream = new FileStream(path, FileMode.Create);

        CharacterData charData = new CharacterData(character);

        formatter.Serialize(stream, charData);
        stream.Close();
    }

    public static CharacterData LoadData()
    {
        string path = Application.persistentDataPath + "/New.weeklyhow";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CharacterData data = formatter.Deserialize(stream) as CharacterData;

            stream.Close();

            return data;
        } else
        {
            Debug.LogError("Error: Save file not found in " + path);
            return null;
        }
    }
}