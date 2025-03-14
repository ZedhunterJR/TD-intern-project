using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ExplosionAdvanced : MonoBehaviour
{
    // Reference to all enemies
    [HideInInspector] public List<GameObject> AllEnemies = new();

    // Events
    public Action PreDestruct = null;
    public Action UpdateFunc = null;
    public Action<GameObject> HitEvent = null;

    // Properties
    public float lifeSpan = 0.5f;
    public int pierce = 99;
    public float delayFirstDmgInstance = 0.2f;
    public float dmgInterval = 0.5f;
    public float explosionRadius = 3f; // Explosion detection radius

    // Private Variables
    private float lifeSpanCountDown;
    private int runTimePierce;
    private float dmgCountDown;
    private List<GameObject> enemiesInRange = new();

    // Start is called before the first frame update
    void OnEnable()
    {
        lifeSpanCountDown = lifeSpan;
        runTimePierce = pierce;
        dmgCountDown = delayFirstDmgInstance;
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        lifeSpanCountDown -= Time.deltaTime;
        dmgCountDown -= Time.deltaTime;

        // Update the enemies in range
        UpdateEnemiesInRange();

        // Damage enemies in range at intervals
        if (dmgCountDown < 0)
        {
            DamageEnemies();
            dmgCountDown = dmgInterval; // Reset damage interval
        }

        // Custom update function
        UpdateFunc?.Invoke();

        // Destroy the explosion after lifespan ends
        if (lifeSpanCountDown < 0)
        {
            DestroyObj();
        }
    }

    private void UpdateEnemiesInRange()
    {
        float explosionRadiusSqr = explosionRadius * explosionRadius; // Use squared distance for efficiency
        enemiesInRange.Clear(); // Refresh enemies in range

        foreach (var enemy in new List<GameObject>(AllEnemies))
        {
            if (!AllEnemies.Contains(enemy)) continue; // Skip null enemies

            float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance <= explosionRadiusSqr) // If within explosion radius
            {
                enemiesInRange.Add(enemy);
            }
        }
    }

    private void DamageEnemies()
    {
        foreach (GameObject enemy in enemiesInRange)
        {
            if (!AllEnemies.Contains(enemy))
                continue;
            if (runTimePierce <= 0) break;
            if (enemy != null)
            {
                HitEvent?.Invoke(enemy);
                runTimePierce--;
            }
        }
    }

    private void DestroyObj()
    {
        PreDestruct?.Invoke();
        //Destroy(gameObject);
    }
}
