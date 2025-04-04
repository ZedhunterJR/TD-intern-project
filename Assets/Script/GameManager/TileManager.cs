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

    public List<TowerData> ListData => listData;

    private int currentSpawnCost = 10;

    public int CurrentSpawnCost => currentSpawnCost;

    //[SerializeField] Button spawnTower;

    private void Start()
    {
        InitAllTiles();
        UIManager.Instance.UpdateSpawnCostText(currentSpawnCost);
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

        if (tilesHasTower.Count >= GameManager.Instance.playerData.maxTileBuilding)
        {
            Debug.Log("hết chỗ ");
            UIManager.Instance.NoBuildingEffect();
            return;
        }

        if (!GameManager.Instance.ModifyGold(-currentSpawnCost))
        {
            Debug.Log("Không tiền");
            UIManager.Instance.NoCurrencyEffect();
            return;
        }

        if (tilesNoneTower.Count > 0)
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

            currentSpawnCost += 5;
            UIManager.Instance.UpdateSpawnCostText(currentSpawnCost);
        }
        else
        {
            Debug.Log("Không còn chỗ trống để spawn");
        }

        UIManager.Instance.UpdateCurrentBuildingText();
    }

    public void RemoveTileInDic(Vector2 pos)
    {
        try
        {
            TileEntity tileEntity = tilesDic[pos];
            PoolManager.Instance.ReturnTower(tileEntity.currentTower.gameObject);
            UIManager.Instance.UpdateCurrentBuildingText();
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

    public void ReplaceTower(TowerData towerData)
    {
        Debug.Log(towerData == null);
        Element elementTower = towerData.element;

        foreach (var item in new List<TowerData>(listData))
        {
            if(item.element == elementTower)
            {
                listData.Remove(item);
                listData.Add(towerData);
                break;
            }
        }
    }

    public TowerData GetTowerByElement(Element element)
    {
        foreach (var item in new List<TowerData>(listData))
        {
            if (item.element == element)
            {
                return item;
            }
        }

        return null;
    }
}

