using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEarth : TowerAttack
{
    private int dirtPathLeft = 1;
    private LineRenderer crosshair;
    protected override void OnAwake()
    {
        if (!isPlaced)
        {
            var res = Resources.Load<GameObject>("Prefab/projectile_object");
            var proj = Instantiate(res);
            proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.2f, 0.2f);
            PoolManager.Instance.RegisterProjectilePool(proj, "Earth_thrower_bullet", 5, "sniper_earth_proj");
            AttackDmg = (baseDmg) =>
            {
                if (Random.Range(0, 100) < 50)
                    return baseDmg * 2;
                return baseDmg;
            };

            isPlaced = true;
        }
        if (stat.level != 2)
        {
            var crosshair = Resources.Load<GameObject>("Prefab/sniper_earth_abi");
            var proj = Instantiate(crosshair, this.transform);
            this.crosshair = proj.GetComponent<LineRenderer>();
            proj.transform.position = new Vector2(100, 100);
        }
    }
    protected override void Attack(GameObject target)
    {
        if (stat.level != 2)
        {
            base.Attack(target);
            //might need pooling for projectile
            var instance = PoolManager.Instance.GetProjectileFromPool("sniper_earth_proj");
            instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
            TowerBehaviorLibrary.Instance.ProjectileStraightNoHitbox(instance, target, speed: 12f);
            instance.SetActive(true);
            var projSc = instance.GetComponent<ProjectileAdvanced>();
            projSc.PreDestruct = () =>
            {
                DealDmg(projSc.currentTarget, projSc.transform.position);
                PoolManager.Instance.ReturnProjectileToPool(instance, "sniper_earth_proj");
            };
        }
        else
        {
            StartCoroutine(Snipe());
        }
    }

    private IEnumerator Snipe()
    {
        float aimTime = 5f;
        GameObject target = null;
        while (aimTime > 0)
        {
            aimTime -= 1/30f;
            target = stat.range.StrongTarget();

            if (target != null)
            {
                crosshair.positionCount = 2;
                crosshair.SetPosition(0, transform.position + new Vector3(0, stat.data.projSpwPosY));
                var targetPos = target.transform.position + new Vector3(0, target.GetComponent<EnemyStat>().Center);
                crosshair.SetPosition(1, targetPos);
                crosshair.gameObject.transform.position = targetPos;
            }
            else
            {
                crosshair.positionCount = 0;
                crosshair.gameObject.transform.position = new Vector2(100, 100);
            }
            yield return new WaitForSeconds(1/30f);
        }

        crosshair.positionCount = 0;
        crosshair.gameObject.transform.position = new Vector2(100, 100);

        if (target != null)
        {
            base.Attack(target);
            //might need pooling for projectile
            var instance = PoolManager.Instance.GetProjectileFromPool("sniper_earth_proj");
            instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
            TowerBehaviorLibrary.Instance.ProjectileStraightNoHitbox(instance, target, speed: 15f);
            instance.SetActive(true);
            var projSc = instance.GetComponent<ProjectileAdvanced>();
            projSc.PreDestruct = () =>
            {
                if (projSc.currentTarget != null)
                {
                    if (projSc.currentTarget.GetComponent<EnemyStat>().UpdateHp(-1000, Color.white) && dirtPathLeft > 0)
                    {
                        var pos = PathManager.Instance.GetNearestTileCenter(projSc.transform.position);
                        var path = PathManager.Instance.GetCurrentPathEntity(pos);
                        if (path != null && path.currentPathType == PathType.None)
                        {
                            PathManager.Instance.SetGraphic(PathType.DirtyMist, path);
                            dirtPathLeft--;
                        }
                    }
                }
                PoolManager.Instance.ReturnProjectileToPool(instance, "sniper_earth_proj");
            };
        }
    }
}
