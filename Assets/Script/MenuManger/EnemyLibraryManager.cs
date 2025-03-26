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

    [SerializeField] Sprite lockSprite;

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
                allButtons[i].GetComponent<Image>().sprite = lockSprite; 
            }
            else
            {
                AddFuncToButton(allButtons[index], keyLibrary[index]);
            }
        }
    }

    void AddFuncToButton(Button button, EnemyLibraryData enemy)
    {
        button.GetComponent<Image>().sprite = enemy.sprite;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            rightPanel.Find("title").GetComponent<TextMeshProUGUI>().text = enemy.enemyName;
            rightPanel.Find("des").GetComponent<TextMeshProUGUI>().text = enemy.description;
            rightPanel.Find("img").GetComponent<Image>().sprite = enemy.sprite;
            rightPanel.Find("hp").GetComponent<TextMeshProUGUI>().text = enemy.enemyHealth.ToString();
            rightPanel.Find("ms").GetComponent<TextMeshProUGUI>().text = enemy.enemyMoveSpeed.ToString();
        });
    }
}
