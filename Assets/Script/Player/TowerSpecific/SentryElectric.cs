using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryElectric : TowerAttack
{
    protected override void OnAwake()
    {
        var res = Resources.Load<GameObject>("Prefab/projectile_object");
        var proj = Instantiate(res);
        InitPool(proj, "Elec_thrower_bullet", 5);
    }

    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
        var instance = GetFromPool();
        var ran = Random.Range(0, 2) == 0 ? 300f : 240f;
        ProjectileLibrary.Instance.ProjectileLob(instance, target, controlHeight:5f, controlRotation:ran, lifeSpan:0.8f);
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
