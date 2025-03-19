using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : Singleton<MergeManager>
{
    private Queue<ButtonUI> listButtons = new();

    [SerializeField] private GameObject mergeButtonPrefabs;
    [SerializeField] private Transform worldCanvas;

    ButtonUI currentDragingButton = null;

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
           
           
        };
        buttonUI.MouseDragBegin = () =>
        {
            Debug.Log($"Begin = {buttonUI.transform.position}");
            currentDragingButton = buttonUI;

            Debug.Log($"currentDragingButton {currentDragingButton == null}");
        };

        buttonUI.MouseDragEnd = () =>
        {
            currentDragingButton = null;
        };

        buttonUI.MouseDrop = () =>
        {
            if (currentDragingButton == null) return;
            Debug.Log("Have Button");

            if (currentDragingButton == buttonUI) return;
            Debug.Log("Different");

            TowerStat towerBegin = TileManager.Instance.GetTowerStatInTile(currentDragingButton.transform.position);
            Debug.Log($"Tower Begin = {towerBegin == null}");

            TowerStat towerEnd = TileManager.Instance.GetTowerStatInTile(buttonUI.transform.position);
            Debug.Log($"Tower End = {towerEnd == null}");

            if(towerBegin.data == towerEnd.data && towerBegin.level == towerEnd.level)
            {
                TileManager.Instance.RemoveTileInDic(currentDragingButton.transform.position);
                ReturnButtonUI(currentDragingButton);

                towerEnd.LevelUp();
            }
        };
    }
}
