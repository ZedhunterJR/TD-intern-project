using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ExplosionAdvanced : MonoBehaviour
{
    //event
    public Action PreDestruct = null;
    public Action UpdateFunc = null;
    public Action<GameObject> HitEvent = null;

    //property
    public float lifeSpan = 0.5f;
    public int pierce = 99;
    public string enemyTag = "enemy";
    public float delayFirstDmgInstance = 0.2f;
    public float dmgInterval = 0.5f;

    //public modifier
    [HideInInspector] public List<GameObject> enemies = new();

    //private modifier
    private float lifeSpanCountDown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        lifeSpanCountDown = lifeSpan;
    }

    // Update is called once per frame
    void Update()
    {
        lifeSpanCountDown -= Time.deltaTime;
        delayFirstDmgInstance -= Time.deltaTime;

        if (delayFirstDmgInstance < 0)
        {
            foreach (GameObject enemy in enemies)
            {
                if (pierce <= 0)
                    break;
                if (enemy != null)
                {
                    HitEvent?.Invoke(enemy);
                    pierce--;
                }
            }
            delayFirstDmgInstance = dmgInterval;
        }

        UpdateFunc?.Invoke();
        if (lifeSpanCountDown < 0)
        {
            DestroyObj();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Transform enemy = other.gameObject.transform;
        if (enemy != null)
            if (enemy.CompareTag(enemyTag))
            {
                enemies.Add(other.gameObject);
            }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Transform enemy = other.gameObject.transform;
        if (enemy != null)
            if (enemy.CompareTag(enemyTag))
            {
                enemies.Remove(other.gameObject);
            }
    }
    private void DestroyObj()
    {
        PreDestruct?.Invoke();
        Destroy(gameObject);
    }
}
