using UnityEngine;
using Newtonsoft.Json;
public abstract class JsonReaderBase<T> : ScriptableObject where T : new()
{
    public T Data;

    public T LoadData(string jsonFileName)
    {
        if (string.IsNullOrEmpty(jsonFileName))
        {
            Debug.LogError("JSON file name is not set!");
            return Data;
        }

        T _loadedData = default(T);

        // Load the TextAsset from Resources folder
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile != null)
        {
            string jsonString = jsonFile.text;
            //loadedData = JsonUtility.FromJson<T>(jsonString);
            _loadedData = JsonConvert.DeserializeObject<T>(jsonString);
        }
        else
        {
            Debug.LogError("JSON file not found with name: " + jsonFileName);
        }

        Data = _loadedData;
        return Data;
    }
}
