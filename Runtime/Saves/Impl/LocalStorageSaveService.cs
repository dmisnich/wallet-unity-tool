using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using wallet_unity_tool.Runtime.Saves.API;

namespace wallet_unity_tool.Runtime.Saves.Impl
{
    public class LocalStorageSaveService : ISaveService
    {
        private readonly string _savePathString = Application.persistentDataPath + "/Saves";
        
        public void Save(object data, string key)
        {
            var path = Path.Combine(_savePathString, key);
            var save = JsonConvert.SerializeObject(data);
            
            if (Path.GetDirectoryName(path) is var directoryPath && !string.IsNullOrEmpty(directoryPath))
                Directory.CreateDirectory(directoryPath);
			
            File.WriteAllText(path, save);
            Debug.Log($"Saved: {key}");
        }

        public bool TryToLoad<T>(string key, out T data)
        {
            var path = Path.Combine(_savePathString, key);
            data = default;
            
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                data =  JsonConvert.DeserializeObject<T>(text);
                Debug.Log($"Loaded: {key}");
                return true;
            }

            Debug.Log($"File doesn't exists: {key}");
            return false;
        }
    }
}