using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] Button tileUpgradeLevel;
    [SerializeField] Button heartUpgradeLevel;
    [SerializeField] Button attackUpgradeLevel;
    [SerializeField] Button attackSpeedUpgradeLevel;


    [SerializeField] private TextMeshProUGUI upgradeTileCostText;
    [SerializeField] private TextMeshProUGUI upgradeHeartCostText;
    [SerializeField] private TextMeshProUGUI upgradeAttackCostText;
    [SerializeField] private TextMeshProUGUI upgradeAttackSpeedCostText;

    [SerializeField] private TextMeshProUGUI levelTileText;
    [SerializeField] private TextMeshProUGUI levelHeartText;
    [SerializeField] private TextMeshProUGUI levelAttackText;
    [SerializeField] private TextMeshProUGUI levelAttackSpeedText;

    [SerializeField] private TextMeshProUGUI detailTileText;
    [SerializeField] private TextMeshProUGUI detailHeartText;
    [SerializeField] private TextMeshProUGUI detailAttackText;
    [SerializeField] private TextMeshProUGUI detailAttackSpeedText;

    [SerializeField] private TextMeshProUGUI currentGoldText;

    public void OnAwake()
    {
    }

    private void Start()
    {
        tileUpgradeLevel.onClick.AddListener(DataManager.Instance.UpgradeTileLevel);
        heartUpgradeLevel.onClick.AddListener(DataManager.Instance.UpgradeHeartLevel);
        attackUpgradeLevel.onClick.AddListener(DataManager.Instance.UpgradeAttackLevel);
        attackSpeedUpgradeLevel.onClick.AddListener(DataManager.Instance.UpgradeAttackSpeedLevel);
    }
    #region UI
    public void UpdateCurrentGoldText(int value)
    {
        currentGoldText.text = value.ToString();
    }

    public void UpdateUpgradeCostTileUI(int level, int value)
    {
        int cost = DataManager.Instance.GetCostTileUpgrade();
        if (level < 5)
            upgradeTileCostText.text = $"{cost}G";
        else upgradeTileCostText.text = "Max Lvl!";
        levelTileText.text = $"{level}";
        detailTileText.text = $"Max Tile Building: {value}";
    }

    public void UpdateUpgradeCostHeartUI(int level, int value)
    {
        int cost = DataManager.Instance.GetCostHeartUpgrade();
        if (level < 2)
            upgradeHeartCostText.text = $"{cost}G";
        else upgradeHeartCostText.text = "Max Lvl!";
        levelHeartText.text = $"{level}";
        detailHeartText.text = $"Max Heart: {value}";
    }

    public void UpdateUpgradeCostAttackUI(int level, float value)
    {
        int cost = DataManager.Instance.GetCostAttackUpgrade();
        if (level < 5)
            upgradeAttackCostText.text = $"{cost}G";
        else upgradeAttackCostText.text = "Max Lvl!";
        levelAttackText.text = $"{level}";
        detailAttackText.text = $"Attack Bonus: {value}%";
    }

    public void UpdateUpgradeCostAttackSpeedUI(int level, float value)
    {
        int cost = DataManager.Instance.GetCostAttackSpeedUpgrade();
        if (level < 5)
            upgradeAttackSpeedCostText.text = $"{cost}G";
        else upgradeAttackSpeedCostText.text = "Max Lvl!";
        levelAttackSpeedText.text = $"Level {level}";
        detailAttackSpeedText.text = $"Attack Speed Bonus: {value}%";
    }
    #endregion
}
