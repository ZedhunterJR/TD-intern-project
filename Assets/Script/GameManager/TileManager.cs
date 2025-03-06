using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    [SerializeField]
    List<TileEntity> tiles = new List<TileEntity>();

    private List<OBJ_TYPE> objType = new List<OBJ_TYPE>() 
    {
        OBJ_TYPE.tower_earth,
        OBJ_TYPE.tower_water,
    };

    public void OnStart()
    {
        InitAllTiles();
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
            SpawnRandomTile();
    }

    void InitAllTiles()
    {
        foreach (var tile in tiles)
        {
            tile.Init();
        }
    }

    public void SpawnRandomTile()
    {
        List<TileEntity> tilesNoneTower = tiles.Where(n => n.Status == TILE_BUILDING_STATUS.None).ToList();

        if (tilesNoneTower.Count > 0)
        {
            TileEntity tile = tilesNoneTower.GetRandom();
            tile.ChangeStatus(TILE_BUILDING_STATUS.HasTower);
            //TestEnemyAndTowerSpawn.Instance.SpawnTower(tile.transform.position);
            GameObject tower = PoolManager.Instance.GetPoolObject(objType.GetRandom());
            tower.transform.position = tile.transform.position;
            tower.SetActive(true);
        }
        else
        {
            Debug.Log("Không còn chỗ trống để spawn");
        }
    }
}
