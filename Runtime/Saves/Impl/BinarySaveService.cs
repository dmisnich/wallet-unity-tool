using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using wallet_unity_tool.Runtime.Saves.API;

namespace wallet_unity_tool.Runtime.Saves.Impl
{
    public class BinarySaveService : ISaveService
    {
        private readonly string _savePathString = Application.persistentDataPath + "/Saves";
        private const string Extension = ".dat";
        
        public void Save(object data, string key)
        {
            var path = Path.Combine(_savePathString, key + Extension);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(path);
                     
            formatter.Serialize(file, data);
            file.Close();
            Debug.Log($"Saved: {key}");
        }

        public bool TryToLoad<T>(string key, out T data)
        {
            var path = Path.Combine(_savePathString, key + Extension);
            data = default;
            
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);
                data = (T)formatter.Deserialize(file);
                file.Close();
                Debug.Log($"Loaded: {key}");
                return true;
            }

            Debug.Log($"File doesn't exists: {key}");
            return false;
        }
    }
}