using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [Header("Pool")]
    [SerializeField] List<GameObject> poolEnemyTest = new List<GameObject>();
    [SerializeField] List<GameObject> poolTowerWater = new List<GameObject>();
    [SerializeField] List<GameObject> poolTowerEarth = new List<GameObject>();

    [Header("Prefabs")]
    [SerializeField] GameObject PrefabsEnemyTest;
    [SerializeField] GameObject PrefabsTower;

    [Header("Data")]
    [SerializeField] EnemyData enemyData;
    [SerializeField] TowerData dataTowerWater;
    [SerializeField] TowerData dataTowerEarth;

    [Header("Contain")]
    [SerializeField] Transform ContainerEnemyTest;
    [SerializeField] Transform containerTowerWater;
    [SerializeField] Transform containerTowerEarth;

    public void OnStart()
    {
        FillPool();
    }

    public void OnUpdate()
    {

    }

    private void FillPool()
    {
        for (int i = 0; i < 10; i++)
        {
            CreateEnemyTest();

            CreateTowerWater();
            CreateTowerEarth();
        }
    }

    #region Instantiate Method
    GameObject CreateEnemyTest()
    {
        //GameObject objInstance = null;
        var objInstance = Instantiate(PrefabsEnemyTest, ContainerEnemyTest);
        objInstance.gameObject.SetActive(false);
        //objInstance.GetComponent<EnemyStat>().Init(enemyData); don't init enemydata on empty pool objects
        poolEnemyTest.Add(objInstance);
        return objInstance;
    }

    GameObject CreateTowerWater()
    {
        //GameObject objInstance = null;
        var objInstance = Instantiate(PrefabsTower, containerTowerWater);
        objInstance.GetComponent<TowerStat>().Init(dataTowerWater);
        objInstance.gameObject.SetActive(false);
        poolTowerWater.Add(objInstance);
        return objInstance;
    }
    GameObject CreateTowerEarth()
    {
        //GameObject objInstance = null;
        var objInstance = Instantiate(PrefabsTower, containerTowerEarth);
        objInstance.GetComponent<TowerStat>().Init(dataTowerEarth);
        objInstance.gameObject.SetActive(false);
        poolTowerEarth.Add(objInstance);
        return objInstance;
    }
    #endregion

    #region GetObjFromPool
    public GameObject GetPoolObject(OBJ_TYPE type)
    {
        if(type == OBJ_TYPE.enemyTest)
        {
            GameObject enemyTest = poolEnemyTest.Find(x => !x.gameObject.activeSelf);
            if (enemyTest == null)
            {
                enemyTest = CreateEnemyTest();
                EnemyManager.Instance.AddEnemy(enemyTest);
            }
            else
                EnemyManager.Instance.AddEnemy(enemyTest);
            return enemyTest;
        }

        if (type == OBJ_TYPE.tower_water)
        {
            GameObject towerWater = poolTowerWater.Find(x => !x.gameObject.activeSelf);
            if (towerWater == null)
            {
                towerWater = CreateEnemyTest();
                TowerManager.Instance.AddTower(towerWater);
            }
            else
                TowerManager.Instance.AddTower(towerWater);
            return towerWater;
        }

        if (type == OBJ_TYPE.tower_earth)
        {
            GameObject towerEarth = poolTowerEarth.Find(x => !x.gameObject.activeSelf);
            if (towerEarth == null)
            {
                towerEarth = CreateEnemyTest();
                TowerManager.Instance.AddTower(towerEarth);
            }
            else
                TowerManager.Instance.AddTower(towerEarth);
            return towerEarth;
        }

        return null;
    }
    #endregion

    #region Respawn OBJ
    public void RespawnObject(OBJ_TYPE type, GameObject objSpawn)
    {
        if (type == OBJ_TYPE.enemyTest)
        {
            GameObject enemyTest = objSpawn;
            enemyTest.gameObject.SetActive(false);
            EnemyManager.Instance.RemoveEnemy(enemyTest);
        }
        if (type == OBJ_TYPE.tower_water)
        {
            GameObject towerWater = objSpawn;
            towerWater.gameObject.SetActive(false);
            TowerManager.Instance.RemoveTower(towerWater);
        }
        if (type == OBJ_TYPE.tower_earth)
        {
            GameObject towerEarth = objSpawn;
            towerEarth.gameObject.SetActive(false);
            TowerManager.Instance.RemoveTower(towerEarth);
        }
    }

    #endregion
}

public enum OBJ_TYPE
{
    enemyTest, 
    tower_water,
    tower_earth,
}