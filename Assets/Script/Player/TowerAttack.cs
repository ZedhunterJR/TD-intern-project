using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for managing tower behavior
/// For each individual tower, create a derivative of this class and modify it instead
/// Always call base.Attack(target), as it contains animation
/// Also pooling because fuck it
/// </summary>
public class TowerAttack : MonoBehaviour
{
    //private variables
    private float attackTimer = 1f;

    //references
    public Range range;
    public TowerStat stat;

    public Action<Vector2> KillEffect = null;
    public Action<EnemyStat> HitEffect = null;
    public Func<float, float> AttackDmg;

    public void Init()
    {
        range = GetComponent<Range>();
        stat = GetComponent<TowerStat>();
        AttackDmg = (d) =>
        {
            return d;
        };
        OnAwake();
    }

    public void OnUpdate()
    {

        if (attackTimer >= 0) 
            attackTimer -= Time.deltaTime;
        else
        {
            var target = GetTarget();
            if (target != null)
            {
                Attack(target);
                attackTimer = 1 / stat.data.baseAtkSpd; // base, if to implement buff, use live attack speed instead
            }
        }

    }
    protected virtual void OnAwake()
    {

    }
    protected virtual GameObject GetTarget()
    {
        //base: get first in range
        return range.FirstTarget();
    }

    protected virtual void Attack(GameObject target)
    {
        //print($"Attacking {target.name}. This is the base class. This shouldn't be on any run-time objects");
        stat.AttackAnimation();
    }

    protected void DealDmg(GameObject enemy, Vector2 position)
    {
        if (!range.AllEnemies.Contains(enemy))
            return;

        var st = enemy.GetComponent<EnemyStat>();
        HitEffect?.Invoke(st);
        if (st.PreMitiDmg(AttackDmg(stat.dmg), stat.data))
        {
            KillEffect?.Invoke(position);
        }
    }
}

