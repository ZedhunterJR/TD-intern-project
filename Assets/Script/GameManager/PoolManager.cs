using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [Header("Pool")]
    [SerializeField] Queue<GameObject> poolEnemyTest = new();
    [SerializeField] Queue<GameObject> poolTower = new();
    private Dictionary<string, Queue<GameObject>> projectilePool = new();

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

    private void Start()
    {
        FillPool();

        foreach (var item in allTowerDatas)
        {
            allTowersDict.Add(item.name, item);
        }
        //var test = allTowersDIct["sentry_earth"];
    }

    private void FillPool()
    {
        for (int i = 0; i < 5; i++)
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
        poolEnemyTest.Enqueue(objInstance);
        return objInstance;
    }

    GameObject CreateTower(Transform container)
    {
        //GameObject objInstance = null;
        var objInstance = Instantiate(PrefabsTower, container);
        //objInstance.GetComponent<TowerStat>().Init(data);
        objInstance.gameObject.SetActive(false);
        poolTower.Enqueue(objInstance);
        return objInstance;
    }
    #endregion

    #region GetObjFromPool
    public GameObject GetEnemyFromPool()
    {
        if (poolEnemyTest.Count == 0)
        {
            CreateEnemyTest();
            //   EnemyManager.Instance.AddEnemy(enemyTest);
            return GetEnemyFromPool();
        }
        GameObject enemyTest = poolEnemyTest.Dequeue();
        EnemyManager.Instance.AddEnemy(enemyTest);
        return enemyTest;
    }

    public GameObject GetTowerFromPool()
    {
        if (poolTower.Count == 0)
        {
            CreateTower(containerTower);
            //TowerManager.Instance.AddTower(towerWater);
            return GetTowerFromPool();
        }

        GameObject towerWater = poolTower.Dequeue();
        TowerManager.Instance.AddTower(towerWater);
        return towerWater;
    }
    #endregion

    #region Return OBJ
    public void ReturnTower(GameObject tower)
    {
        tower.SetActive(false);
        poolTower.Enqueue(tower);
        TowerManager.Instance.RemoveTower(tower);
    }
    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        poolEnemyTest.Enqueue(enemy);
        EnemyManager.Instance.RemoveEnemy(enemy);
    }

    #endregion

    #region Projectile pooling
    public void RegisterProjectilePool(GameObject projectile, string spineAniType, int count, string key)
    {
        // Check if key exists, if not, add a new list
        if (!projectilePool.TryGetValue(key, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            projectilePool[key] = pool;
        }

        // Add new projectiles to the pool
        for (int i = 0; i < count; i++)
        {
            var item = Instantiate(projectile, containerProjectile);
            item.GetComponentInChildren<SpineAnimationController>().PlayAnimation(spineAniType);
            item.SetActive(false);
            pool.Enqueue(item);
        }

        Destroy(projectile);
    }

    public GameObject GetProjectileFromPool(string key)
    {
        if (projectilePool[key].Count == 0)
        {
            return null;
        }
        GameObject projGet = projectilePool[key].Dequeue();
        //projGet.transform.position = transform.position;
        TowerManager.Instance.AddProjectile(projGet);
        //projGet.SetActive(true);
        return projGet;
    }
    public void ReturnProjectileToPool(GameObject projectile, string key)
    {
        TowerManager.Instance.RemoveProjectile(projectile);
        projectilePool[key].Enqueue(projectile);
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