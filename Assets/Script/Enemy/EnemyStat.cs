using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public EnemyData data;

    public float maxHealth;
    public float currentHp;
    public float currentSpeed;
    public float maxSpeed;

    // Buff and DeBuff List 
    [SerializeField]
    List<StatusEffect> activeEffects = new List<StatusEffect>();

    public VisibleStatusEffect CurrentVisibleStatusEffect { get; private set; }
    private float currentVisibleStatusTimer;

    //related to ability
    public Action PreDestruction;
    public List<AbilityUpdateFunc> AbilityUpdates = new();
    public Func<float, TowerData, float> PreMitiDmgFunc;
    public bool isUntargetable;
    public Action<PathType> EnteringTile;
    public Action<PathType> ExitingTile;

    //tile and path
    public Vector2 CurrentPositionInAbs { get; private set; }
    private PathType currentStandingPathType = PathType.None;

    //ref
    private GameObject hpBarCover;
    private WaveMove moveScript;
    private Transform spineAnimation;

    private float initialScale;
    private bool flipX;

    void Awake()
    {
        hpBarCover = transform.Find("health/cover").gameObject;
        moveScript = GetComponent<WaveMove>();
        spineAnimation = transform.Find("spine_animation");
    }
    private string SkinName(int level)
    {
        string res = "skin" + level + "-";
        switch (data.element)
        {
            case Element.Fire: res += "fire"; break;
            case Element.Water: res += "water"; break;
            case Element.Earth: res += "earth"; break;
        }
        return res;
    }

    public void Init(EnemyData data, int level)
    {
        this.data = data;
        maxHealth = data.maxHp;
        currentHp = maxHealth;
        maxSpeed = data.baseMoveSpeed;
        currentSpeed = maxSpeed;
        UpdateHp(0); //to reset hp bar

        //spine init
        spineAnimation.GetComponent<SpineAnimationController>().Init(data);
        spineAnimation.GetComponent<SpineAnimationController>().SetSkinName(SkinName(level));
        initialScale = spineAnimation.transform.localScale.y;

        //init wave move script
        List<Vector2> wps = new(FindFirstObjectByType<Waypoints>().waypoints[0].points);
        Action onexit = () =>
        {
            GameManager.Instance.TakeDame();
            PoolManager.Instance.RespawnObject(OBJ_TYPE.enemyTest, gameObject);
        };
        moveScript.Init(wps, onexit);

        //idk but this should be be4 ability
        CurrentPositionInAbs = new Vector2(69, 420);
        CurrentVisibleStatusEffect = VisibleStatusEffect.None;
        isUntargetable = false;
        PreDestruction = null;
        EnteringTile = null;
        ExitingTile = null;
        AbilityUpdates = new();
        PreMitiDmgFunc = (d, s) => d;

        //init ability
        foreach (var item in data.lvl1Abilities)
            EnemyAbilityLibrary.Instance.GetAbility(this, item);
        if (level > 1) foreach (var item in data.lvl2Abilities)
                EnemyAbilityLibrary.Instance.GetAbility(this, item);
        if (level > 2) foreach (var item in data.lvl3Abilities)
                EnemyAbilityLibrary.Instance.GetAbility(this, item);
    }

    public void OnUpdate()
    {
        /* Test thành công apply hiệu ứng vào trong enem, có thể apply nhiều hiệu ứng cùng lúc 
        if (Input.GetKeyDown(KeyCode.S))
        {
            statusEffect = new StatusEffect() { status = STATUS_EFFECT.Slow, duration = 2f };
            Debug.Log($"Hiệu ứng là {statusEffect.status} thời gian {statusEffect.duration}");
            AddEffect(statusEffect);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            statusEffect = new StatusEffect() { status = STATUS_EFFECT.Stun, duration = 1f };
            Debug.Log($"Hiệu ứng là {statusEffect.status} thời gian {statusEffect.duration}");
            AddEffect(statusEffect);
        }
        */

        // Effect Apply and Undo
        UpdateMovementEffect();
        if (currentVisibleStatusTimer > 0)
            currentVisibleStatusTimer -= Time.deltaTime;
        else
            SetVisibleStatusEffectGraphic(VisibleStatusEffect.None);

        //Manage movement and rotation
        moveScript.MoveUpdate(currentSpeed);
        spineAnimation.GetComponent<SpineAnimationController>().SetAnimationSpeedBaseOnMoveSpeed(currentSpeed);
        if (flipX != moveScript.FlipX)
        {
            flipX = moveScript.FlipX;
            spineAnimation.localScale = new Vector3(initialScale * (flipX ? -1 : 1), initialScale, initialScale);
        }

        //Enemy ability
        foreach (var item in AbilityUpdates)
        {
            item.OnUpdate();
        }

        UpdatePathPosition();
    }

    #region HP,Pos Update
    public bool UpdateHp(float value)
    {
        currentHp += value;
        currentHp = Mathf.Clamp(currentHp, 0, maxHealth);
        if (currentHp == 0)
        {
            PreDestruction?.Invoke();

            PoolManager.Instance.RespawnObject(OBJ_TYPE.enemyTest, gameObject);
            //Destroy(gameObject);
            //EventManager.Instance.ModiGold(enemyEquivalent * 10f);
            return true;
        }

        Vector2 scale = new Vector2(1 - (currentHp / maxHealth), 1);
        hpBarCover.transform.localScale = scale;
        return false;
    }

    public bool PreMitiDmg(float dmg, TowerData attackData)
    {
        //print(dmg);
        dmg = PreMitiDmgFunc(dmg, attackData);
        if (CurrentVisibleStatusEffect == VisibleStatusEffect.Crystalized)
            dmg = 0;
        if (CurrentVisibleStatusEffect == VisibleStatusEffect.Fortified)
            dmg = dmg / 2f;
        return UpdateHp(-Mathf.Floor(dmg));
    }
    /// <summary>
    /// have to check every frame, not even skipping checking the same grid because of possible
    /// changing path
    /// </summary>
    /// <param name="gridSize"></param>
    public void UpdatePathPosition(float gridSize = 1f)
    {
        Vector2 pos = transform.position;
        Vector2 snappedPos = GetNearestTileCenter(pos, gridSize);

        // Continue
        var pathManager = PathManager.Instance;
        var pType = pathManager.GetCurrentStandingPath(snappedPos);

        if (currentStandingPathType == pType && CurrentPositionInAbs == snappedPos)
            return;

        // And continue
        pathManager.UndoPathEffect(this.gameObject, currentStandingPathType);
        ExitingTile?.Invoke(currentStandingPathType);
        pathManager.ApplyPathEffect(gameObject, pType);
        EnteringTile?.Invoke(pType);
        currentStandingPathType = pType;
        CurrentPositionInAbs = snappedPos;
    }

    // Helper method to get the nearest tile center
    private Vector2 GetNearestTileCenter(Vector2 position, float tileSize)
    {
        float tileX = Mathf.Round(position.x / tileSize) * tileSize;
        float tileY = Mathf.Round(position.y / tileSize) * tileSize;
        return new Vector2(tileX, tileY);
    }
    #endregion

    #region Status Effect
    public void AddEffect(StatusEffect effect)
    {
        activeEffects.Add(effect);
    }

    /* idk honestly
    private void ApplyEffects()
    {
        currentSpeed = maxSpeed;

        foreach (StatusEffect effect in activeEffects)
        {
            switch (effect.status)
            {
                case STATUS_EFFECT.None:
                    break;
                case STATUS_EFFECT.Slow:
                    currentSpeed *= (1 - .5f);
                    break;
                case STATUS_EFFECT.Stun:
                    currentSpeed = 0f;
                    break;
                default:
                    break;
            }
        }
    }

    private void UpdateEffect()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].duration -= Time.deltaTime;
            if (activeEffects[i].duration <= 0)
            {
                activeEffects.RemoveAt(i); // Xóa hiệu ứng khi hết thời gian
            }
        }

        // Nếu danh sách trống => Reset tốc độ về bình thường
        if (activeEffects.Count == 0)
        {
            currentSpeed = maxSpeed;
        }
    }*/
    private void UpdateMovementEffect()
    {
        //update timer first
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].duration -= Time.deltaTime;
            if (activeEffects[i].duration <= 0)
            {
                activeEffects.RemoveAt(i); // Xóa hiệu ứng khi hết thời gian
            }
        }

        var stun = false;
        var msIncrease = 1f;
        var msDecrease = 1f;
        foreach (StatusEffect effect in activeEffects)
        {
            if (effect.status == STATUS_EFFECT.Stun)
            {
                stun = true;
                break;
            }
            if (effect.status == STATUS_EFFECT.MovespeedChange)
            {
                if (effect.msPercentage > 1)
                    msIncrease += effect.msPercentage - 1;
                if (effect.msPercentage < 1)
                    msDecrease += effect.msPercentage - 1;
            }
        }
        if (stun)
        {
            currentSpeed = 0f;
            return;
        }
        currentSpeed = maxSpeed * msIncrease * Mathf.Max(msDecrease, 0.2f); //slow never go past 80% slow
    }

    public void InflictVisibleStatusEffect(VisibleStatusEffect status)
    {
        var pathSet = new HashSet<VisibleStatusEffect> { CurrentVisibleStatusEffect, status };

        if (pathSet.SetEquals(new HashSet<VisibleStatusEffect> { VisibleStatusEffect.Wet, VisibleStatusEffect.Heated }))
            SetVisibleStatusEffectGraphic(VisibleStatusEffect.None);
        if (pathSet.SetEquals(new HashSet<VisibleStatusEffect> { VisibleStatusEffect.Wet, VisibleStatusEffect.Dirted }))
        {
            SetVisibleStatusEffectGraphic(VisibleStatusEffect.Glutinous);
            AddEffect(new StatusEffect(3f, 0.5f));
        }
        if (pathSet.SetEquals(new HashSet<VisibleStatusEffect> { VisibleStatusEffect.Dirted, VisibleStatusEffect.Heated }))
        {
            SetVisibleStatusEffectGraphic(VisibleStatusEffect.Crystalized);
            AddEffect(new StatusEffect(3f));
        }
        if (CurrentVisibleStatusEffect == VisibleStatusEffect.None)
        {
            SetVisibleStatusEffectGraphic(status);
        }
    }
    private void SetVisibleStatusEffectGraphic(VisibleStatusEffect status)
    {
        CurrentVisibleStatusEffect = status;
        if (status == VisibleStatusEffect.None)
        {
            return;
        }
        currentVisibleStatusTimer = 3f; //3s timer for status, after which, return to None status
        //maybe dealing with icon on top of healthbar later
    }


    #endregion

}

