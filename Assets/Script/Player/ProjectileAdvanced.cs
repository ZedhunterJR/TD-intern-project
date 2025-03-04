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
    [HideInInspector] public List<GameObject> targetsHit = new();
    public float LifeSpanInInterpolation { get => 1 - lifeSpanCountDown / lifeSpan; }

    // Private Modifiers
    private float lifeSpanCountDown;

    // Start is called before the first frame update
    void Start()
    {
        lifeSpanCountDown = lifeSpan;
    }

    // Update is called once per frame
    void Update()
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

        foreach (var enemy in AllEnemies)
        {
            if (enemy == null || targetsHit.Contains(enemy)) continue; // Skip null or already hit enemies, might not stay

            float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance <= hitRadiusSqr) // Enemy within hit radius
            {
                targetsHit.Add(enemy);
                HitEvent?.Invoke(enemy);

                pierce--;
                if (pierce == 0)
                {
                    DestroyObj();
                    return;
                }
            }
        }
    }

    private void DestroyObj()
    {
        PreDestruct?.Invoke();
        Destroy(gameObject);
    }
}
