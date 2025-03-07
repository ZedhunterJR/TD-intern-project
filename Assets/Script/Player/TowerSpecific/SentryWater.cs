using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryWater : TowerAttack
{
    protected override void OnAwake()
    {
        var res = Resources.Load<GameObject>("Prefab/projectile_object");
        var proj = Instantiate(res);
        InitPool(proj, "Water_thrower_bullet", 5);
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
        ProjectileLibrary.Instance.ProjectileStraightNoHitbox(instance, target);
        instance.SetActive(true);
        instance.GetComponent<ProjectileAdvanced>().PreDestruct += () =>
        {
            if (target != null || !target.activeSelf)
                target.GetComponent<EnemyStat>().PreMitiDmg(stat.data.baseDamage);
            ReturnToPool(instance);
        };
    }
}
