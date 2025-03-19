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
    public Range range;
    private SpineAnimationController spineAnimationController;

    //live stats
    public float dmg;
    public float atkSpd;
    public int level = 0;
    public int statusStack;
    public string lvl3Ability;
    public TowerAttack liveAttackScript;

    private void Awake()
    {
        spineAnimationController = transform.Find("spine_animation").GetComponent<SpineAnimationController>();
    }

    //Call this when instantiate the object
    public void Init(TowerData data)
    {
        range = new Range(EnemyManager.Instance.AllEnemies, transform.position);
        this.data = data;
        //initialize all the needed stats

        //init attack script
        if (gameObject.HasComponent<TowerAttack>())
        {
            Destroy(GetComponent<TowerAttack>());
        }
        gameObject.AddComponentByString(data.attackScriptName);
        liveAttackScript = GetComponent<TowerAttack>();
        liveAttackScript.Init();

        //graphic
        spineAnimationController.Init(data);
        spineAnimationController.PlayAnimationOnce("Build", "Idle");

        level = -1;
        LevelUp();
    }
    public void LevelUp()
    {
        level++;
        dmg = data.baseDamage[level];
        atkSpd = data.baseAtkSpd[level];
        statusStack = data.statusEffectStack[level];
        ModifyRange(data.range[level]);

        switch (level)
        {
            case 0: TowerBehaviorLibrary.Instance.GetTowerAbility(data.lvl1Ability, liveAttackScript); break;
            case 1: TowerBehaviorLibrary.Instance.GetTowerAbility(data.lvl2Ability, liveAttackScript); break;
            case 2: TowerBehaviorLibrary.Instance.GetTowerAbility(lvl3Ability, liveAttackScript); break;
        }
    }
    private void ModifyRange(float value)
    {
        range.detectionRange = data.range[0];
        //initialize the range display
        //if there is runtime range modification, move this to a method instead
        transform.Find("range_display").localScale = Vector3.one * data.range[0];
    }
    public void AttackAnimation()
    {
        spineAnimationController.PlayAnimationOnce("Attack", "Idle");
    }
}
