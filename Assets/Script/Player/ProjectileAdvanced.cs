using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ProjectileAdvanced : MonoBehaviour
{
    // Reference to all enemies
    [HideInInspector] public List<GameObject> AllEnemies = new();

    // Events
    public Action PreDestruct = null;
    public Action UpdateFunc = null;
    public Action<GameObject> HitEvent = null;

    // Properties
    public float lifeSpan = 1;
    public float speed = 10;
    public int pierce = 1;
    public float hitRadius = 0.5f; // Manual collision radius

    // Public Modifiers
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public List<GameObject> targetsHit = new(); //this is very computationally expensive //nvm this is needed
    public float LifeSpanInInterpolation { get => 1 - lifeSpanCountDown / lifeSpan; }

    // Private Modifiers
    private float lifeSpanCountDown;
    private float runtimePierce;

    // Start is called before the first frame update
    void OnEnable()
    {
        lifeSpanCountDown = lifeSpan;
        runtimePierce = pierce;
        targetsHit = new();
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        // Check lifespan expiration
        lifeSpanCountDown -= Time.deltaTime;
        if (lifeSpanCountDown < 0)
        {
            DestroyObj();
            return;
        }

        // Check for enemy collisions manually, if radius == -1, skip
        if (hitRadius != -1)
            CheckForHits();

        // Custom update logic
        UpdateFunc?.Invoke();
    }

    private void CheckForHits()
    {
        float hitRadiusSqr = hitRadius * hitRadius; // Use squared distance for performance

        foreach (var enemy in new List<GameObject>(AllEnemies))
        {
            if (!AllEnemies.Contains(enemy) || targetsHit.Contains(enemy)) continue; // Skip null or already hit enemies, might not stay

            float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance <= hitRadiusSqr) // Enemy within hit radius
            {
                HitEvent?.Invoke(enemy);
                targetsHit.Add(enemy);
                runtimePierce--;
                if (runtimePierce == 0)
                {
                    DestroyObj();
                    return;
                }
            }
        }
    }

    public void DestroyObj()
    {
        PreDestruct?.Invoke();
        //Destroy(gameObject);
    }
}
