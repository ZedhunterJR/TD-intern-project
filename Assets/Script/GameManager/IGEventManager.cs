using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IGEventManager : Singleton<IGEventManager>
{
    [SerializeField] private GameObject[] cardPanels;
    [SerializeField] private List<IGEventData> events = new();

    private bool hasChosen = false;

    public void ActivateCardPanel()
    {
        var eventChose = events.GetRandom(3);
        for (int i = 0; i < 3; i++)
        {
            int index = i;
            DecoratePanel(cardPanels[index], eventChose[index]);
        }
        hasChosen = false;
    }

    private void DecoratePanel(GameObject panel, IGEventData data)
    {
        var title = panel.transform.Find("title").GetComponent<TextMeshProUGUI>();
        var des = panel.transform.Find("des").GetComponent<TextMeshProUGUI>();
        var icon = panel.transform.Find("icon").GetComponent<Image>();

        title.text = data.title;
        des.text = data.description;
        icon.sprite = data.icon;

        var button = panel.GetComponent<ButtonUI>();
        button.ClickFunc = () =>
        {
            if (hasChosen) return;

            ProcessEvent(data);
            UIManager.Instance.DeactiveEvenPanel();
            hasChosen = true;
        };
    }

    private void ProcessEvent(IGEventData eventData)
    {
        switch (eventData.name)
        {
            case "EVT_001":
                Debug.Log("Stun all Enemies");
                foreach (var enemy in EnemyManager.Instance.AllEnemies)
                {
                    EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
                    enemyStat.ResetAllStatusEffect();
                    enemyStat.StackElement(5, Element.Earth);
                    enemyStat.StackElement(5, Element.Fire);
                }
                break;
            case "EVT_002":
                Debug.Log("Healing 1 Hp");
                GameManager.Instance.Healing(1);
                break;
            case "EVT_003":
                Debug.Log("Increase Energy by 50");
                GameManager.Instance.ModifyGold(50);
                break;
            case "EVT_004":
                break;
            case "EVT_005":
                break;
            case "EVT_006":
                break;
            case "EVT_007":
                break;
            case "EVT_008":
                break;
            case "EVT_009":
                break;
            case "EVT_010":
                break;
        }
    }
}
