using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : Singleton<DataManager>
{
    private const string PlayerDataKey = "PlayerData";

    private PlayerData.Player playerData => PlayerData.Instance.playerData;

    private void Start()
    {
        LoadPlayerData();
        EnemyLibraryManager.Instance.Init();
        TowerLibraryManager.Instance.Init();

        Debug.Log(playerData == null);
        ShopManager.Instance.UpdateCurrentGoldText(playerData.gold);
        ShopManager.Instance.UpdateUpgradeCostAttackUI(playerData.bonusAttackLevel, playerData.bonusAttack);
        ShopManager.Instance.UpdateUpgradeCostAttackSpeedUI(playerData.bonusAttackSpeedLevel, playerData.bonusAttackSpeed);
        ShopManager.Instance.UpdateUpgradeCostTileUI(playerData.tileLevel, playerData.maxTileBuilding);
        ShopManager.Instance.UpdateUpgradeCostHeartUI(playerData.heartLevel, playerData.maxHeart);
    }

    #region Get and Set fuc
    public int GetCostTileUpgrade()
    {
        return GetTileLevel() * 200;
    }

    public int GetCostHeartUpgrade()
    {
        return GetHeartLevel() * 1000;
    }

    public int GetCostAttackUpgrade()
    {
        return GetAttackLevel() * 100;
    }

    public int GetCostAttackSpeedUpgrade()
    {
        return GetAttackSpeedLevel() * 100;
    }

    public int GetTileLevel()
    {
        if (playerData != null)
        {
            return playerData.tileLevel;
        }

        return 0;
    }

    public int GetHeartLevel()
    {
        if (playerData != null)
        {
            return playerData.heartLevel;
        }

        return 0;
    }

    public int GetAttackLevel()
    {
        if (playerData != null)
        {
            return playerData.bonusAttackLevel;
        }

        return 0;
    }

    public int GetAttackSpeedLevel()
    {
        if (playerData != null)
        {
            return playerData.bonusAttackSpeedLevel;
        }

        return 0;
    }

    #endregion

    #region Change Data Function
    public void IncreaseGold(int gold)
    {
    }

    public void UpgradeTileLevel()
    {
        //if (playerData != null)
        //{
        //    if (playerData.tileLevel >= 5)
        //    {
        //        Debug.Log("Level Max");
        //        return;
        //    }

        //    if (playerData.gold < GetCostTileUpgrade())
        //    {
        //        Debug.Log($"Không đủ tiền {playerData.gold} target {GetCostTileUpgrade()}");
        //        return;
        //    }

        //    playerData.gold -= GetCostTileUpgrade();
        //    playerData.tileLevel++;

        //    if (playerData.tileLevel == 5)
        //    {
        //        playerData.maxTileBuilding = 15;
        //    }
        //    else
        //    {
        //        playerData.maxTileBuilding = 6 + (playerData.tileLevel - 1) * 2;
        //    }

        //    string json = JsonUtility.ToJson(playerData);
        //    PlayerPrefs.SetString(PlayerDataKey, json);

        //    Debug.Log($"Tile Level: {playerData.tileLevel}, Max Tiles: {playerData.maxTileBuilding}, Gold left: {playerData.gold}");
        if (playerData.tileLevel >= 5) return;

        if (playerData.gold < GetCostTileUpgrade()) return;

        playerData.gold -= GetCostTileUpgrade();
        playerData.tileLevel++;
        if (playerData.tileLevel == 5)
        {
            playerData.maxTileBuilding = 15;
        }
        else
        {
            playerData.maxTileBuilding = 6 + (playerData.tileLevel - 1) * 2;
        }



        ShopManager.Instance.UpdateUpgradeCostTileUI(playerData.tileLevel, playerData.maxTileBuilding);
        ShopManager.Instance.UpdateCurrentGoldText(playerData.gold);
        //}
    }

    public void UpgradeHeartLevel()
    {
        if (playerData.heartLevel >= 2) return;

        if (playerData.gold < GetCostHeartUpgrade()) return;

        playerData.gold -= GetCostHeartUpgrade();
        playerData.heartLevel++;
        playerData.maxHeart = 0 + (playerData.heartLevel - 1) * 5;

        // cập nhật UI 
        ShopManager.Instance.UpdateUpgradeCostHeartUI(playerData.heartLevel, playerData.maxHeart);
        ShopManager.Instance.UpdateCurrentGoldText(playerData.gold);
    }

    public void UpgradeAttackLevel()
    {
        if (playerData.bonusAttackLevel >= 5) return;

        if (playerData.gold < GetCostAttackUpgrade()) return;

        playerData.gold -= GetCostAttackUpgrade();
        playerData.bonusAttackLevel++;
        playerData.bonusAttack = 0 + (playerData.bonusAttackLevel - 1) * 5;

        // cập nhật UI 
        ShopManager.Instance.UpdateUpgradeCostAttackUI(playerData.bonusAttackLevel, playerData.bonusAttack);
        ShopManager.Instance.UpdateCurrentGoldText(playerData.gold);
    }

    public void UpgradeAttackSpeedLevel()
    {
        //if (playerData != null)
        //{
        //    if (playerData.bonusAttackSpeedLevel >= 5)
        //    {
        //        Debug.Log("Max lv");
        //        return;
        //    }

        //    if (playerData.gold < GetCostAttackSpeedUpgrade())
        //    {
        //        Debug.Log($"Không đủ tiền {playerData.gold} target {GetCostAttackSpeedUpgrade()}");
        //        return;
        //    }

        //    playerData.gold -= GetCostAttackSpeedUpgrade();
        //    playerData.bonusAttackSpeedLevel++;
        //    playerData.bonusAttackSpeed = 0 + (playerData.bonusAttackSpeedLevel - 1) * 5;

        //    string json = JsonUtility.ToJson(playerData);
        //    PlayerPrefs.SetString(PlayerDataKey, json);

        //    Debug.Log($"Tile Level: {playerData.heartLevel}, Max Tiles: {playerData.bonusAttackSpeed}, Gold left: {playerData.gold}");

        if (playerData.bonusAttackSpeedLevel >= 5) return;

        if (playerData.gold < GetCostAttackSpeedUpgrade()) return;

        playerData.gold -= GetCostAttackSpeedUpgrade();
        playerData.bonusAttackSpeedLevel++;
        playerData.bonusAttackSpeed = 0 + (playerData.bonusAttackSpeedLevel - 1) * 5;

        // cập nhật UI 
        ShopManager.Instance.UpdateUpgradeCostAttackSpeedUI(playerData.bonusAttackSpeedLevel, playerData.bonusAttackSpeed);
        ShopManager.Instance.UpdateCurrentGoldText(playerData.gold);
        //}
    }
    #endregion

    private void LoadPlayerData()
    {
        if (DDOLoad.Instance.gameInit == true) return;

        DDOLoad.Instance.gameInit = true;

        Debug.Log("Loaded");

        if (PlayerPrefs.HasKey("IsFirstTime"))
        {
            PlayerData.Instance.LoadPlayerData();
            EnemyLibrary.Instance.LoadDictionary();
        }
        else
        {
            // Lần đầu chạy game -> Khởi tạo dữ liệu mặc định rồi lưu lại
            PlayerPrefs.SetInt("IsFirstTime", 1); // Đánh dấu đã khởi tạo dữ liệu
            PlayerPrefs.Save();

            PlayerData.Instance.BaseData();

            Debug.Log(PlayerData.Instance.playerData.gold);
            Debug.Log(PlayerData.Instance.playerData.maxTileBuilding);
            PlayerData.Instance.SavePlayerData();

            var enemyUnlock = EnemyLibrary.Instance;
            enemyUnlock.allEnemies = new Dictionary<string, int>();
            for (int i = 1; i <= 12; i++)
            {
                string key = $"TRP_{i:D3}"; // Định dạng số thành 3 chữ số (001, 002, ..., 015)
                enemyUnlock.allEnemies[key] = 0;
            }
            for (int i = 1; i <= 3; i++)
            {
                string key = $"BOSS_{i:D3}"; // Định dạng số thành 3 chữ số (001, 002, ..., 015)
                enemyUnlock.allEnemies[key] = 0;
            }

            enemyUnlock.SaveDictionary();
        }
    }
}

[System.Serializable]
public class PlayerData
{
    private static PlayerData instance;

    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerData();
            }
            return instance;
        }
    }

    public class Player
    {
        public int gold;
        public int tileLevel;
        public int maxTileBuilding;
        public int heartLevel;
        public int maxHeart;
        public int bonusAttackLevel;
        public float bonusAttack;
        public int bonusAttackSpeedLevel;
        public float bonusAttackSpeed;
    }

    public Player playerData;
    public readonly string PlayerDataKey = "PlayerData";

    public void SavePlayerData()
    {

        string json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(PlayerDataKey, json);
    }

    public void LoadPlayerData()
    {
        string json = PlayerPrefs.GetString(PlayerDataKey);
        playerData = JsonUtility.FromJson<Player>(json);
        Debug.Log(playerData == null);
    }

    public void BaseData() // Reset Data Default, Dont use 
    {
        playerData = new Player()
        {
            gold = 0,
            tileLevel = 1,
            maxTileBuilding = 6,
            heartLevel = 1,
            maxHeart = 3,
            bonusAttackLevel = 1,
            bonusAttack = 0,
            bonusAttackSpeedLevel = 1,
            bonusAttackSpeed = 0,
        };
    }
}
