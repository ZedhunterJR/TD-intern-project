using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileEntityUI : MonoBehaviour
{
    [SerializeField] Image tileImage;
    [SerializeField] Sprite tileSprite;
    private TILE_BUILDING_STATUS status;

    //public TILE_BUILDING_STATUS Status => status;

    public void OnStart()
    {
        Init();
    }

    private void Init()
    {
        tileImage.sprite = tileSprite;
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