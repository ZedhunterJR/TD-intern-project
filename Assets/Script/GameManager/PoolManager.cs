using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [Header("Pool")]
    [SerializeField] List<GameObject> poolEnemyTest = new List<GameObject>();
    [SerializeField] List<GameObject> poolTower = new List<GameObject>();
    private Dictionary<string, List<GameObject>> projectilePool = new();

    [Header("Prefabs")]
    [SerializeField] GameObject PrefabsEnemyTest;
    [SerializeField] GameObject PrefabsTower;

    [Header("Data")]
    [SerializeField] List<TowerData> allTowerDatas = new();
    Dictionary<string, TowerData> allTowersDict = new();

    [Header("Contain")]
    [SerializeField] Transform ContainerEnemyTest;
    [SerializeField] Transform containerTower;
    [SerializeField] Transform containerProjectile;

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
        for (int i = 0; i < 20; i++)
        {
            CreateEnemyTest();

            CreateTower(containerTower);
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
            towerWater = CreateTower(containerTower);
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

    #region Projectile pooling
    public void RegisterProjectilePool(GameObject projectile, string spineAniType, int count, string key)
    {
        // Check if key exists, if not, add a new list
        if (!projectilePool.TryGetValue(key, out List<GameObject> pool))
        {
            pool = new List<GameObject>();
            projectilePool[key] = pool;
        }

        // Add new projectiles to the pool
        for (int i = 0; i < count; i++)
        {
            var item = Instantiate(projectile, containerProjectile);
            item.GetComponentInChildren<SpineAnimationController>().PlayAnimation(spineAniType);
            item.SetActive(false);
            pool.Add(item);
        }

        Destroy(projectile);
    }

    public GameObject GetProjectileFromPool(string key)
    {
        GameObject projGet = projectilePool[key].Find(x => !x.activeSelf);
        //projGet.transform.position = transform.position;
        TowerManager.Instance.AddProjectile(projGet);
        //projGet.SetActive(true);
        return projGet;
    }
    public void ReturnProjectileToPool(GameObject projectile)
    {
        TowerManager.Instance.RemoveProjectile(projectile);
        projectile.SetActive(false);
    }
    #endregion
}

public enum OBJ_TYPE
{
    enemyTest,
    tower_water,
    tower_earth,
}