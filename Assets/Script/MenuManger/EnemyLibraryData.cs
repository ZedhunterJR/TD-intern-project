using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class EnemyLibraryData : ScriptableObject
{
    public string enemyName;
    public int enemyHealth;
    public float enemyMoveSpeed;
    public Element element;
    public Sprite sprite;
    public string description;
}

public class EnemyLibrary
{
    private static EnemyLibrary instance;

  

    public static EnemyLibrary Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyLibrary();
            }
            return instance;
        }
    }

    public readonly string dictionaryKey = "DictionaryData";

    public Dictionary<string, int> allEnemies;

    public void SaveDictionary()
    {
        string json = JsonUtility.ToJson(new SerializableDictionary(allEnemies));
        PlayerPrefs.SetString(dictionaryKey, json);
        PlayerPrefs.Save();
    }

    public void LoadDictionary()
    {
        if (!PlayerPrefs.HasKey(dictionaryKey))
            allEnemies = new Dictionary<string, int>();

        string json = PlayerPrefs.GetString(dictionaryKey);
        SerializableDictionary serializableDict = JsonUtility.FromJson<SerializableDictionary>(json);
        allEnemies = serializableDict.ToDictionary();
    }

    [System.Serializable]
    private class SerializableDictionary
    {
        public List<string> keys = new List<string>();
        public List<int> values = new List<int>();

        public SerializableDictionary(Dictionary<string, int> dict)
        {
            foreach (var pair in dict)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public Dictionary<string, int> ToDictionary()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for (int i = 0; i < keys.Count; i++)
            {
                dict[keys[i]] = values[i];
            }
            return dict;
        }
    }

    public void EnemySpawnListener(EnemyData enemyData)
    {
        // chekc xem đã mở khóa hay chưa
        if (allEnemies.TryGetValue(enemyData.name, out var enemyValue))
        {
            if (enemyValue == 0) allEnemies[enemyData.name] = 1;
        }
    }
}
