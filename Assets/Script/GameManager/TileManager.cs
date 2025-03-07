using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : Singleton<TileManager>
{
    [SerializeField]
    List<TileEntity> tiles = new List<TileEntity>();

    [SerializeField]
    List<TowerData> listData = new List<TowerData>();

    //[SerializeField] Button spawnTower;

    public void OnStart()
    {
        InitAllTiles();
        /* Already linked this from CanvasAction/Content1/GameObject/Button
        spawnTower.onClick.AddListener(SpawnRandomTile);*/
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
            GameObject tower = PoolManager.Instance.GetTowerFromPool();
            tower.GetComponent<TowerStat>().Init(listData.GetRandom());
            tower.transform.position = tile.transform.position;
            tower.SetActive(true);
        }
        else
        {
            Debug.Log("Không còn chỗ trống để spawn");
        }
    }
}
