using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>  
{
    [SerializeField] TileManager tileManager;

    private void Start()
    {
        if (tileManager != null) tileManager.OnStart();
        else Debug.Log($"Chưa gán tile manager");
    }
}
