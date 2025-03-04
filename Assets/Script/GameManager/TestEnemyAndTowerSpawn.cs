using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAndTowerSpawn : MonoBehaviour
{
    public static TestEnemyAndTowerSpawn Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        towerPrefab = Resources.Load<GameObject>("Prefab/tower_prefab");
        enemyPrefab = Resources.Load<GameObject>("Prefab/enemy_template");
    }

    private GameObject towerPrefab;
    private GameObject enemyPrefab;

    public TowerData towerData;
    public EnemyData enemyData;
    public Transform spawnTowerSpot;

    public List<GameObject> AllEnemies = new();

    [ContextMenu("Spawn tower")]
    public void SpawnTower()
    {
        GameObject instance = Instantiate(towerPrefab, spawnTowerSpot.position, Quaternion.identity);
        instance.GetComponent<TowerStat>().Init(towerData);
    }
    [ContextMenu("Spawn enemy")]
    public void SpawnEnemy()
    {
        GameObject instance = Instantiate(enemyPrefab);
        instance.GetComponent<EnemyStat>().Init(enemyData);
        AllEnemies.Add(instance);
    }
}
