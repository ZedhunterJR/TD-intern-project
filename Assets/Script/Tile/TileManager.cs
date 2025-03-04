using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    List<TileEntity> tiles = new List<TileEntity>();

    [SerializeField] GameObject tower;

    private void Start()
    {
        InitAllTiles();
    }

    private void Update()
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
            TileEntity tile = tilesNoneTower[Random.Range(0, tilesNoneTower.Count)];
            tile.ChangeStatus(TILE_BUILDING_STATUS.HasTower);

            Debug.Log($"Tháp đã được đặt là {tile.name}");
        }
        else
        {
            Debug.Log("Không còn chỗ trống để spawn");
        }
    }
}
