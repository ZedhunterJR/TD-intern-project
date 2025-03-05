using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEnemyAndTowerSpawn : Singleton<TestEnemyAndTowerSpawn>
{
    private void Awake()
    {
        towerPrefab = Resources.Load<GameObject>("Prefab/tower_prefab");
        enemyPrefab = Resources.Load<GameObject>("Prefab/enemy_template");
    }

    private GameObject towerPrefab;
    private GameObject enemyPrefab;

    public TowerData towerData;
    public EnemyData enemyData;
    public Transform spawnTowerSpot;

    //public List<GameObject> AllEnemies = EnemyManager.Instance.AllEnemies;
    public void SpawnTower(Vector2 Position)
    {
        GameObject instance = Instantiate(towerPrefab, Position, Quaternion.identity);
        instance.GetComponent<TowerStat>().Init(towerData);
    }
    [ContextMenu("Spawn tower")]
    public void SpawnTower()
    {
        GameObject instance = Instantiate(towerPrefab, spawnTowerSpot.position, Quaternion.identity);
        instance.GetComponent<TowerStat>().Init(towerData);
    }
    [ContextMenu("Spawn enemy")]
    public void SpawnEnemy()
    {
        GameObject enemy = PoolManager.Instance.GetPoolObject(OBJ_TYPE.enemyTest);
        enemy.GetComponent<EnemyStat>().Init(enemyData);
        enemy.SetActive(true);
        

        //GameObject instance = Instantiate(enemyPrefab);
        //instance.GetComponent<EnemyStat>().Init(enemyData);
        //EnemyManager.Instance.AddEnemy(instance);
    }
}
