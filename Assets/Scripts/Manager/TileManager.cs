using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    [SerializeField] List<TileEntity> tileBuilding = new List<TileEntity>();
    [SerializeField] List<TileEntityUI> tileBuildingUI = new List<TileEntityUI>();

    public void OnStart()
    {
        InitAllTile();
    }

    void InitAllTile()
    {
        foreach (var t in tileBuilding)
        {
            t.OnStart();
        }
        foreach (var t in tileBuildingUI)
        {
            t.OnStart();
        }
    }
}
