using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEngine : MonoBehaviour
{
    public void Init(WaveData data, int num, float delay)
    {
        set = data;
        waypointNum = num;
        this.delay = delay;
    }
    private WaveData set;

    private float delay;
    private int waypointCount = 0;
    private int waypointNum;

    private int enemyCount = 0;
    private float spawnInterval = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delay -= Time.deltaTime;
        if (delay < 0)
        {
            spawnInterval += Time.deltaTime;
            if (spawnInterval > set.interval)
            {
                GameObject enemy = Instantiate(set.enemy);
                enemy.GetComponent<WaveMove>().waypointNum = waypointCount;
                waypointCount++;
                if (waypointCount >= waypointNum)
                {
                    waypointCount = 0;
                }
                spawnInterval = 0;
                enemyCount++;
            }
            if (enemyCount >= set.number)
            {
                Destroy(this);
            }
        }
    }
}
