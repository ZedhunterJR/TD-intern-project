using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryWater : TowerAttack
{
    protected override void OnAwake()
    {
        var res = Resources.Load<GameObject>("Prefab/projectile_object");
        var proj = Instantiate(res);
        proj.GetComponent<ProjectileAdvanced>().speed = 5f;
        proj.GetComponent<ProjectileAdvanced>().pierce = 3;
        InitPool(proj, "Water_thrower_bullet", 24);
    }
    
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
        for (int i = 0; i < 6; i++)
        {
            var dir = Quaternion.Euler(0, 0, 60f * i) * Vector2.up;
            var instance = GetFromPool();
            ProjectileLibrary.Instance.ProjectileStraight(instance, dir, lifeSpan:0.6f);
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
}
