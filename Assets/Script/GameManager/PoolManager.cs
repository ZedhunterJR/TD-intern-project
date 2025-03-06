using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [Header("Pool")]
    [SerializeField] List<GameObject> poolEnemyTest = new List<GameObject>();

    [Header("Prefabs")]
    [SerializeField] GameObject PrefabsEnemyTest;

    [Header("Data")]
    [SerializeField] EnemyData enemyData;

    [Header("Contain")]
    [SerializeField] Transform ContainerEnemyTest;

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
    }

    #endregion
}

public enum OBJ_TYPE
{
    enemyTest, 
}