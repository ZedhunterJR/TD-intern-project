using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWhenDeath : MonoBehaviour
{
    private EnemyStat stat;
    public GameObject spawn;
    public int number;
    // Start is called before the first frame update
    void Start()
    {
        stat = GetComponent<EnemyStat>();
        stat.PreDestruction += Summon;
    }

    void Summon(Vector2 pos)
    {
        for (int i = 0; i < number; i++)
        {
            Vector2 ran = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            ran.Normalize();
            GameObject ins = Instantiate(spawn,
                pos + ran, Quaternion.identity);
            ins.GetComponent<WaveMove>().isSummoned = true;
            ins.GetComponent<WaveMove>().waypointIndex = gameObject.GetComponent<WaveMove>().waypointIndex;
            ins.GetComponent<WaveMove>().waypointNum = gameObject.GetComponent<WaveMove>().waypointNum;
        }
    }
}
