using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    //List<Wave> waves;
    [SerializeField]
    List<EnemyData> enemiesData = new List<EnemyData>();
    Dictionary<string, EnemyData> enemiesDict = new Dictionary<string, EnemyData>();

    // Wave Attributes 
    WaveList waveList;
    int currentWave = 0;
    bool isSpawning = false;
    float waveTimer = 0f;
    Wave waveTemp;
    //float spawnTimer = 0f;
    //int enemySpawned = 0;
    //int currentEnemyIndex = 0;

    //New Wave Attibutes
    List<float> spawnTimers = new List<float>();
    List<int> enemiesSpawned = new List<int>();

    //[SerializeField] Transform[] spawnPoints; // Điểm spawn enemy 
    //Not neccesary, enemy already spawn at the PathWaypoint[0]

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
        LoadDataFromJsonToList();

        waveTemp = waveList.waves[currentWave];
        waveTimer = waveTemp.startTime;
    }

    public void OnUpdate()
    {
        // Nếu mà không còn wave trong Level thì return
        if (currentWave >= waveList.waves.Count) return;

        if (!isSpawning)
        {
            waveTimer -= Time.deltaTime;
            if (waveTimer <= 0)
            {
                StartWave();
            }
        }
        else
        {
            SpawnEnemies();
        }
    }

    #region Wave Spawner Methods
    void StartWave()
    {
        isSpawning = true;
        //enemySpawned = 0;
        //currentEnemyIndex = 0;
        waveTemp = waveList.waves[currentWave];

        spawnTimers.Clear();
        enemiesSpawned.Clear();

        foreach (EnemyItem enemy in waveTemp.enemyGroup)
        {
            spawnTimers.Add(enemy.waveInterval);
            enemiesSpawned.Add(0);
        }

        Debug.Log($"Bắt đầu Wave {waveTemp.waveIndex}");
    }

    void EndWave()
    {
        isSpawning = false;
        currentWave++;
        if (currentWave < waveList.waves.Count)
        {
            waveTimer = waveList.waves[currentWave].startTime;
            Debug.Log($"Wave {currentWave} đã kết thúc. Chờ {waveTimer}s để wave mới bắt đầu.");
        }
        else
            Debug.Log("Tất cả waves đã kết thúc!");
    }

    void SpawnEnemies()
    {
        bool allEnemiesSpawned = true;

        for (int i = 0; i < waveTemp.enemyGroup.Count; i++)
        {
            EnemyItem enemyItem = waveTemp.enemyGroup[i];

            if (enemiesSpawned[i] < enemyItem.count)
            {
                allEnemiesSpawned = false;
                spawnTimers[i] -= Time.deltaTime;

                if (spawnTimers[i] <= 0)
                {
                    SpawnEnemy(enemyItem.enemyName);
                    enemiesSpawned[i]++;
                    spawnTimers[i] = enemyItem.spawnInterval;
                }
            }
        }

        if (allEnemiesSpawned)
        {
            EndWave();
        }


        /*
        if (currentEnemyIndex >= waveTemp.enemyGroup.Count)
        {
            EndWave();
            return;
        } 
         
        EnemyItem enemyItem = waveTemp.enemyGroup[currentEnemyIndex];
         
        if (enemySpawned >= enemyItem.count)
        {
            currentEnemyIndex++;
            enemySpawned = 0;
            return;
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnEnemy(enemyItem.enemyName);
            enemySpawned++;
            spawnTimer = enemyItem.spawnInterval;
        }
        */
    }

    void SpawnEnemy(string enemyName)
    {
        if (!enemiesDict.ContainsKey(enemyName))
        {
            Debug.Log($"Không tìm thấy enemyData có tên là {enemyName}");
            return;
        }

        //Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = PoolManager.Instance.GetEnemyFromPool();
        enemy.GetComponent<EnemyStat>().Init(enemiesDict[enemyName]);
        enemy.SetActive(true);

        Debug.Log($"Spawned: {enemyName}");
    }
    #endregion

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
    // ok cool
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

        WaveList waveListJson = JsonUtility.FromJson<WaveList>(json);
        waveList = waveListJson;

        /*
        //foreach (Wave wave in waveList.waves) Đã test và hoạt động thành công 
        //{
        //    Debug.Log($"Đây là wave thứ {wave.waveIndex} = Thời gian bắt đầu {wave.startTime} = EnemyGruop là {wave.enemyGroup}");

        //    foreach (var enemy in wave.enemyGroup)
        //    {
        //        Debug.Log($"Data {enemy.enemyName} = {enemy.count} = {enemy.spawnInterval} = {enemy.waveInterval}");
        //    }
        //}
        */ // Dữ liệu lấy ra từ Json là đúng đắn 
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