using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    //List<Wave> waves;
    List<EnemyData> enemiesData = new List<EnemyData>();
    Dictionary<string, EnemyData> enemiesDict = new Dictionary<string, EnemyData>();

    WaveList waveList;

    // Path của file json waveData
    private string path;

    public void OnAwake()
    {
        enemiesData.AddRange(Resources.LoadAll<EnemyData>("EnemyData"));
        LoadEnemyDataFromListToDict();
    }

    public void OnStart()
    {
        SetPath();
        //SaveData();
        LoadDataFromJsonToList();
    }

    public void OnUpdate()
    {

    }

    #region Init Methods
    void SetPath()
    {
        path = Application.streamingAssetsPath + "/waveData.json";
    }

    void LoadEnemyDataFromListToDict()
    {
        foreach (EnemyData e in enemiesData)
        {
            enemiesDict.Add(e.name, e);
        }
    }

    //public void SaveData() // lưu ý đừng dùng hàm này, nó hoạt động rồi, dùng là mất nó save waves default mất hết dữ liệu từ json 
    //{
    //    string savaPath = path;

    //    string json = JsonUtility.ToJson(waves);
    //    Debug.Log(json);

    //    using StreamWriter writer = new StreamWriter(savaPath);
    //    writer.Write(json);
    //}

    public void LoadDataFromJsonToList()
    {
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        //Debug.Log(json);

        WaveList waveListJson = JsonUtility.FromJson<WaveList>(json);
        waveList = waveListJson;

        //foreach (Wave wave in waveList.waves) Đã test và hoạt động thành công 
        //{
        //    Debug.Log($"Đây là wave thứ {wave.waveIndex} = Thời gian bắt đầu {wave.startTime} = EnemyGruop là {wave.enemyGroup}");

        //    foreach (var enemy in wave.enemyGroup)
        //    {
        //        Debug.Log($"Data {enemy.enemyName} = {enemy.count} = {enemy.spawnInterval} = {enemy.waveInterval}");
        //    }
        //}
    }
    #endregion

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
    public int pathId;
    public float waveInterval;
}

[System.Serializable]
public class WaveList
{
    public List<Wave> waves;
}