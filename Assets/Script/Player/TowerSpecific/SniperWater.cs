using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SniperWater : TowerAttack
{
    private float abiInterval = 0;
    protected override void OnAwake()
    {
        if (!isPlaced)
        {
            var res = Resources.Load<GameObject>("Prefab/projectile_object");
            var proj = Instantiate(res);
            proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.15f, 0.15f);
            PoolManager.Instance.RegisterProjectilePool(proj, "Water_thrower_bullet", 3, "sniper_water_proj");

            AttackDmg = (baseDmg) =>
            {
                if (Random.Range(0, 100) < 50)
                    return baseDmg * 2;
                return baseDmg;
            };

            isPlaced = true;
        }

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
            DealDmg(projSc.currentTarget, projSc.transform.position);
            PoolManager.Instance.ReturnProjectileToPool(instance, "sniper_water_proj");
        };

    }
    protected override void OnUpdate()
    {
        if (stat.level != 2) return;
        if (abiInterval < 0.8f)
        {
            abiInterval += Time.deltaTime;
            return;
        }
        abiInterval = 0;

        var abiTarget = GetTargetForAbi(out var status);
        if (abiTarget != null)
        {
            var abi = PoolManager.Instance.GetProjectileFromPool("sniper_water_proj");
            abi.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
            TowerBehaviorLibrary.Instance.ProjectileSineWaveNoHitbox(abi, abiTarget, lifeSpan: 0.5f);
            abi.SetActive(true);
            var abiSc = abi.GetComponent<ProjectileAdvanced>();
            abiSc.PreDestruct = () =>
            {
                //print(target);
                if (abiSc.currentTarget != null)
                {
                    List<Element> avais = new List<Element>() { Element.Fire, Element.Water, Element.Earth };
                    avais.Remove(status);
                    abiSc.currentTarget.GetComponent<EnemyStat>().StackElement(5, avais.GetRandom());
                }
                PoolManager.Instance.ReturnProjectileToPool(abi, "sniper_water_proj");
            };
        }

    }
    private GameObject GetTargetForAbi(out Element status)
    {
        foreach (var enemy in stat.range.AllEnemies)
        {
            var eStat = enemy.GetComponent<EnemyStat>();
            if (eStat.CurrentBasicStatusEffect != Element.None)
            {
                status = eStat.CurrentBasicStatusEffect;
                return enemy;
            }
        }
        status = Element.None;
        return null;
    }
}
