using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] TileManager tileManager;
    [SerializeField] EnemyManager enemyManager;

    private void Start()
    {
        if (tileManager != null)
            tileManager.OnStart();
        if(enemyManager != null)
            enemyManager.OnStart();
    }

    private void Update()
    {
        if(tileManager != null)
            tileManager.OnUpdate();
        if(enemyManager != null)
            enemyManager.OnUpdate();
    }
}

public enum GAME_STATUS
{
    Init, 
    Playing, 
    Pause, 
    Win, 
    Lose
}
