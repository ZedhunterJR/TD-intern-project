using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerFire : TowerAttack
{
    protected override void OnAwake()
    {
        var res = Resources.Load<GameObject>("Prefab/projectile_object");
        var proj = Instantiate(res);
        proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.5f, 0.5f);
        PoolManager.Instance.RegisterProjectilePool(proj, "Fire_thrower_bullet", 5, "thrower_fire_proj");

        var ex = Resources.Load<GameObject>("Prefab/explosion_object");
        var explos = Instantiate(ex);
        PoolManager.Instance.RegisterProjectilePool(explos, "Fire_impact", 5, "thrower_fire_explos");
    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
        var instance = PoolManager.Instance.GetProjectileFromPool("thrower_fire_proj");
        instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
        TowerBehaviorLibrary.Instance.ProjectileLob(instance, target.transform.position, lifeSpan: 0.9f);
        instance.SetActive(true);
        var projSc = instance.GetComponent<ProjectileAdvanced>();
        projSc.PreDestruct = () =>
        {
            Explosion(projSc.transform.position);
            PoolManager.Instance.ReturnProjectileToPool(instance);
        };
    }
    private void Explosion(Vector2 spot)
    {
        var instance = PoolManager.Instance.GetProjectileFromPool("thrower_fire_explos");
        var sc = instance.GetComponent<ExplosionAdvanced>();
        sc.AllEnemies = EnemyManager.Instance.AllEnemies;
        sc.PreDestruct = () => PoolManager.Instance.ReturnProjectileToPool(instance); ;
        sc.HitEvent = (target) =>
        {
            DealDmg(target, sc.transform.position);
        };
        instance.transform.position = spot;
        instance.SetActive(true);
    }
}
