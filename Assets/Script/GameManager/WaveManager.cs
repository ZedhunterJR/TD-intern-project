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
    public int currentWave = 0;
    [SerializeField ]private int currentWavePower = 5;
    private List<EnemyData> currentWaveEnemies = new();
    private bool isSpawning = false;
    private float spawnInterval = 0;
    private int currentWaveEnemiesIndex = 0;
    private void SpawnTestWave()
    {
        if (spawnInterval > 0) 
            spawnInterval -= Time.deltaTime;
        else
        {
            SpawnEnemy(currentWaveEnemies[currentWaveEnemiesIndex]);
            currentWaveEnemiesIndex++;
            if (currentWaveEnemiesIndex >= currentWaveEnemies.Count)
                isSpawning = false;
            spawnInterval = 2f;
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (!isSpawning)
            {
                currentWaveEnemies = new(GenerateWave(currentWavePower, availableEnemies));
                currentWaveEnemiesIndex = 0;
                isSpawning = true;
                currentWave++;
                UIManager.Instance.UpdateWaveDetailText(currentWave);

                if (currentWave != 0 && currentWave % 5 == 0)
                {
                    UIManager.Instance.ActiveEventPanel();
                }
            }
        }

        if (isSpawning) 
            SpawnTestWave();
    }
    void SpawnEnemy(EnemyData data)
    { 
        //Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = PoolManager.Instance.GetEnemyFromPool();
        enemy.GetComponent<EnemyStat>().Init(data); //temporary
        enemy.SetActive(true);

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