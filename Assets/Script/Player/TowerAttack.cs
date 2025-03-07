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
    protected Range range;
    protected TowerStat stat;

    //pooling projectiles
    private List<GameObject> projectilePool = new();
    protected void InitPool(string spineAniType, int num)
    {
        var res = Resources.Load<GameObject>("Prefab/projectile_object");
        for (int i = 0; i < num; i++)
        {
            //print("this si bs");
            var item = Instantiate(res, this.transform);
            item.GetComponentInChildren<SpineAnimationController>().PlayAnimation(spineAniType);
            item.SetActive(false);
            projectilePool.Add(item);
        }

    }

    /// <summary>
    /// The bullshit SpineAnimation doesn't allow direct copy in runtime, so
    /// have to reapply the skin, otherwise the projectile can have any properties
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="spineAniType"></param>
    /// <param name="num"></param>
    protected void InitPool(GameObject projectile, string spineAniType, int num)
    {
        for (int i = 0; i < num; i++)
        {
            var item = Instantiate(projectile, this.transform);
            item.GetComponentInChildren<SpineAnimationController>().PlayAnimation(spineAniType);
            item.SetActive(false);
            projectilePool.Add(item);
        }
        Destroy(projectile);
    }
    protected GameObject GetFromPool()
    {
        GameObject projGet = projectilePool.Find(x => !x.activeSelf);
        projGet.transform.position = transform.position;
        //projGet.SetActive(true);
        return projGet;
    }
    protected void ReturnToPool(GameObject projectile)
    {
        projectile.SetActive(false);
    }

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

