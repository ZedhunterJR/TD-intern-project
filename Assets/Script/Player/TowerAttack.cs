using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for managing tower behavior
/// For each individual tower, create a derivative of this class and modify it instead
/// Always call base.Attack(target), as it contains animation
/// </summary>
public class TowerAttack : MonoBehaviour
{
    //private variables
    private float attackTimer = 1f;

    //references
    protected Range range;
    protected TowerStat stat;

    private void Awake()
    {
        range = GetComponent<Range>();
        stat = GetComponent<TowerStat>();
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
}

