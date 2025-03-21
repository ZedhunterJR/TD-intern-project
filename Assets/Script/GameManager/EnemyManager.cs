using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]
    List<GameObject> allEnemies = new List<GameObject>(); // chứa tất cả enemy 

    public List<GameObject> AllEnemies => allEnemies;

    public void AddEnemy(GameObject enemy)
    {
        allEnemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        allEnemies.Remove(enemy);
    }

    public List<GameObject> SamePathEnemies(Vector2 pos)
    {
        var list = new List<GameObject>();
        var absPos = PathManager.Instance.GetNearestTileCenter(pos);
        foreach (var enemy in AllEnemies)
        {
            var absPosE = PathManager.Instance.GetNearestTileCenter(enemy.transform.position);
            if (absPosE == absPos)
                list.Add(enemy);
        }
        return list;
    }
}
