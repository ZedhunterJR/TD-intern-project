using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public EnemyData data;

    public float maxHealth;
    public float currentHp;
    public float moveSpeed;
    public float currentSpeed;

    // Buff and DeBuff List 
    [SerializeField]
    List<StatusEffect> activeEffects = new List<StatusEffect>();
    public bool isStunned;
    StatusEffect statusEffect;

    public Action<Vector2> PreDestruction;

    private Vector2 currentPositionInAbs = new Vector2(-69, 42);
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

    public void Init(EnemyData data)
    {
        this.data = data;
        maxHealth = data.maxHp;
        currentHp = maxHealth;
        moveSpeed = data.baseMoveSpeed;
        currentSpeed = moveSpeed;
        UpdateHp(0); //to reset hp bar

        //spine init
        spineAnimation.GetComponent<SpineAnimationController>().Init(data);
        initialScale = spineAnimation.transform.localScale.y;

        //init wave move script
        List<Vector2> wps = new(FindFirstObjectByType<Waypoints>().waypoints[0].points);
        Action onexit = () =>
        {
            GameManager.Instance.TakeDame();
            PoolManager.Instance.RespawnObject(OBJ_TYPE.enemyTest, gameObject);
        };
        moveScript.Init(wps, onexit);

        
    }

    // Update is called once per frame
    public void OnStart()
    {
        statusEffect = new StatusEffect() { status = STATUS_EFFECT.Slow, duration = 2f };
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
        ApplyEffects();
        UpdateEffect();

        //Manage movement and rotation
        if (!isStunned)
            moveScript.MoveUpdate(currentSpeed);
        if (flipX != moveScript.FlipX)
        {
            flipX = moveScript.FlipX;
            spineAnimation.localScale = new Vector3(initialScale * (flipX ? -1 : 1), initialScale, initialScale);
        }

        UpdatePathPosition();
    }

    #region HP,Pos Update
    public void UpdateHp(float value)
    {
        currentHp += value;
        currentHp = Mathf.Clamp(currentHp, 0, maxHealth);
        if (currentHp == 0)
        {
            PreDestruction?.Invoke(transform.position);

            PoolManager.Instance.RespawnObject(OBJ_TYPE.enemyTest, gameObject);
            //Destroy(gameObject);
            //EventManager.Instance.ModiGold(enemyEquivalent * 10f);
        }

        Vector2 scale = new Vector2(1 - (currentHp / maxHealth), 1);
        hpBarCover.transform.localScale = scale;
    }

    public void PreMitiDmg(float dmg)
    {
        //print(dmg);
        UpdateHp(-dmg);
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

        if (snappedPos == currentPositionInAbs)
            return;

        // Continue
        var pathManager = PathManager.Instance;
        var pType = pathManager.GetCurrentStandingPath(snappedPos);
        currentPositionInAbs = snappedPos;

        if (pType == currentStandingPathType)
            return;

        // And continue
        pathManager.UndoPathEffect(this.gameObject, currentStandingPathType);
        pathManager.ApplyPathEffect(gameObject, pType);
        currentStandingPathType = pType;
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
        StatusEffect existingEffect = activeEffects.Find(e => e.status == effect.status);
        if (existingEffect != null)
        {
            existingEffect.duration = Mathf.Max(existingEffect.duration, effect.duration);
        }
        else
            activeEffects.Add(effect);
    }

    private void ApplyEffects()
    {
        currentSpeed = moveSpeed;

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
            currentSpeed = moveSpeed;
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
}

public enum STATUS_EFFECT
{
    None,
    Slow,
    Stun,
}
