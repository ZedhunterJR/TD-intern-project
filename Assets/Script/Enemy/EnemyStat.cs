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

    //ref
    private GameObject hpBarCover;

    void Awake()
    {
        hpBarCover = transform.Find("health/cover").gameObject;
    }

    public void Init(EnemyData data)
    {
        this.data = data;
        maxHealth = data.maxHp;
        currentHp = maxHealth;
        moveSpeed = data.baseMoveSpeed;

        //sprite and other bs
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
        }
    }
    public void UpdateHp(float value)
    {
        currentHp -= value;
        currentHp = Mathf.Clamp(currentHp, 0, maxHealth);
        if (currentHp == 0)
        {
            PreDestruction?.Invoke(transform.position);
            EnemyManager.Instance.RemoveEnemy(gameObject);
            Destroy(gameObject);
            //EventManager.Instance.ModiGold(enemyEquivalent * 10f);
        }

        Vector2 scale = new Vector2(1 - (currentHp / maxHealth), 1);
        hpBarCover.transform.localScale = scale;
    }

    public void PreMitiDmg(float dmg)
    {
        UpdateHp(dmg);
    }
    
}
