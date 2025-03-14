using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage stats
/// if to manage behavior, look at TowerAttack.cs and its derivative
/// </summary>
public class TowerStat : MonoBehaviour
{
    //public variables
    public TowerData data;

    //references
    private Range range;
    private SpineAnimationController spineAnimationController;

    //live stats
    public float dmg;
    public float atkSpd;
    public int level;

    private void Awake()
    {
        range = GetComponent<Range>();
        spineAnimationController = transform.Find("spine_animation").GetComponent<SpineAnimationController>();
    }

    //Call this when instantiate the object
    public void Init(TowerData data, int level)
    {
        this.data = data;
        //initialize all the needed stats
        range.detectionRange = data.range;
        range.AllEnemies = EnemyManager.Instance.AllEnemies;
        dmg = data.baseDamage;
        atkSpd = data.baseAtkSpd;

        //init attack script
        if (gameObject.HasComponent<TowerAttack>())
        {
            Destroy(GetComponent<TowerAttack>());
        }
        gameObject.AddComponentByString(data.attackScriptName);
        var sc = GetComponent<TowerAttack>();

        //graphic
        spineAnimationController.Init(data);
        spineAnimationController.PlayAnimationOnce("Build", "Idle");
        //initialize the range display
        //if there is runtime range modification, move this to a method instead
        transform.Find("range_display").localScale = Vector3.one * data.range;

        //a lotta bs
        foreach (var item in data.lvl1Abilites)
            TowerBehaviorLibrary.Instance.GetTowerAbility(item, sc);
        if (level > 1) foreach (var item in data.lvl2Abilites)
                TowerBehaviorLibrary.Instance.GetTowerAbility(item, sc);
        if (level > 2) foreach (var item in data.lvl3Abilites)
                TowerBehaviorLibrary.Instance.GetTowerAbility(item, sc);
    }

    public void AttackAnimation()
    {
        spineAnimationController.PlayAnimationOnce("Attack", "Idle");
    }
}
