using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnerEvent 
{
    /*
    private GameObject spawner;
    public int currentWave = 0;

    private int wavePower;
    private float waveInterval = 10;
    private float waveTimer = 5;
    private TextMeshProUGUI waveText;
    private List<GameObject> unlockedEnemy = new List<GameObject>();
    private List<WaveData> allSets;
    private int wayPointNum = 1;

    public SpawnerEvent(GameObject spawner, TextMeshProUGUI text)
    {
        wavePower = 5;
        this.spawner = spawner;
        waveText = text;
        //find reference by name or some shit idk
        allSets = new List<WaveData>();
        allSets.AddRange(Resources.LoadAll<WaveData>("EnemyWave"));
    }

    private void IncrementWave()
    {
        currentWave++;
        waveText.text = currentWave.ToString();
        waveTimer = 0;
        waveInterval += 0.5f * (currentWave / 10) + 1;
        wavePower += 1 * (currentWave / 10) + 1;
        switch (currentWave)
        {
            case 1:
                unlockedEnemy.Add(Resources.Load<GameObject>("Enemy/slime_0"));
                break;
            case 3:
                unlockedEnemy.Add(Resources.Load<GameObject>("Enemy/rat"));
                break;
            case 6:
                unlockedEnemy.Add(Resources.Load<GameObject>("Enemy/bat"));
                break;
            case 10:
                unlockedEnemy.Add(Resources.Load<GameObject>("Enemy/kobold"));
                unlockedEnemy.Add(Resources.Load<GameObject>("Enemy/slime_1"));
                wayPointNum++;
                //crystals[0].SetActive(true);
                break;
            case 12:
                unlockedEnemy.Add(Resources.Load<GameObject>("Enemy/ghost"));
                break;
            case 16:
                unlockedEnemy.Add(Resources.Load<GameObject>("Enemy/skelly_0"));
                unlockedEnemy.Add(Resources.Load<GameObject>("Enemy/rat_1"));
                break;
            case 20:
                unlockedEnemy.Add(Resources.Load<GameObject>("Enemy/slime_2"));
                wayPointNum++;
                //crystals[1].SetActive(true);
                break;
            case 25:
                {
                    //GameObject.Find("Audio Source").GetComponent<AudioSource>().clip = bossBattleMusic;
                    //GameObject.Find("Audio Source").GetComponent<AudioSource>().Play();
                    SpawnEngine engine = spawner.AddComponent<SpawnEngine>();
                    engine.Init(Resources.Load<WaveData>("EnemyWave/boss_0"), wayPointNum, 0);
                    break;
                }
            case 40:
                {
                    SpawnEngine engine = spawner.AddComponent<SpawnEngine>();
                    engine.Init(Resources.Load<WaveData>("EnemyWave/boss_0"), wayPointNum, 0);
                    break;
                }
            case 60:
                {
                    for (int i = 0; i < 2; i++)
                    {
                        SpawnEngine engine = spawner.AddComponent<SpawnEngine>();
                        engine.Init(Resources.Load<WaveData>("EnemyWave/boss_0"), wayPointNum, 0);
                    }
                    break;
                }

        }
        SpawnWave();
    }
    //2s delay between each "spawn"
    private void SpawnWave()
    {
        List<WaveData> thisWave = new List<WaveData>();
        List<WaveData> avaiSets = new List<WaveData>();
        foreach (WaveData set in allSets)
        {
            if (unlockedEnemy.Contains(set.enemy))
                avaiSets.Add(set);
        }
        int power = wavePower;
        while (power > 0)
        {
            WaveData set = power.randomWaveData(avaiSets);
            if (set == null)
                break;
            thisWave.Add(set);
            power -= set.equivalent;
        }
        for (int i = 0; i < thisWave.Count; i++)
        {
            SpawnEngine engine = spawner.AddComponent<SpawnEngine>();
            engine.Init(thisWave[i], wayPointNum, i * 2f);
        }
    }

    public void Update(float deltaTime)
    {
        waveTimer += deltaTime;
        if (waveTimer > waveInterval)
            IncrementWave();
    }
    */
}
