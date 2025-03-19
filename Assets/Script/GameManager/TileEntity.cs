using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class TileEntity : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite _sprite;

    [HideInInspector]
    public TowerStat currentTower;

    TILE_BUILDING_STATUS status;

    public TILE_BUILDING_STATUS Status => status; 

    public void Init()
    {
        spriteRenderer.sprite = _sprite;
        status = TILE_BUILDING_STATUS.None;
    }

    public void Init(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        status = TILE_BUILDING_STATUS.None;
    }

    public void ChangeStatus(TILE_BUILDING_STATUS newStatus)
    {
        if (status != newStatus)
        {
            status = newStatus;
            switch (status)
            {
                case TILE_BUILDING_STATUS.None:
                    //spriteRenderer.color = Color.white;
                    break;
                case TILE_BUILDING_STATUS.HasTower:
                    //spriteRenderer.color = Color.black;
                    break;
                default:
                    break;
            }
        }
    }
}

public enum TILE_BUILDING_STATUS
{
    None,
    HasTower,
}
