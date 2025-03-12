using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]
    List<GameObject> allEnemies = new List<GameObject>(); // chứa tất cả enemy 

    public List<GameObject> AllEnemies => allEnemies;

    public void OnStart()
    {

    }

    public void OnUpdate()
    {
        AllEnemiesAction();
    }

    void AllEnemiesAction()
    {
        foreach (var enemy in allEnemies.ToArray())
        {
            enemy.GetComponent<EnemyStat>().OnUpdate();
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        allEnemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        allEnemies.Remove(enemy);
    }
}
