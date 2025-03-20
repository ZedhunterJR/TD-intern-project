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

    #region new status effect bs
    [SerializeField]
    List<StatusEffect> activeEffects = new List<StatusEffect>();
    private int burnStack = 0;
    private float burnTime = 0;
    private int wetStack = 0;
    private float wetTime = 0;
    private int dirtedStack = 0;
    private float dirtedTime = 0;
    private float combineEffectTimer = 0;
    public void ResetAllStatusEffect()
    {
        activeEffects = new();
        burnStack = 0;
        burnTime = 0;
        wetStack = 0;
        wetTime = 0;
        dirtedStack = 0;
        dirtedTime = 0;
        combineEffectTimer = 0;
        CurrentCombinedStatusEffect = CombinedStatusEffect.None;
        statusEffectCon.ResetAll();
    }
    public CombinedStatusEffect CurrentCombinedStatusEffect { get; private set; }
    public void StackElement(int stack, Element element)
    {
        if (CurrentCombinedStatusEffect != CombinedStatusEffect.None)
            return;
        switch (element)
        {
            case Element.Earth:
                dirtedStack += stack;
                if (dirtedStack >= 5)
                {
                    dirtedStack -= 5;
                    activeEffects.Remove(dirtedStun);
                    activeEffects.Add(dirtedStun);
                    HandleVisibleStatusEffect(Element.Earth);
                }
                break;
            case Element.Water:
                wetStack += stack;
                if (wetStack >= 5)
                {
                    activeEffects.Remove(wetSlow);
                    activeEffects.Add(wetSlow);
                    wetStack = stack - 5;
                    HandleVisibleStatusEffect(Element.Water);
                }
                break;
            case Element.Fire:
                burnStack += stack;
                if (burnStack >= 5)
                {
                    burnStack = stack - 5;
                    HandleVisibleStatusEffect(Element.Fire);
                }
                break;
        }
        //print(dirtedStack);
    }
    private float dotInterval;
    private void UpdateBasicEffect()
    {
        dotInterval += Time.deltaTime;
        if (dotInterval > 0.5f)
        {
            if (burnTime > 0)
                UpdateHp(-1, "#FF6A00".HexColor());
            if (CurrentCombinedStatusEffect == CombinedStatusEffect.Crystalized)
                UpdateHp(-1.5f, "#D85CFF".HexColor());
            dotInterval = 0;
        }
        if (burnTime > 0)
            burnTime -= Time.deltaTime;
        else
            DeactivateStatusEffectGraphic();
        if (dirtedTime > 0)
            dirtedTime -= Time.deltaTime;
        else
        {
            activeEffects.Remove(dirtedStun);
            DeactivateStatusEffectGraphic();
        }
        if (wetTime > 0)
            wetTime -= Time.deltaTime;
        else
        {
            activeEffects.Remove(wetSlow);
            DeactivateStatusEffectGraphic();
        }
        if (combineEffectTimer > 0)
        {
            combineEffectTimer -= Time.deltaTime;
        }
        else if (CurrentCombinedStatusEffect != CombinedStatusEffect.None)
        {
            CurrentCombinedStatusEffect = CombinedStatusEffect.None;
            DeactivateStatusEffectGraphic();
        }
    }
    private void DeactivateStatusEffectGraphic()
    {
        if (wetTime > 0 || burnTime > 0 || dirtedTime > 0 || combineEffectTimer > 0)
            return;
        statusEffectCon.ClearEffect();
    }
    private StatusEffect wetSlow = new StatusEffect(999f, 0.7f);
    private StatusEffect dirtedStun = new StatusEffect(999f);
    #endregion

    //related to ability
    public Action PreDestruction;
    public List<AbilityUpdateFunc> AbilityUpdates = new();
    public Func<float, TowerData, float> PreMitiDmgFunc;
    public float isUntargetable;
    public bool IsUntargetable => isUntargetable > 0;
    public Action<PathType> EnteringTile;

    //tile and path
    public Vector2 CurrentPositionInAbs { get; private set; }
    private PathType currentStandingPathType = PathType.None;

    //ref
    private GameObject hpBarCover;
    private WaveMove moveScript;
    private Transform spineAnimation;
    private StatusEffectCon statusEffectCon;

    private float initialScale;
    private bool flipX;

    void Awake()
    {
        hpBarCover = transform.Find("health/cover").gameObject;
        moveScript = GetComponent<WaveMove>();
        spineAnimation = transform.Find("spine_animation");
        statusEffectCon = GetComponentInChildren<StatusEffectCon>();
    }
    /*
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
    */

    public void Init(EnemyData data)
    {
        this.data = data;
        maxHealth = data.maxHp;
        currentHp = maxHealth;
        maxSpeed = data.baseMoveSpeed;
        currentSpeed = maxSpeed;
        UpdateHp(0, Color.white); //to reset hp bar

        //spine init
        spineAnimation.GetComponent<SpineAnimationController>().Init(data);
        spineAnimation.GetComponent<SpineAnimationController>().SetSkinName(data.skinName);
        initialScale = spineAnimation.transform.localScale.y;

        //init wave move script
        List<Vector2> wps = new(FindFirstObjectByType<Waypoints>().waypoints[0].points);
        Action onexit = () =>
        {
            GameManager.Instance.TakeDame();
            PoolManager.Instance.ReturnEnemy(gameObject);
        };
        moveScript.Init(wps, onexit);

        //idk but this should be be4 ability
        CurrentPositionInAbs = new Vector2(69, 420);
        CurrentCombinedStatusEffect = CombinedStatusEffect.None;
        isUntargetable = 0;
        PreDestruction = null;
        EnteringTile = null;
        AbilityUpdates = new();
        PreMitiDmgFunc = (d, s) => d;
        ResetAllStatusEffect();

        //init ability
        EnemyAbilityLibrary.Instance.GetAbility(this, data.ability);
    }

    private void Update()
    {
        
        // Effect Apply and Undo
        UpdateMovementEffect();
        UpdateBasicEffect();
        if (isUntargetable > 0)
            isUntargetable -= Time.deltaTime;

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
    public bool UpdateHp(float value, Color color)
    {
        currentHp += value;
        currentHp = Mathf.Clamp(currentHp, 0, maxHealth);
        if (currentHp == 0)
        {
            PreDestruction?.Invoke();

            PoolManager.Instance.ReturnEnemy(gameObject);
            //Destroy(gameObject);
            //EventManager.Instance.ModiGold(enemyEquivalent * 10f);
            return true;
        }
        if (value != 0)
        {
            DmgNumberManager.Instance.DmgNumber(color, Mathf.Abs(value), transform.position);
        }

        Vector2 scale = new Vector2(1 - (currentHp / maxHealth), 1);
        hpBarCover.transform.localScale = scale;
        return false;
    }

    public bool PreMitiDmg(float dmg, TowerData attackData)
    {
        //print(dmg);
        var mul = CounterElement(attackData.element, data.element);
        var color = "#EEEEEE".HexColor();
        if (mul == 1.2f) color = "#FFDD44".HexColor();
        if (mul == 0.7f) color = "#888888".HexColor();

        dmg *= mul;
        dmg = PreMitiDmgFunc(dmg, attackData);
        if (dirtedTime > 0)
        {
            dirtedTime = 0;
            activeEffects.Remove(dirtedStun);
            DeactivateStatusEffectGraphic();
            dmg *= 2;
        }
        if (CurrentCombinedStatusEffect == CombinedStatusEffect.Glutinous)
        {
            dmg *= 1.2f;
        }

        //print(dmg);
        return UpdateHp(-dmg, color);
    }
    private float CounterElement(Element e1, Element e2)
    {
        if (e1 == Element.Fire && e2 == Element.Water) { return 0.7f; }
        if (e1 == Element.Water && e2 == Element.Fire) { return 1.2f; }
        if (e1 == Element.Water && e2 == Element.Earth) { return 0.7f; }
        if (e1 == Element.Earth && e2 == Element.Water) { return 1.2f; }
        if (e1 == Element.Earth && e2 == Element.Fire) { return 0.7f; }
        if (e1 == Element.Fire && e2 == Element.Earth) { return 1.2f; }
        return 1f;
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
        //pathManager.UndoPathEffect(this.gameObject, currentStandingPathType);
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

    private void HandleVisibleStatusEffect(Element element)
    {
        switch (element)
        {
            case Element.Earth: dirtedTime = 3f; statusEffectCon.Rock(); break;
            case Element.Water: wetTime = 3f; statusEffectCon.Wet(); break;
            case Element.Fire: burnTime = 3f; statusEffectCon.Burn(); break;
        }
        if (dirtedTime > 0 && wetTime > 0)
        {
            dirtedTime = 0;
            wetTime = 0;
            combineEffectTimer = 7f;
            CurrentCombinedStatusEffect = CombinedStatusEffect.Glutinous;
            activeEffects.Add(new(7f, 0.3f));
            statusEffectCon.Mud();
        }
        else if (dirtedTime > 0 && burnTime > 0)
        {
            dirtedTime = 0;
            burnTime = 0;
            combineEffectTimer = 5f;
            CurrentCombinedStatusEffect = CombinedStatusEffect.Crystalized;
            activeEffects.Add(new(5f));
            statusEffectCon.Crystal();
        }
        else if (burnTime > 0 && wetTime > 0)
        {
            burnTime = 0;
            wetTime = 0;
            combineEffectTimer = 0.5f;
            CurrentCombinedStatusEffect = CombinedStatusEffect.Combustion;
        }
    }

    #endregion

}

//move here for easier managing
[System.Serializable]
public class StatusEffect
{
    public STATUS_EFFECT status;
    public float duration;
    public float hashMul;
    public float msPercentage;
    /// <summary>
    /// Constructor for stun, just need duration
    /// </summary>
    /// <param name="duration"></param>
    public StatusEffect(float duration)
    {
        status = STATUS_EFFECT.Stun;
        this.duration = duration;
        hashMul = duration;
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
        hashMul = duration;
    }

    public override int GetHashCode()
    {
        return status.GetHashCode() ^ hashMul.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        if (obj is StatusEffect other)
        {
            return hashMul == other.hashMul && status == other.status;
        }
        return false;
    }
}

public enum STATUS_EFFECT
{
    None,
    MovespeedChange,
    Stun,
}
public enum CombinedStatusEffect
{
    None,
    Crystalized,
    Glutinous, 
    Combustion
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