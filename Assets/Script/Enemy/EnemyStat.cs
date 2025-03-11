using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public EnemyData data;

    public bool immuneToDebuff;
    public float maxHealth;
    public float currentHp;
    public float moveSpeed;

    public List<MovementDebuff> debuffList = new List<MovementDebuff>();
    public bool isStunned;

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
    void Update()
    {
        if (!immuneToDebuff)
        {
            bool stun = false;
            float slowPer = 0;
            for (int i = debuffList.Count - 1; i >= 0; i--)
            {
                if (debuffList[i].Timer())
                {
                    debuffList.RemoveAt(i);
                    continue;
                }
                if (debuffList[i].stunTimer > 0)
                    stun = true;
                if (debuffList[i].slowTimer > 0 && debuffList[i].slowPercentage > slowPer)
                    slowPer = debuffList[i].slowPercentage;
            }

            isStunned = stun;
            moveSpeed = data.baseMoveSpeed * (1 - slowPer);

            //Manage movement and rotation
            moveScript.moveSpeed = moveSpeed;
            moveScript.isStunned = isStunned;
            if (flipX != moveScript.FlipX)
            {
                flipX = moveScript.FlipX;
                spineAnimation.localScale = new Vector3(initialScale * (flipX ? -1 : 1), initialScale, initialScale);
            }
            
        }

        UpdatePathPosition();
    }
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

}

//move here for easier managing
public class MovementDebuff
{
    public float slowPercentage;
    public float slowTimer;

    public float stunTimer;

    public MovementDebuff(float slowPercentage, float slowTimer, float stunTimer)
    {
        this.slowPercentage = slowPercentage;
        this.slowTimer = slowTimer;
        this.stunTimer = stunTimer;
    }

    public bool Timer()
    {
        slowTimer -= Time.deltaTime;
        stunTimer -= Time.deltaTime;

        return slowTimer < 0 && stunTimer < 0;
    }
}
