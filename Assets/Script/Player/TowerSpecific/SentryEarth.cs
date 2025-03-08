using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryEarth : TowerAttack
{
    protected override void OnAwake()
    {
        var res = Resources.Load<GameObject>("Prefab/projectile_object");
        var proj = Instantiate(res);
        proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.3f, 0.3f);
        InitPool(proj, "Earth_thrower_bullet", 5);

        var ex = Resources.Load<GameObject>("Prefab/explosion_object");
        var explos = Instantiate(ex);
        InitPool(explos, "Earth_impact", 5);
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
        ProjectileLibrary.Instance.ProjectileLob(instance, target.transform.position, lifeSpan: 0.9f);
        instance.SetActive(true);
        var projSc = instance.GetComponent<ProjectileAdvanced>();
        projSc.PreDestruct = () =>
        {
            Explosion(projSc.transform.position);
            ReturnToPool(instance);
        };
    }
    private void Explosion(Vector2 spot)
    {
        var instance = GetFromPool(1);
        var sc = instance.GetComponent<ExplosionAdvanced>();
        sc.AllEnemies = EnemyManager.Instance.AllEnemies;
        sc.PreDestruct = () => ReturnToPool(instance);
        sc.HitEvent = (target) =>
        {
            target.GetComponent<EnemyStat>().PreMitiDmg(stat.data.baseDamage);
        };
        instance.transform.position = spot;
        instance.SetActive(true);
    }

}
