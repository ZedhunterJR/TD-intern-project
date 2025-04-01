using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField] private List<EnemyData> availableEnemies = new List<EnemyData>();
    [SerializeField] private List<EnemyData> availableBoss = new List<EnemyData>();
    public int currentWave = 0;
    [SerializeField] private int currentWavePower = 5;
    private List<EnemyData> currentWaveEnemies = new();
    private bool isSpawning = false;
    private float spawnInterval = 0;
    private int currentWaveEnemiesIndex = 0;

    float waveInterval = 0f;
    private void SpawnTestWave()
    {
        if (spawnInterval > 0)
            spawnInterval -= Time.deltaTime;
        else
        {
            SpawnEnemy(currentWaveEnemies[currentWaveEnemiesIndex]);
            currentWaveEnemiesIndex++;
            if (currentWaveEnemiesIndex >= currentWaveEnemies.Count)
            {
                isSpawning = false;
                waveInterval = 10f;

            }
            spawnInterval = 2f;
        }
    }
    private void Update()
    {
        if (isSpawning)
            SpawnTestWave();

        if (waveInterval > 0)
        {
            waveInterval -= Time.deltaTime;
        }
        else if (!isSpawning)
        {
            Debug.Log($"Wave hiện tại là {currentWave}");
            currentWaveEnemiesIndex = 0;
            isSpawning = true;
            currentWave++;
            currentWavePower = Mathf.RoundToInt(5 * Mathf.Pow(1.2f, currentWave));

            if (currentWave % 2 == 0) // Boss xuất hiện mỗi 5 wave
            {
                EnemyData bossData = GetBoss(currentWave);
                currentWaveEnemies = new List<EnemyData>() { bossData };
                UIManager.Instance.ActiveEventPanel();
            }
            else
            {
                currentWaveEnemies = new(GenerateWave(currentWavePower, availableEnemies));
            }

            UIManager.Instance.UpdateWaveDetailText(currentWave);
            GameManager.Instance.ModifyGold(50);
        }
    }
    void SpawnEnemy(EnemyData data)
    {
        float mul = 1 + (float)currentWave / 10f;
        //Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = PoolManager.Instance.GetEnemyFromPool();
        enemy.GetComponent<EnemyStat>().Init(data, mul); //temporary
        enemy.SetActive(true);
       
        if (EnemyLibrary.Instance.EnemySpawnListener(data))
        {
            var dataLib = Resources.Load<EnemyLibraryData>($"EnemyUnlock/{data.name}");
            EnemyMessageManager.Instance.ShowStatusMessage(dataLib);
        }

        //Debug.Log($"Spawned: {enemyName}");
    }

    public List<EnemyData> GenerateWave(int totalWavePower, List<EnemyData> enemyTypes)
    {
        List<EnemyData> wave = new List<EnemyData>();
        int currentPower = 0;

        // Randomize order to ensure variety in waves
        List<EnemyData> shuffledEnemies = new List<EnemyData>(enemyTypes);
        shuffledEnemies = shuffledEnemies.OrderBy(e => UnityEngine.Random.value).ToList();

        while (currentPower < totalWavePower)
        {
            // Pick a random enemy that fits within the remaining power
            EnemyData selectedEnemy = shuffledEnemies
                .Where(e => currentPower + e.wavePower <= totalWavePower)
                .OrderBy(_ => UnityEngine.Random.value) // Randomize selection
                .FirstOrDefault();

            if (selectedEnemy == null)
                break; // No valid enemy left, exit loop

            wave.Add(selectedEnemy);
            currentPower += selectedEnemy.wavePower;
        }

        return wave;
    }

    public EnemyData GetBoss(int currentWave)
    {
        if (availableBoss == null || availableBoss.Count == 0)
            return null; // Tránh lỗi nếu không có boss

        // Chọn Boss ngẫu nhiên từ danh sách Boss có sẵn
        EnemyData originalBoss = availableBoss.GetRandom();
        return originalBoss;
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
    public int level;
    public int count;
    public float spawnInterval;
    public int pathId;
    public float waveInterval;

    public EnemyItem(string enemyName, int level, int count, float spawnInterval, int pathId, float waveInterval)
    {
        this.enemyName = enemyName;
        this.level = level;
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