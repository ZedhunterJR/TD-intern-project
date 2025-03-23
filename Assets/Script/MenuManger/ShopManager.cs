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
            return;
        }

        upgradeTileCostText.text = $"{cost}G";
    }

    public void UpdateUpgradeCostHeartUI()
    {
        int cost = DataManager.Instance.GetCostHeartUpgrade();

        if (DataManager.Instance.PlayerData.heartLevel >= 3)
        {
            upgradeHeartCostText.text = $"Max Lv";
            return;
        }

        upgradeHeartCostText.text = $"{cost}G";
    }

    public void UpdateUpgradeCostAttackUI()
    {
        int cost = DataManager.Instance.GetCostAttackUpgrade();

        if (DataManager.Instance.PlayerData.bonusAttackLevel >= 5)
        {
            upgradeAttackCostText.text = $"Max Lv";
            return;
        }

        upgradeAttackCostText.text = $"{cost}G";
    }

    public void UpdateUpgradeCostAttackSpeedUI()
    {
        int cost = DataManager.Instance.GetCostAttackSpeedUpgrade();

        if (DataManager.Instance.PlayerData.bonusAttackSpeedLevel >= 5)
        {
            upgradeAttackSpeedCostText.text = $"Max Lv";
            return;
        }

        upgradeAttackSpeedCostText.text = $"{cost}G";
    }
    #endregion
}
