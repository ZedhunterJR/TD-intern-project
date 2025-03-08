using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryIce : TowerAttack
{
    protected override void OnAwake()
    {
        var res = Resources.Load<GameObject>("Prefab/projectile_object");
        var proj = Instantiate(res);
        proj.GetComponent<ProjectileAdvanced>().speed = 15f;
        proj.GetComponent<ProjectileAdvanced>().pierce = 5;
        InitPool(proj, "Ice_thrower_bullet", 5);
    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
        var instance = GetFromPool();
        ProjectileLibrary.Instance.ProjectileStraight(instance, target.transform.position - transform.position, lifeSpan: 1f);
        instance.SetActive(true);
        var sc = instance.GetComponent<ProjectileAdvanced>();
        sc.AllEnemies = EnemyManager.Instance.AllEnemies;
        sc.HitEvent = (target) =>
        {
            //print("??");
            target.GetComponent<EnemyStat>().PreMitiDmg(stat.data.baseDamage);
        };
        sc.PreDestruct = () => ReturnToPool(instance);
    }
}
