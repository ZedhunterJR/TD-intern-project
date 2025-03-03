using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public bool immuneToDebuff;
    public float maxHealth;
    [HideInInspector]
    public float currentHp;
    private GameObject hpBarCover;

    public List<MovementDebuff> debuffList = new List<MovementDebuff>();
    public bool isStunned;

    public float moveSpeed;

    [HideInInspector]
    public float baseMoveSpeed;

    public float enemyEquivalent;

    public Action<Vector2> PreDestruction;
    // Start is called before the first frame update
    void Start()
    {
        //int currentWave = EventManager.Instance.spawnerEvent.currentWave;
        //maxHealth *= Mathf.Pow(1.022f, currentWave);
        //moveSpeed *= Mathf.Pow(1.02f, currentWave);
        hpBarCover = transform.Find("health/cover").gameObject;
        baseMoveSpeed = moveSpeed;
        currentHp = maxHealth;
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
            moveSpeed = baseMoveSpeed * (1 - slowPer);
        }
    }
    public void UpdateHp(float value)
    {
        currentHp -= value;
        currentHp = Mathf.Clamp(currentHp, 0, maxHealth);
        if (currentHp == 0)
        {
            PreDestruction?.Invoke(transform.position);
            //EventManager.Instance.ModiGold(enemyEquivalent * 10f);
            gameObject.DestroyWithTween();
        }

        Vector2 scale = new Vector2(1 - (currentHp / maxHealth), 1);
        hpBarCover.transform.localScale = scale;
    }

    public void PreMitiDmg(float dmg)
    {
        UpdateHp(dmg);
    }
    
}
