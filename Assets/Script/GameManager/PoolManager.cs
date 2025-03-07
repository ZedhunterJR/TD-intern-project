using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [Header("Pool")]
    [SerializeField] List<GameObject> poolEnemyTest = new List<GameObject>();
    [SerializeField] List<GameObject> poolTower = new List<GameObject>();
    [SerializeField] List<GameObject> poolTowerEarth = new List<GameObject>();

    [Header("Prefabs")]
    [SerializeField] GameObject PrefabsEnemyTest;
    [SerializeField] GameObject PrefabsTower;

    [Header("Data")]
    [SerializeField] EnemyData enemyData;
    [SerializeField] TowerData dataTowerWater;
    [SerializeField] TowerData dataTowerEarth;

    List<TowerData> allTowerDatas = new();
    Dictionary<string, TowerData> allTowersDict = new();

    [Header("Contain")]
    [SerializeField] Transform ContainerEnemyTest;
    [SerializeField] Transform containerTowerWater;
    [SerializeField] Transform containerTowerEarth;

    public void OnStart()
    {
        FillPool();

        foreach (var item in allTowerDatas)
        {
            allTowersDict.Add(item.name, item);
        }
        //var test = allTowersDIct["sentry_earth"];
    }

    public void OnUpdate()
    {

    }

    private void FillPool()
    {
        for (int i = 0; i < 10; i++)
        {
            CreateEnemyTest();

            CreateTower(containerTowerWater);
            CreateTower(containerTowerEarth);
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

    GameObject CreateTower(Transform container)
    {
        //GameObject objInstance = null;
        var objInstance = Instantiate(PrefabsTower, container);
        //objInstance.GetComponent<TowerStat>().Init(data);
        objInstance.gameObject.SetActive(false);
        poolTower.Add(objInstance);
        return objInstance;
    }
    #endregion

    #region GetObjFromPool
    public GameObject GetEnemyFromPool()
    {

        GameObject enemyTest = poolEnemyTest.Find(x => !x.gameObject.activeSelf);
        if (enemyTest == null)
        {
            enemyTest = CreateEnemyTest();
            //   EnemyManager.Instance.AddEnemy(enemyTest);
            GetEnemyFromPool();
        }
        else
            EnemyManager.Instance.AddEnemy(enemyTest);
        return enemyTest;
    }

    public GameObject GetTowerFromPool()
    {
        GameObject towerWater = poolTower.Find(x => !x.gameObject.activeSelf);
        if (towerWater == null)
        {
            towerWater = CreateTower(containerTowerWater);
            //TowerManager.Instance.AddTower(towerWater);
            return GetTowerFromPool();
        }
        else
            TowerManager.Instance.AddTower(towerWater);
        return towerWater;
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