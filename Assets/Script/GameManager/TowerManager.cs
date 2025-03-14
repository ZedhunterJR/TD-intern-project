using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    [SerializeField] List<GameObject> towers = new List<GameObject>();  
    [SerializeField] List<GameObject> projectiles = new List<GameObject>();

    public void AddTower(GameObject tower)
    {
        towers.Add(tower);
    }

    public void RemoveTower(GameObject tower)
    {
        towers.Remove(tower);
    }
    public void OnUpdate()
    {
        foreach (GameObject tower in new List<GameObject>(towers))
        {
            tower.GetComponent<TowerAttack>().OnUpdate();
        }
        foreach (GameObject proj in new List<GameObject>(projectiles))
        {
            if (proj.TryGetComponent<ProjectileAdvanced>(out var projA))
            {
                projA.OnUpdate();
                continue;
            }
            if (proj.TryGetComponent<ExplosionAdvanced>(out var explosA))
            {
                explosA.OnUpdate(); 
            }
        }
    }

    public void AddProjectile(GameObject proj)
    {
        projectiles.Add(proj);
    }
    public void RemoveProjectile(GameObject proj) 
    { 
        projectiles.Remove(proj); 
    }
}
