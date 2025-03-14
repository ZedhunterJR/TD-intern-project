using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class TowerBehaviorLibrary 
{
    private static TowerBehaviorLibrary instance;
    public static TowerBehaviorLibrary Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new();
            }
            return instance;
        }
    }
    private List<GameObject> AllEnemies => EnemyManager.Instance.AllEnemies;
    //lob at target enemy, if enemy die, use the last position
    public void ProjectileLob(GameObject projectile, GameObject target, bool rotation = true, float controlHeight = 5f, float lifeSpan = 1f, float controlRotation = 0)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        var startPos = projectile.transform.position;
        sc.lifeSpan = lifeSpan;
        sc.UpdateFunc = () =>
        {
            if (AllEnemies.Contains(target))
            {
                sc.direction = target.transform.position;
            }
            var start = startPos;
            Vector3 end = sc.direction;
            var control = CalculateControlPoint(start, end, controlHeight, controlRotation);

            float count = sc.LifeSpanInInterpolation;

            Vector3 m1 = Vector2.Lerp(start, control, count);
            Vector3 m2 = Vector2.Lerp(control, end, count);

            projectile.transform.position = Vector2.Lerp(m1, m2, count);

            if (rotation)
            {
                var dir = m2 - m1;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        };
    }
    //lob at target position
    public void ProjectileLob(GameObject projectile, Vector3 endPos, bool rotation = true, float controlHeight = 5f, float controlRotation = 0f, float lifeSpan = 1f)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        var startPos = projectile.transform.position;
        var controlSign = endPos.x - startPos.x >= 0f ? -1 : 1;
        var control = CalculateControlPoint(startPos, endPos, controlHeight, controlRotation);
        sc.lifeSpan = lifeSpan;
        sc.UpdateFunc = () =>
        {
            float count = sc.LifeSpanInInterpolation;

            Vector3 m1 = Vector2.Lerp(startPos, control, count);
            Vector3 m2 = Vector2.Lerp(control, endPos, count);

            projectile.transform.position = Vector2.Lerp(m1, m2, count);

            if (rotation)
            {
                var dir = m2 - m1;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        };
    }
    //straight and pierce
    public void ProjectileStraight(GameObject projectile, Vector2 dir, bool rotation = true, float lifeSpan = 1f)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        sc.lifeSpan = lifeSpan;
        dir.Normalize();
        sc.UpdateFunc = () =>
        {
            projectile.transform.position += sc.speed * Time.deltaTime * dir.ToVector3();

            if (rotation)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        };
    }
    //straight at target
    public void ProjectileStraightNoHitbox(GameObject projectile, GameObject target, bool rotation = true, float speed = 10f)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        sc.lifeSpan = 99;

        Vector2 direction = (target != null && target.activeSelf)
            ? (Vector2)(target.transform.position - projectile.transform.position).normalized
            : Vector2.zero;

        sc.UpdateFunc = () =>
        {
            if (AllEnemies.Contains(target))
            {
                sc.direction = target.transform.position; // Update target position if it moves
            }

            projectile.transform.position = Vector2.MoveTowards(projectile.transform.position, sc.direction, speed * Time.deltaTime);

            if (rotation)
            {
                var dir = (Vector3)sc.direction - projectile.transform.position;
                if (dir.sqrMagnitude < 0.001f) // Prevent NaN errors when the projectile reaches the target
                {
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    sc.DestroyObj();
                }
            }
        };
    }

    //straight at target position
    public void ProjectileStraightNoHitbox(GameObject projectile, Vector3 pos, bool rotation = true, float lifeSpan = 1f)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        var startPos = projectile.transform.position;
        sc.lifeSpan = lifeSpan;
        sc.UpdateFunc = () =>
        {
            float count = sc.LifeSpanInInterpolation;
            projectile.transform.position = Vector2.Lerp(startPos, pos, count);

            if (rotation)
            {
                var dir = pos - startPos;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        };
    }
    //sine wave at target
    public void ProjectileSineWaveNoHitbox(GameObject projectile, GameObject target, bool rotation = true, float amplitude = 0.5f, float frequency = 2f, float lifeSpan = 1f)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        var startPos = projectile.transform.position;
        sc.lifeSpan = lifeSpan;
        sc.UpdateFunc = () =>
        {
            if (AllEnemies.Contains(target))
            {
                sc.direction = target.transform.position;
            }

            float count = sc.LifeSpanInInterpolation;
            Vector2 straightPath = Vector2.Lerp(startPos, sc.direction, count);

            // Calculate sine wave offset
            Vector2 dir = (Vector3)sc.direction - startPos;
            Vector2 perpendicularDir = new Vector2(-dir.y, dir.x).normalized; // Perpendicular to the main direction
            float sineOffset = Mathf.Sin(count * frequency * Mathf.PI * 2) * amplitude;

            // Apply the sine wave offset
            projectile.transform.position = straightPath + perpendicularDir * sineOffset;

            if (rotation)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        };
    }

    private Vector2 CalculateControlPoint(Vector2 start, Vector2 end, float controlHeight, float controlRotation)
    {
        Vector2 midPoint = (start + end) / 2;
        if (controlRotation == 0)
            return midPoint + new Vector2(0, controlHeight);

        Vector2 offset = new Vector2(0, controlHeight);
        // Rotate the offset around the midpoint by `controlRotation`
        Vector2 rotatedOffset = Quaternion.Euler(0, 0, controlRotation) * offset;
        // Final control point
        return midPoint + rotatedOffset;
    }
    public void GetTowerAbility(string ability, TowerAttack sc)
    {
        switch (ability)
        {
            case "ABL_001": //water bullet
                {
                    sc.HitEffect += (e) =>
                    {
                        e.InflictVisibleStatusEffect(VisibleStatusEffect.Wet);
                    };
                }
                break;
            case "ABL_002": //fire bullet
                {
                    sc.HitEffect += (e) =>
                    {
                        e.InflictVisibleStatusEffect(VisibleStatusEffect.Heated);
                    };
                }
                break;
            case "ABL_004": //dir bullet
                {
                    sc.HitEffect += (e) =>
                    {
                        e.InflictVisibleStatusEffect(VisibleStatusEffect.Dirted);
                    };
                }
                break;
            case "ABL_006": //pond create
                {
                    sc.KillEffect += (pos) =>
                    {
                        var absolutePos = PathManager.Instance.GetNearestTileCenter(pos);
                        var path = PathManager.Instance.GetCurrentPathEntity(absolutePos);
                        if (path != null)
                            path.InflictLandMaking(PathType.Pond);
                    };
                }
                break;
            case "ABL_007": //lava create
                {
                    sc.KillEffect += (pos) =>
                    {
                        var absolutePos = PathManager.Instance.GetNearestTileCenter(pos);
                        var path = PathManager.Instance.GetCurrentPathEntity(absolutePos);
                        if (path != null)
                            path.InflictLandMaking(PathType.Lava);
                    };
                }
                break;
            case "ABL_009": //dirt create
                {
                    sc.KillEffect += (pos) =>
                    {
                        var absolutePos = PathManager.Instance.GetNearestTileCenter(pos);
                        var path = PathManager.Instance.GetCurrentPathEntity(absolutePos);
                        if (path != null)
                            path.InflictLandMaking(PathType.DirtyMist);
                    };
                }
                break;
            case "ABL_011":
                {
                    sc.stat.dmg += 1;
                }
                break;
            case "ABL_012":
                {
                    sc.stat.dmg += 2;
                }
                break;
            case "ABL_013":
                {
                    sc.stat.dmg += 3;
                }
                break;
            case "ABL_014":
                {
                    sc.range.detectionRange += 1;
                }
                break;
            case "ABL_015":
                {
                    sc.range.detectionRange += 2;
                }
                break;
            case "ABL_016":
                {
                    sc.range.detectionRange += 3;
                }
                break;
            case "ABL_019": //crit
                {
                    sc.AttackDmg = (d) => CritDmg(d,50);
                }
                break;
            case "ABL_020":
                {
                    sc.AttackDmg = (d) => CritDmg(d, 80);
                }
                break;
            case "ABL_026": //atk speed
                {
                    sc.stat.atkSpd *= 1.2f;
                }
                break;
            case "ABL_027":
                {
                    sc.stat.atkSpd *= 1.5f;
                }
                break;
            case "ABL_028":
                {
                    sc.stat.atkSpd *= 2f;
                }
                break;
        }
    }

    private float CritDmg(float dmg, float critChance)
    {
        Debug.Log("Dead Eye 1");
        var ran = Random.Range(0, 100);
        if (ran < critChance)
            return dmg * 2;
        return dmg;
    }
}
