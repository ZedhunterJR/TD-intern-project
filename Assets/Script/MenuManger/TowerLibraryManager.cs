using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerLibraryManager : Singleton<TowerLibraryManager>
{
    [SerializeField] List<Button> allButtons;
    private List<TowerData> towerDatas;

    [SerializeField] Sprite fire, water, earth;

    [SerializeField] Transform rightPanel;

    public void Init()
    {
        towerDatas = new List<TowerData>();

        towerDatas.AddRange(Resources.LoadAll<TowerData>("TowerData"));

        for (int i = 0; i < towerDatas.Count; i++)
        {
            int index = i;
            AddFuncToButton(allButtons[index], towerDatas[index]);
        }

        var firstValue = towerDatas.First();
        rightPanel.Find("title").GetComponent<TextMeshProUGUI>().text = firstValue.towerName;
        rightPanel.Find("des").GetComponent<TextMeshProUGUI>().text = firstValue.description;
        rightPanel.Find("icon").GetComponent<Image>().sprite = firstValue.towerSprite;
        rightPanel.Find("dame").GetComponent<TextMeshProUGUI>().text = $"         {firstValue.baseDamage[0].ToString()} => {firstValue.baseDamage[1].ToString()} => {firstValue.baseDamage[2].ToString()}";
        rightPanel.Find("ar").GetComponent<TextMeshProUGUI>().text = $"         {firstValue.range[0].ToString()} => {firstValue.range[1].ToString()} => {firstValue.range[2].ToString()}";
        rightPanel.Find("as").GetComponent<TextMeshProUGUI>().text = $"         {firstValue.baseAtkSpd[0].ToString()} => {firstValue.baseAtkSpd[1].ToString()} => {firstValue.baseAtkSpd[2].ToString()}";

        var elementImg = rightPanel.Find("element").GetComponent<Image>();
        switch (firstValue.element)
        {
            case Element.Earth: elementImg.sprite = earth; break;
            case Element.Water: elementImg.sprite = water; break;
            case Element.Fire: elementImg.sprite = fire; break;
        }
    }

    void AddFuncToButton(Button button, TowerData towerData)
    {
        button.transform.Find("sprite").GetComponent<Image>().sprite = towerData.towerSprite;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            rightPanel.Find("title").GetComponent<TextMeshProUGUI>().text = towerData.towerName;
            rightPanel.Find("des").GetComponent<TextMeshProUGUI>().text = towerData.description;
            rightPanel.Find("icon").GetComponent<Image>().sprite = towerData.towerSprite;
            rightPanel.Find("dame").GetComponent<TextMeshProUGUI>().text = $"         {towerData.baseDamage[0].ToString()} => {towerData.baseDamage[1].ToString()} => {towerData.baseDamage[2].ToString()}";
            rightPanel.Find("ar").GetComponent<TextMeshProUGUI>().text = $"         {towerData.range[0].ToString()} => {towerData.range[1].ToString()} => {towerData.range[2].ToString()}";
            rightPanel.Find("as").GetComponent<TextMeshProUGUI>().text = $"         {towerData.baseAtkSpd[0].ToString()} => {towerData.baseAtkSpd[1].ToString()} => {towerData.baseAtkSpd[2].ToString()}";

            var elementImg = rightPanel.Find("element").GetComponent<Image>();
            switch (towerData.element)
            {
                case Element.Earth: elementImg.sprite = earth; break;
                case Element.Water: elementImg.sprite = water; break;
                case Element.Fire: elementImg.sprite = fire; break;
            }
        });
    }
}
