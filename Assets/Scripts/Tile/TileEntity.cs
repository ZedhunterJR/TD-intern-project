using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileEntity : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite imageTile;
    private TILE_BUILDING_STATUS status;

    //public TILE_BUILDING_STATUS Status => status;

    public void OnStart()
    {
        Init();
    }

    private void Init()
    {
        spriteRenderer.sprite = imageTile;
        status = TILE_BUILDING_STATUS.none;
    }

    public bool ChangeStatus(TILE_BUILDING_STATUS newStatus)
    {
        if (status != newStatus)
        {
            return true;
        }

        return false;
    }
}

public enum TILE_BUILDING_STATUS
{
    none,
    hasTower,
}