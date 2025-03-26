using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerFire : TowerAttack
{
    string projKey = "thrower_fire_proj";
    string explosKey = "thrower_fire_explos";
    string explosKeyLvl3 = "thrower_fire_explos_lvl3";
    protected override void OnAwake()
    {
        if (!isPlaced)
        {
            var res = Resources.Load<GameObject>("Prefab/projectile_object");
            var proj = Instantiate(res);
            proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.5f, 0.5f);
            PoolManager.Instance.RegisterProjectilePool(proj, "Fire_thrower_bullet", 3, projKey);

            var ex = Resources.Load<GameObject>("Prefab/explosion_object");
            var explos = Instantiate(ex);
            PoolManager.Instance.RegisterProjectilePool(explos, "Fire_impact", 3, explosKey);

            isPlaced = true;
        }
        if (stat.level == 2)
        {
            var ex = Resources.Load<GameObject>("Prefab/explosion_object");
            var explos = Instantiate(ex);
            explos.transform.Find("spine_animation").transform.localScale = new Vector3(3f, 3f);
            explos.GetComponentInChildren<SpineAnimationController>().SetAnimationSpeed(0.5f);
            explos.GetComponent<ExplosionAdvanced>().lifeSpan = 1;
            explos.GetComponent<ExplosionAdvanced>().explosionRadius = 10;
            explos.GetComponent<ExplosionAdvanced>().dmgInterval = 2f;
            explos.GetComponent<ExplosionAdvanced>().delayFirstDmgInstance = 0.5f;
            PoolManager.Instance.RegisterProjectilePool(explos, "Fire_impact", 1, explosKeyLvl3);
            //print("?");
        }
    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        if (stat.level != 2)
        {
            var instance = PoolManager.Instance.GetProjectileFromPool(projKey);
            instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
            TowerBehaviorLibrary.Instance.ProjectileLob(instance, target.transform.position, lifeSpan: 0.9f);
            instance.SetActive(true);
            var projSc = instance.GetComponent<ProjectileAdvanced>();
            projSc.PreDestruct = () =>
            {
                Explosion(projSc.transform.position);
                PoolManager.Instance.ReturnProjectileToPool(instance, projKey);
            };
        }
        else
        {
            var instance = PoolManager.Instance.GetProjectileFromPool(projKey);
            instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
            TowerBehaviorLibrary.Instance.ProjectileLob(instance, Vector3.zero, lifeSpan: 1.5f);
            instance.SetActive(true);
            var projSc = instance.GetComponent<ProjectileAdvanced>();
            projSc.PreDestruct = () =>
            {
                ExplosionLvl3(projSc.transform.position);
                PoolManager.Instance.ReturnProjectileToPool(instance, projKey);
            };
        }
    }
    private void Explosion(Vector2 spot)
    {
        var instance = PoolManager.Instance.GetProjectileFromPool(explosKey);
        var sc = instance.GetComponent<ExplosionAdvanced>();
        sc.AllEnemies = EnemyManager.Instance.AllEnemies;
        sc.PreDestruct = () => PoolManager.Instance.ReturnProjectileToPool(instance, explosKey);
        sc.HitEvent = (target) =>
        {
            DealDmg(target, sc.transform.position);
        };
        instance.transform.position = spot;
        instance.SetActive(true);
    }
    private void ExplosionLvl3(Vector2 spot)
    {
        var instance = PoolManager.Instance.GetProjectileFromPool(explosKeyLvl3);
        var sc = instance.GetComponent<ExplosionAdvanced>();
        sc.AllEnemies = EnemyManager.Instance.AllEnemies;
        sc.PreDestruct = () => PoolManager.Instance.ReturnProjectileToPool(instance, explosKeyLvl3);
        sc.HitEvent = (target) =>
        {
            DealDmg(target, sc.transform.position);
        };
        instance.transform.position = spot;
        instance.SetActive(true);
    }
}
