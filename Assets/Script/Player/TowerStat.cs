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
    [SerializeField] private SpineAnimationController spineAnimationController;

    //live stats
    public float dmg;
    public float atkSpd;
    public int level = 0;
    public int statusStack;
    public TowerAttack liveAttackScript;

    private void Awake()
    {
        //spineAnimationController = transform.Find("spine_animation").GetComponent<SpineAnimationController>();
    }

    //Call this when instantiate the object
    public void Init(TowerData data)
    {
        range = new Range(EnemyManager.Instance.AllEnemies, transform.position);
        this.data = data;
        //initialize all the needed stats

        //init attack script
        if (GetComponent<TowerAttack>() != null)
        {
            Destroy(GetComponent<TowerAttack>());
        }
        GameManager.Instance.StartCoroutine(InitNextFrame());
    }
    private IEnumerator InitNextFrame()
    {
        yield return null;
        gameObject.SetActive(true);
        liveAttackScript = gameObject.AddComponentByString(data.attackScriptName) as TowerAttack;

        //graphic
        spineAnimationController.Init(data);

        level = -1;
        LevelUp();
        HideRange();
    }
    public void LevelUp()
    {
        level++;
        dmg = data.baseDamage[level];
        atkSpd = data.baseAtkSpd[level];
        statusStack = data.statusEffectStack[level];
        ModifyRange(data.range[level]);
        spineAnimationController.PlayAnimationOnce("Build", "Idle");
        liveAttackScript.Init(this);
        switch (level)
        {
            case 0: 
                spineAnimationController.SetOutlineColor("#2E7D32".HexColor());
                spineAnimationController.transform.parent.localScale = Vector3.one;
                break;
            case 1:
                spineAnimationController.SetOutlineColor("#8A2BE2".HexColor());
                spineAnimationController.transform.parent.localScale = new Vector3(1.2f, 1.2f);
                break;
            case 2:
                spineAnimationController.SetOutlineColor("#FFD700".HexColor());
                spineAnimationController.transform.parent.localScale = new Vector3(1.5f, 1.5f);
                break;
        }
    }
    private void ModifyRange(float value)
    {
        range.detectionRange = value;
        //initialize the range display
        //if there is runtime range modification, move this to a method instead
        transform.Find("range_display").localScale = Vector3.one * value;
    }
    public void ShowRange(Color color)
    {
        color = color.SetAlpha(0.2f);
        var rDis = transform.Find("range_display");
        rDis.GetComponent<SpriteRenderer>().color = color;
        rDis.gameObject.SetActive(true);
    }
    public void HideRange()
    {
        transform.Find("range_display").gameObject.SetActive(false);
    }
    public void AttackAnimation()
    {
        spineAnimationController.PlayAnimationOnce("Attack", "Idle");
    }

    public bool CanMerge(TowerStat other)
    {
        return this.data == other.data && this.level == other.level && level < 2;
    }
}
