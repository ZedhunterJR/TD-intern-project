using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLibraryManager : Singleton<EnemyLibraryManager>
{
    [SerializeField] List<Button> allButtons;
    private List<EnemyLibraryData> keyLibrary;
    private List<int> unlockValue;

    [SerializeField] Sprite lockSprite, fire, water, earth;

    [SerializeField] Transform rightPanel;

    private void Awake()
    {
        
    }

    public void Init()
    {
        keyLibrary = new List<EnemyLibraryData>();
        unlockValue = new List<int>();

        var enemyLibrary = EnemyLibrary.Instance.allEnemies;
        foreach (var enemy in enemyLibrary)
        {
            keyLibrary.Add(Resources.Load<EnemyLibraryData>($"EnemyUnlock/{enemy.Key}"));
            unlockValue.Add(enemy.Value);
        }

        for (int i = 0; i < keyLibrary.Count; i++)
        {
            int index = i;
            if (unlockValue[i] == 2)
            {
                allButtons[i].transform.Find("sprite").GetComponent<Image>().sprite = lockSprite; 
            }
            else
            {
                AddFuncToButton(allButtons[index], keyLibrary[index]);
            }
        }
    }

    void AddFuncToButton(Button button, EnemyLibraryData enemy)
    {
        button.transform.Find("sprite").GetComponent<Image>().sprite = enemy.sprite;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            rightPanel.Find("title").GetComponent<TextMeshProUGUI>().text = enemy.enemyName;
            rightPanel.Find("des").GetComponent<TextMeshProUGUI>().text = enemy.description;
            rightPanel.Find("icon").GetComponent<Image>().sprite = enemy.sprite;
            rightPanel.Find("hp").GetComponent<TextMeshProUGUI>().text = enemy.enemyHealth.ToString();
            rightPanel.Find("ms").GetComponent<TextMeshProUGUI>().text = enemy.enemyMoveSpeed.ToString();

            var elementImg = rightPanel.Find("element").GetComponent<Image>();
            switch (enemy.element)
            {
                case Element.Earth: elementImg.sprite = earth; break;
                case Element.Water: elementImg.sprite = water; break;
                case Element.Fire: elementImg.sprite = fire; break;
            }
        });
    }
}
