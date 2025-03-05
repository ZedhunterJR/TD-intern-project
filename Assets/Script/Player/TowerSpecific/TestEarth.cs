using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEarth : TowerAttack
{
    private GameObject projectile;
    protected override void OnAwake()
    {
        projectile = Resources.Load<GameObject>("Prefab/projectile");
    }
    protected override GameObject GetTarget()
    {
        return range.LastTarget();
    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
        var instance = GameObject.Instantiate(projectile, transform.position, Quaternion.identity);
        ProjectileLibrary.Instance.ProjectileLob(instance, target);
        instance.GetComponent<ProjectileAdvanced>().PreDestruct += () =>
        {
            if (target != null)
                target.GetComponent<EnemyStat>().PreMitiDmg(stat.data.baseDamage);
        };
    }
}
