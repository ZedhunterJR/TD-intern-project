using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    List<Wave> waves;
    List<EnemyData> enemiesData = new List<EnemyData>();
    Dictionary<string, EnemyData> enemiesDict = new Dictionary<string, EnemyData>();

    public void OnAwake()
    {
        enemiesData.AddRange(Resources.LoadAll<EnemyData>("EnemyData"));
        LoadEnemyDataFromListToDict();
    }

    void LoadEnemyDataFromListToDict()
    {
        foreach (EnemyData e in enemiesData)
        {
            enemiesDict.Add(e.name, e);
        }
    }
}

[System.Serializable]
public class Wave
{
    public int waveIndex; 
    public float startTime;
    public List<EnemyItem> enemyGroup; 
}

[System.Serializable]
public class EnemyItem
{
    public string enemyName;
    public int count;
    public float spawnInterval;
    public int pathID;
    public float waveInterval;
}
