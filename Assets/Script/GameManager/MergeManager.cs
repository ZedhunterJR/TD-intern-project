using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : Singleton<MergeManager>
{
    private Queue<ButtonUI> listButtons = new();

    [SerializeField] private GameObject mergeButtonPrefabs;
    [SerializeField] private Transform worldCanvas;

    TowerStat currentDragingTowerBeign = null;

    private void Start()
    {
        CreatePool(10);
    }

    private void CreatePool(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject instane = Instantiate(mergeButtonPrefabs, worldCanvas);
            instane.SetActive(false);
            listButtons.Enqueue(instane.GetComponent<ButtonUI>());
        }
    }

    public ButtonUI GetFromPool()
    {
        if(listButtons.Count <= 0)
        {
            CreatePool(5);
            return GetFromPool();
        }

        ButtonUI buttonUI = listButtons.Dequeue();
        return buttonUI;
    }

    public void ReturnButtonUI(ButtonUI buttonUI)
    {
        buttonUI.gameObject.SetActive(false);
        listButtons.Enqueue(buttonUI);
    }

    public void SetButtonPos(Vector2 pos)
    {
        ButtonUI buttonUI = GetFromPool();
        buttonUI.ResetButtonUI();
        buttonUI.gameObject.SetActive(true);

        buttonUI.transform.position = pos;

        buttonUI.ClickFunc = () => 
        { 
            TileManager.Instance.RemoveTileInDic(pos);
            ReturnButtonUI(buttonUI); 
        };
        buttonUI.MouseDragBegin = () =>
        {
            currentDragingTowerBeign = TileManager.Instance.GetTowerStatInTile(pos);
        };

        buttonUI.MouseDragEnd = () =>
        {

        };
    }
}
