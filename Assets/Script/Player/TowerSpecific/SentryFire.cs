using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryFire : TowerAttack
{
    protected override void OnAwake()
    {
        var res = Resources.Load<GameObject>("Prefab/projectile_object");
        var proj = Instantiate(res);
        proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.3f, 0.3f);
        InitPool(proj, "Fire_thrower_bullet", 5);
    }
    protected override GameObject GetTarget()
    {
        return range.StrongTarget();
    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
        var instance = GetFromPool();
        ProjectileLibrary.Instance.ProjectileLob(instance, target, lifeSpan:0.7f);
        instance.SetActive(true);
        var projSc = instance.GetComponent<ProjectileAdvanced>();
        projSc.PreDestruct = () =>
        {
            if (!projSc.AllEnemies.Contains(target))
                target.GetComponent<EnemyStat>().PreMitiDmg(stat.data.baseDamage);
            ReturnToPool(instance);
        };
    }
}
