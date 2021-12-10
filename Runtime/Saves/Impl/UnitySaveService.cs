using Newtonsoft.Json;
using UnityEngine;
using wallet_unity_tool.Runtime.Saves.API;

namespace wallet_unity_tool.Runtime.Saves.Impl
{
    public class UnitySaveService : ISaveService
    {
        public void Save(object data, string key)
        {
            var save = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(key, save);
            PlayerPrefs.Save();
            Debug.Log($"Saved: {key}");
        }

        public bool TryToLoad<T>(string key, out T data)
        {
            var text = PlayerPrefs.GetString(key);
            data = default;
            
            if (!string.IsNullOrEmpty(text))
            {
                data = JsonConvert.DeserializeObject<T>(text);
                Debug.Log($"Loaded: {key}");
                return true;
            }
            Debug.Log($"Saves doesn't exists: {key}");
            return false;
        }
    }
}