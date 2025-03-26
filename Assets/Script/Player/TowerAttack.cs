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
    protected bool isPlaced = false;
    //references
    //public Range range;
    public TowerStat stat;

    public Action<Vector2> KillEffect = null;
    public Action<EnemyStat> HitEffect = null;
    public Func<float, float> AttackDmg;

    public void Init(TowerStat stat)
    {
        this.stat = stat;
        AttackDmg = (d) =>
        {
            return d;
        };
        KillEffect = null;
        HitEffect = (enemy) =>
        {
            //print("fuck");
            enemy.StackElement(stat.statusStack, stat.data.element);
        };
        OnAwake();
    }

    private void Update()
    {

        if (attackTimer >= 0) 
            attackTimer -= Time.deltaTime;
        else
        {
            var target = GetTarget();
            if (target != null)
            {
                Attack(target);
            }
            attackTimer = 1 / stat.atkSpd; // base, if to implement buff, use live attack speed instead
        }

        OnUpdate();
    }
    protected virtual void OnAwake()
    {

    }
    protected virtual void OnUpdate()
    {

    }
    protected virtual GameObject GetTarget()
    {
        //base: get first in range
        return stat.range.FirstTarget();
    }

    protected virtual void Attack(GameObject target)
    {
        //print($"Attacking {target.name}. This is the base class. This shouldn't be on any run-time objects");
        stat.AttackAnimation();
    }

    protected void DealDmg(GameObject enemy, Vector2 position)
    {
        if (!stat.range.AllEnemies.Contains(enemy))
            return;

        var st = enemy.GetComponent<EnemyStat>();
        if (st.PreMitiDmg(AttackDmg(stat.dmg), stat.data))
        {
            KillEffect?.Invoke(position);
        }
        HitEffect?.Invoke(st);
    }
}

