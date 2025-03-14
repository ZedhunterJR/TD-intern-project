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

    //private variables
    private bool hasInit = false; //for testing, might need to remove later

    //references
    private Range range;
    private SpineAnimationController spineAnimationController;

    private void Awake()
    {
        range = GetComponent<Range>();
        spineAnimationController = transform.Find("spine_animation").GetComponent<SpineAnimationController>();
    }

    //Call this when instantiate the object
    public void Init(TowerData data)
    {
        this.data = data;
        //initialize all the needed stats
        range.detectionRange = data.range;
        range.AllEnemies = EnemyManager.Instance.AllEnemies;

        //init attack script
        if (gameObject.HasComponent<TowerAttack>())
        {
            Destroy(GetComponent<TowerAttack>());
        }
        gameObject.AddComponentByString(data.attackScriptName);

        //graphic
        spineAnimationController.Init(data);
        spineAnimationController.PlayAnimationOnce("Build", "Idle");
        //initialize the range display
        //if there is runtime range modification, move this to a method instead
        transform.Find("range_display").localScale = Vector3.one * data.range;


        hasInit = true;
    }
    private void Update()
    {
        if (!hasInit)
        {
            Init(data);
        }

    }

    public void AttackAnimation()
    {
        spineAnimationController.PlayAnimationOnce("Attack", "Idle");
    }
}
