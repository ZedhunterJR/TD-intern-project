using System.Collections;
using System;
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
    [SerializeField]
    WaveList waveList;
    int currentWave = 0;
    bool isSpawning = false;
    float waveTimer = 0f;
    Wave waveTemp;
    /*
    //float spawnTimer = 0f;
    //int enemySpawned = 0;
    //int currentEnemyIndex = 0;
     */ // Spawn lần lượt từ trên xuống 

    //New Wave Attibutes
    List<float> spawnTimers = new List<float>();
    List<int> enemiesSpawned = new List<int>();

    //[SerializeField] Transform[] spawnPoints; // Điểm spawn enemy 
    //Not neccesary, enemy already spawn at the PathWaypoint[0]

    // Path của file json waveData
    private string path;

    // Đọc dữ liệu từ file CSV 
    [SerializeField] TextAsset textAssetData; 

    public void OnAwake()
    {
        enemiesData.AddRange(Resources.LoadAll<EnemyData>("EnemyData"));
        LoadEnemyDataFromListToDict();
    }

    public void OnStart()
    {
        //SetPath();
        //LoadDataFromJsonToList();
        LoadDataFromCSVToList();

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
        /*
        //enemySpawned = 0;
        //currentEnemyIndex = 0;
         */ // Spawn lần lượt từ trên xuống 
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
        */ // Spawn lần lượt từ trên xuống 
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

    public void LoadDataFromCSVToList()
    {
        if (textAssetData == null)
        {
            Debug.LogError("CSV file not assigned!");
            return;
        }

        string[] lines = textAssetData.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); // Loại bỏ dòng trống
        waveList = new WaveList { waves = new List<Wave>() }; // Khởi tạo danh sách waves

        Wave currentWave = null;

        for (int i = 2; i < lines.Length; i++) // Bỏ qua 2 dòng đầu
        {
            string[] data = lines[i].Split(',');

            if (data.Length < 7) continue; // Bỏ qua dòng không hợp lệ

            if (!string.IsNullOrWhiteSpace(data[0])) // Nếu có waveIndex mới => tạo Wave mới
            {
                currentWave = new Wave
                {
                    waveIndex = int.Parse(data[0].Trim()),
                    startTime = float.Parse(data[1].Trim()),
                    enemyGroup = new List<EnemyItem>()
                };

                waveList.waves.Add(currentWave);
            }

            // Nếu là enemyGroup (có dữ liệu enemyName)
            if (!string.IsNullOrWhiteSpace(data[2]) && currentWave != null)
            {
                string enemyName = data[2].Trim();
                int count = int.TryParse(data[3].Trim(), out int c) ? c : 0; // Cố gắng trả về kiểu int nếu đúng thì return c sai thì trả về 0
                float spawnInterval = float.TryParse(data[4].Trim(), out float si) ? si : 0f;
                int pathId = int.TryParse(data[5].Trim(), out int p) ? p : 0;
                float waveInterval = float.TryParse(data[6].Trim(), out float wi) ? wi : 0f;

                currentWave.enemyGroup.Add(new EnemyItem(enemyName, count, spawnInterval, pathId, waveInterval));
            }
        }

        Debug.Log("Loaded " + waveList.waves.Count + " waves successfully!");
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

    public EnemyItem(string enemyName, int count, float spawnInterval, int pathId, float waveInterval)
    {
        this.enemyName = enemyName;
        this.count = count;
        this.spawnInterval = spawnInterval;
        this.pathId = pathId;
        this.waveInterval = waveInterval;
    }
}

[System.Serializable]
public class WaveList
{
    public List<Wave> waves;
}