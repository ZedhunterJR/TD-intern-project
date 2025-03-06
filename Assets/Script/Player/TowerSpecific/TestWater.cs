using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWater : TowerAttack
{
    protected override void OnAwake()
    {
        InitPool("Water_thrower_bullet", 5);
    }
    protected override GameObject GetTarget()
    {
        return range.LastTarget();
    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
        var instance = GetFromPool();
        ProjectileLibrary.Instance.ProjectileLob(instance, target);
        instance.SetActive(true);
        instance.GetComponent<ProjectileAdvanced>().PreDestruct += () =>
        {
            if (target != null || !target.activeSelf)
                target.GetComponent<EnemyStat>().PreMitiDmg(stat.data.baseDamage);
            ReturnToPool(instance);
        };
    }
}
