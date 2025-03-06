using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    [SerializeField] List<GameObject> towers = new List<GameObject>();  

    public void AddTower(GameObject tower)
    {
        towers.Add(tower);
    }

    public void RemoveTower(GameObject tower)
    {
        towers.Remove(tower);
    }
}
