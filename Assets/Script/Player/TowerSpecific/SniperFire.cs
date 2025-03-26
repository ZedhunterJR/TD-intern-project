using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperFire : TowerAttack
{
    protected override void OnAwake()
    {
        if (!isPlaced)
        {
            var res = Resources.Load<GameObject>("Prefab/projectile_object");
            var proj = Instantiate(res);
            proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.3f, 0.3f);
            PoolManager.Instance.RegisterProjectilePool(proj, "Fire_thrower_bullet", 3, "sniper_fire_proj");
            AttackDmg = (baseDmg) =>
            {
                if (Random.Range(0, 100) < 50)
                    return baseDmg * 2;
                return baseDmg;
            };

            isPlaced = true;
        }
        if (stat.level == 2)
        {
            HitEffect = (enemy) =>
            {
                enemy.Burn(2f);
            };
        }
    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);

        if (stat.level != 2)
        {
            var instance = PoolManager.Instance.GetProjectileFromPool("sniper_fire_proj");
            instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
            TowerBehaviorLibrary.Instance.ProjectileStraightNoHitbox(instance, target, speed: 12f);
            instance.SetActive(true);
            var projSc = instance.GetComponent<ProjectileAdvanced>();
            projSc.PreDestruct = () =>
            {
                DealDmg(projSc.currentTarget, projSc.transform.position);
                PoolManager.Instance.ReturnProjectileToPool(instance, "sniper_fire_proj");
            };
        }
        else
        {
            var instance = PoolManager.Instance.GetProjectileFromPool("sniper_fire_proj"); 
            var ran = Random.Range(0, 2) == 0 ? 300f : 240f;
            instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
            TowerBehaviorLibrary.Instance.ProjectileLob(instance, target, controlHeight: 5f, controlRotation: ran, lifeSpan: 0.8f);
            instance.SetActive(true);
            var projSc = instance.GetComponent<ProjectileAdvanced>();
            projSc.PreDestruct = () =>
            {
                DealDmg(projSc.currentTarget, projSc.transform.position);
                PoolManager.Instance.ReturnProjectileToPool(instance, "sniper_fire_proj");
            };
        }
    }
}
