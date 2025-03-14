using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEarth : TowerAttack
{
    protected override void OnAwake()
    {
        var res = Resources.Load<GameObject>("Prefab/projectile_object");
        var proj = Instantiate(res);
        proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.2f, 0.2f);
        PoolManager.Instance.RegisterProjectilePool(proj, "Earth_thrower_bullet", 5, "sniper_earth_proj");

    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
        var instance = PoolManager.Instance.GetProjectileFromPool("sniper_earth_proj");
        instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
        TowerBehaviorLibrary.Instance.ProjectileStraightNoHitbox(instance, target, speed:12f);
        instance.SetActive(true);
        var projSc = instance.GetComponent<ProjectileAdvanced>();
        projSc.PreDestruct = () =>
        {
            DealDmg(target, projSc.transform.position);
            PoolManager.Instance.ReturnProjectileToPool(instance);
        };
    }
}
