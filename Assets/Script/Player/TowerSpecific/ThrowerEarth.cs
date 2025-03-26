using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ThrowerEarth : TowerAttack
{
    string projKey = "thrower_earth_proj";
    string explosKey = "thrower_earth_explos";
    protected override void OnAwake()
    {
        var res = Resources.Load<GameObject>("Prefab/projectile_object");
        var proj = Instantiate(res);
        proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.5f, 0.5f);
        PoolManager.Instance.RegisterProjectilePool(proj, "Earth_thrower_bullet", 5, projKey);

        var ex = Resources.Load<GameObject>("Prefab/explosion_object");
        var explos = Instantiate(ex);
        PoolManager.Instance.RegisterProjectilePool(explos, "Earth_impact", 5, explosKey);
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
            StartCoroutine(Lvl3Attack(target));
        }
    }
    private void Explosion(Vector2 spot)
    {
        var instance = PoolManager.Instance.GetProjectileFromPool(explosKey);
        var sc = instance.GetComponent<ExplosionAdvanced>();
        sc.AllEnemies = EnemyManager.Instance.AllEnemies;
        sc.PreDestruct = () => PoolManager.Instance.ReturnProjectileToPool(instance, explosKey); ;
        sc.HitEvent = (target) =>
        {
            DealDmg(target, instance.transform.position);
        };
        instance.transform.position = spot;
        instance.SetActive(true);
    }

    private IEnumerator Lvl3Attack(GameObject target)
    {
        for (int i = 0; i < 3; i++)
        {
            var instance = PoolManager.Instance.GetProjectileFromPool(projKey);
            instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
            TowerBehaviorLibrary.Instance.ProjectileLob(instance, target.transform.position + Random.Range(-1f, 1f).Vec3(), lifeSpan: 1.6f, controlHeight: 50f);
            instance.SetActive(true);
            var projSc = instance.GetComponent<ProjectileAdvanced>();
            projSc.PreDestruct = () =>
            {
                Explosion(projSc.transform.position);
                PoolManager.Instance.ReturnProjectileToPool(instance, projKey);
            };
            yield return new WaitForSeconds(0.8f);
        }
    }
}