//move here for easier managing
[System.Serializable]
public class StatusEffect
{
    public STATUS_EFFECT status;
    public float duration;

    public float msPercentage;
    /// <summary>
    /// Constructor for stun, just need duration
    /// </summary>
    /// <param name="duration"></param>
    public StatusEffect(float duration)
    {
        status = STATUS_EFFECT.Stun;
        this.duration = duration;
    }
    /// <summary>
    /// constructor for ms change. E.g. 1.2f -> 20% speed up, 0.8f -> 20% slow down
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="msPercentage"></param>
    public StatusEffect(float duration, float msPercentage)
    {
        status = STATUS_EFFECT.MovespeedChange;
        this.duration = duration;
        this.msPercentage = msPercentage;
    }
}

public enum STATUS_EFFECT
{
    None,
    MovespeedChange,
    Stun,
}
public enum VisibleStatusEffect //yeah terrible naming i know
{
    None,
    Heated,
    Wet,
    Dirted,
    Fortified,
    Crystalized,
    Glutinous
}

public class AbilityUpdateFunc
{
    private Action updateFunc = null;
    private float interval = 3f;
    private float timer = 1f;
    public AbilityUpdateFunc(Action updateFunc, float interval, float beginTimer = 1f)
    {
        this.updateFunc = updateFunc;
        this.interval = interval;
        timer = beginTimer;
    }
    public void OnUpdate()
    {
        if (timer <= 0)
        {
            updateFunc?.Invoke();
            timer = interval;
        }
        else
            timer -= Time.deltaTime;
    }
}