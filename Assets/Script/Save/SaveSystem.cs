using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    const string SAVE_PATH = "/saves";
    
    public static void Save<T>(T obj, int slot, string folder, string key)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + SAVE_PATH + slot + folder;
        Directory.CreateDirectory(path);

        FileStream stream = new FileStream(path + key, FileMode.Create, FileAccess.ReadWrite);

        formatter.Serialize(stream, obj);
        stream.Close();
    } 

    public static T Load<T>(int slot,string folder, string key)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + SAVE_PATH + slot + folder;

        T data = default;

        if (File.Exists(path + key))
        {
            FileStream stream = new FileStream(path + key, FileMode.Open);

            data = (T)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            Debug.LogError("在路徑中找不到存檔 " + path + key);
        }

        return data;
    }
}
