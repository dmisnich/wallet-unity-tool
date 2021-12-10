namespace wallet_unity_tool.Runtime.Saves.API
{
    public interface ISaveService
    {
        void Save(object data, string key);
        bool TryToLoad<T>(string key, out T data);
    }
}