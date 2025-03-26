using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThrowerWater : TowerAttack
{
    string projKey = "thrower_water_proj";
    string explosKey = "thrower_water_explos";
    protected override void OnAwake()
    {
        if (!isPlaced)
        {
            var res = Resources.Load<GameObject>("Prefab/projectile_object");
            var proj = Instantiate(res);
            proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.3f, 0.3f);
            PoolManager.Instance.RegisterProjectilePool(proj, "Water_thrower_bullet", 3, projKey);

            var ex = Resources.Load<GameObject>("Prefab/explosion_object");
            var explos = Instantiate(ex);
            PoolManager.Instance.RegisterProjectilePool(explos, "Water_impact", 3, explosKey);

            isPlaced = true;
        }
        if (stat.level == 2)
        {
            var allPaths = PathManager.Instance.PathEntityDictionary.Values.ToList();
            foreach ( var path in new List<PathEntity>(allPaths) )
            {
                if (path.currentPathType != PathType.None)
                    allPaths.Remove(path);
            }
            var paths = allPaths.GetRandom(2);
            StartCoroutine(WaterThePath(paths));
        }
    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
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

        if (stat.level == 2)
        {
            projSc.PreDestruct += () =>
            {
                var allPaths = PathManager.Instance.PathEntityDictionary.Values.ToList();
                var ponds = new List<PathEntity>();
                foreach (var path in allPaths)
                {
                    if (path.currentPathType == PathType.Pond)
                        ponds.Add(path);
                }
                foreach (var p in ponds)
                {
                    var pair = GetBezierCurveParams(instance.transform.position, p.transform.position);
                    var ins2 = PoolManager.Instance.GetProjectileFromPool(projKey);
                    ins2.transform.position = instance.transform.position;
                    TowerBehaviorLibrary.Instance.ProjectileLob(ins2, p.transform.position, lifeSpan: pair.duration, controlHeight: pair.controlHeight);
                    ins2.SetActive(true);
                    ins2.GetComponent<ProjectileAdvanced>().PreDestruct = () =>
                    {
                        Explosion(ins2.transform.position);
                        PoolManager.Instance.ReturnProjectileToPool(ins2, projKey);
                    };
                }
            };
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
            DealDmg(target, sc.transform.position);
        };
        instance.transform.position = spot;
        instance.SetActive(true);
    }

    private IEnumerator WaterThePath(List<PathEntity> paths)
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < paths.Count; i++)
        {
            int index = i;
            var instance = PoolManager.Instance.GetProjectileFromPool(projKey);
            instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
            TowerBehaviorLibrary.Instance.ProjectileLob(instance, paths[index].gameObject.transform.position);
            instance.SetActive(true);
            var projSc = instance.GetComponent<ProjectileAdvanced>();
            projSc.PreDestruct = () =>
            {
                PathManager.Instance.SetGraphic(PathType.Pond, paths[index]);
                PoolManager.Instance.ReturnProjectileToPool(instance, projKey);
            };

            yield return new WaitForSeconds(1f); // Wait for 0.5s between each trigger
        }
    }
    public (float controlHeight, float duration) GetBezierCurveParams(Vector2 start, Vector2 end)
    {
        float distance = Vector2.Distance(start, end);

        // Control height proportional to distance (adjust multiplier for different curves)
        float controlHeight = distance;
        controlHeight = Mathf.Max(controlHeight, 3f);

        // Duration based on distance and speed
        float duration = distance * 0.5f;
        duration = Mathf.Clamp(duration, 0.8f, 1.6f);

        return (controlHeight, duration);
    }
}
