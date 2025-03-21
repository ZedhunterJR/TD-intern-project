using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEarth : TowerAttack
{
    protected override void OnAwake()
    {
        if (!isPlaced)
        {
            var res = Resources.Load<GameObject>("Prefab/projectile_object");
            var proj = Instantiate(res);
            proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.3f, 0.3f);
            PoolManager.Instance.RegisterProjectilePool(proj, "Earth_thrower_bullet", 5, "tower_earth_proj");

            isPlaced = true;
            return;
        }
        if (stat.level == 2)
        {
            HitEffect += (EnemyStat enemy) => enemy.PushBack(0.8f, 0.2f);
        }
    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
        var instance = PoolManager.Instance.GetProjectileFromPool("tower_earth_proj");
        instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
        TowerBehaviorLibrary.Instance.ProjectileLob(instance, target);
        instance.SetActive(true);
        var projSc = instance.GetComponent<ProjectileAdvanced>();
        projSc.PreDestruct = () =>
        {
            DealDmg(projSc.currentTarget, projSc.transform.position);
            PoolManager.Instance.ReturnProjectileToPool(instance, "tower_earth_proj");
        };
    }
}
