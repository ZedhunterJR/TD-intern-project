using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperWater : TowerAttack
{
    protected override void OnAwake()
    {
        var res = Resources.Load<GameObject>("Prefab/projectile_object");
        var proj = Instantiate(res);
        proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.15f, 0.15f);
        PoolManager.Instance.RegisterProjectilePool(proj, "Water_thrower_bullet", 5, "sniper_water_proj");

    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
        var instance = PoolManager.Instance.GetProjectileFromPool("sniper_water_proj");
        instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
        TowerBehaviorLibrary.Instance.ProjectileStraightNoHitbox(instance, target, speed:12f);
        instance.SetActive(true);
        var projSc = instance.GetComponent<ProjectileAdvanced>();
        projSc.PreDestruct = () =>
        {
            //print(target);
            DealDmg(target, projSc.transform.position);
            PoolManager.Instance.ReturnProjectileToPool(instance);
        };
    }
}
