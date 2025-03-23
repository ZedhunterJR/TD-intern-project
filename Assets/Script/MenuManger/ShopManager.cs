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

    public void OnAwake()
    {
        UpdateUpgradeCostTileUI();
        UpdateUpgradeCostHeartUI();
        UpdateUpgradeCostAttackUI();
        UpdateUpgradeCostAttackSpeedUI();
    }

    private void Start()
    {
        tileUpgradeLevel.onClick.AddListener(DataManager.Instance.UpgradeTileLevel);
        heartUpgradeLevel.onClick.AddListener(DataManager.Instance.UpgradeHeartLevel);
        attackUpgradeLevel.onClick.AddListener(DataManager.Instance.UpgradeAttackLevel);
        attackSpeedUpgradeLevel.onClick.AddListener(DataManager.Instance.UpgradeAttackSpeedLevel);
    }
    #region UI
    public void UpdateUpgradeCostTileUI()
    {
        int cost = DataManager.Instance.GetCostTileUpgrade();

        if (DataManager.Instance.PlayerData.tileLevel >= 5)
        {
            upgradeTileCostText.text = $"Max Lv";
            levelTileText.text = $"Level {DataManager.Instance.PlayerData.tileLevel}";
            detailTileText.text = $"Current Max Tile Building: {DataManager.Instance.PlayerData.maxTileBuilding}";
            return;
        }

        upgradeTileCostText.text = $"{cost}G";
        levelTileText.text = $"Level {DataManager.Instance.PlayerData.tileLevel}";
        detailTileText.text = $"Max Tile Building: {DataManager.Instance.PlayerData.maxTileBuilding}";
    }

    public void UpdateUpgradeCostHeartUI()
    {
        int cost = DataManager.Instance.GetCostHeartUpgrade();

        if (DataManager.Instance.PlayerData.heartLevel >= 3)
        {
            upgradeHeartCostText.text = $"Max Lv";
            levelHeartText.text = $"Level {DataManager.Instance.PlayerData.heartLevel}";
            detailHeartText.text = $"Max Heart: {DataManager.Instance.PlayerData.maxHeart}";
            return;
        }

        upgradeHeartCostText.text = $"{cost}G";
        levelHeartText.text = $"Level {DataManager.Instance.PlayerData.heartLevel}";
        detailHeartText.text = $"Max Heart: {DataManager.Instance.PlayerData.maxHeart}";
    }

    public void UpdateUpgradeCostAttackUI()
    {
        int cost = DataManager.Instance.GetCostAttackUpgrade();

        if (DataManager.Instance.PlayerData.bonusAttackLevel >= 5)
        {
            upgradeAttackCostText.text = $"Max Lv";
            levelAttackText.text = $"Level {DataManager.Instance.PlayerData.bonusAttackLevel}";
            detailAttackText.text = $"Attack Bonus: {DataManager.Instance.PlayerData.bonusAttack}%";
            return;
        }

        upgradeAttackCostText.text = $"{cost}G";
        levelAttackText.text = $"Level {DataManager.Instance.PlayerData.bonusAttackLevel}";
        detailAttackText.text = $"Attack Bonus: {DataManager.Instance.PlayerData.bonusAttack}%";
    }

    public void UpdateUpgradeCostAttackSpeedUI()
    {
        int cost = DataManager.Instance.GetCostAttackSpeedUpgrade();

        if (DataManager.Instance.PlayerData.bonusAttackSpeedLevel >= 5)
        {
            upgradeAttackSpeedCostText.text = $"Max Lv";
            levelAttackSpeedText.text = $"Level {DataManager.Instance.PlayerData.bonusAttackSpeedLevel}";
            detailAttackSpeedText.text = $"Attack Speed Bonus: {DataManager.Instance.PlayerData.bonusAttackSpeed}%";
            return;
        }

        upgradeAttackSpeedCostText.text = $"{cost}G";
        levelAttackSpeedText.text = $"Level {DataManager.Instance.PlayerData.bonusAttackSpeedLevel}";
        detailAttackSpeedText.text = $"Attack Speed Bonus: {DataManager.Instance.PlayerData.bonusAttackSpeed}%";
    }
    #endregion
}
