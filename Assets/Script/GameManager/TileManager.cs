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
    Dictionary<Vector2, TileEntity> tilesDic = new Dictionary<Vector2, TileEntity>();

    [SerializeField]
    List<TowerData> listData = new List<TowerData>();

    //[SerializeField] Button spawnTower;

    private void Start()
    {
        InitAllTiles();
        /* Already linked this from CanvasAction/Content1/GameObject/Button
        spawnTower.onClick.AddListener(SpawnRandomTile);*/
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //    SpawnRandomTile();
    }

    void InitAllTiles()
    {
        tiles = new();
        tiles.AddRange(FindObjectsOfType<TileEntity>());
        foreach (var tile in tiles)
        {
            tile.Init();
        }
    }

    public void SpawnRandomTile()
    {
        List<TileEntity> tilesNoneTower = tiles.Where(n => n.currentTower == null).ToList();
        List<TileEntity> tilesHasTower = tiles.Where(n => n.currentTower != null).ToList();

        if (tilesNoneTower.Count > 0 && tilesHasTower.Count < GameManager.Instance.PlayerData.maxTileBuilding)
        {
            TileEntity tile = tilesNoneTower.GetRandom();

            //TestEnemyAndTowerSpawn.Instance.SpawnTower(tile.transform.position);
            GameObject tower = PoolManager.Instance.GetTowerFromPool();
            tower.transform.position = tile.transform.position;
            tower.GetComponent<TowerStat>().Init(listData.GetRandom());
            //tower.SetActive(true);
            tile.currentTower = tower.GetComponent<TowerStat>();
            tilesDic.Add(tile.transform.position, tile);
            MergeManager.Instance.SetButtonPos(tile.transform.position);
        }
        else
        {
            Debug.Log("Không còn chỗ trống để spawn");
        }
    }

    public void RemoveTileInDic(Vector2 pos)
    {
        try
        {
            TileEntity tileEntity = tilesDic[pos];
            PoolManager.Instance.ReturnTower(tileEntity.currentTower.gameObject);
            tileEntity.currentTower = null;
            tilesDic.Remove(pos);
        }
        catch 
        {
            Debug.Log("000");
        }
    }

    public TowerStat GetTowerStatInTile(Vector2 pos)
    {
        try
        {
            TileEntity tileEntity = tilesDic[pos];
            return tileEntity.currentTower;
        }
        catch
        {
            return null;
        }
    }
}

