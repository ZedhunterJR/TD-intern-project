using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : Singleton<DataManager>
{
    private const string PlayerDataKey = "PlayerData";
    [SerializeField] private PlayerData playerData = new PlayerData();
    public PlayerData PlayerData => playerData;

    private void Awake()
    {
        LoadPlayerData();
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
        if (PlayerData != null)
        {
            playerData.gold += gold;
            string json = JsonUtility.ToJson(playerData);
            PlayerPrefs.SetString(PlayerDataKey, json);
        }
        else
            Debug.Log("Đang bị thiết Player Data");
    }

    public void UpgradeTileLevel()
    {
        if (playerData != null)
        {
            if (playerData.tileLevel >= 5)
            {
                Debug.Log("Level Max");
                return;
            }

            if (playerData.gold < GetCostTileUpgrade())
            {
                Debug.Log($"Không đủ tiền {playerData.gold} target {GetCostTileUpgrade()}");
                return;
            }

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

            string json = JsonUtility.ToJson(playerData);
            PlayerPrefs.SetString(PlayerDataKey, json);

            Debug.Log($"Tile Level: {playerData.tileLevel}, Max Tiles: {playerData.maxTileBuilding}, Gold left: {playerData.gold}");

            ShopManager.Instance.UpdateUpgradeCostTileUI();
        }
    }

    public void UpgradeHeartLevel()
    {
        if (playerData != null)
        {
            if (playerData.heartLevel >= 3)
            {
                Debug.Log("Max lv");
                return;
            }

            if (playerData.gold < GetCostHeartUpgrade())
            {
                Debug.Log($"Không đủ tiền {playerData.gold} target {GetCostTileUpgrade()}");
                return;
            }

            playerData.gold -= GetCostHeartUpgrade();
            playerData.heartLevel++;
            playerData.maxHeart = 3 + (playerData.heartLevel - 1);

            string json = JsonUtility.ToJson(playerData);
            PlayerPrefs.SetString(PlayerDataKey, json);

            Debug.Log($"Tile Level: {playerData.heartLevel}, Max Tiles: {playerData.maxHeart}, Gold left: {playerData.gold}");

            // cập nhật UI 
            ShopManager.Instance.UpdateUpgradeCostHeartUI();
        }
    }

    public void UpgradeAttackLevel()
    {
        if (playerData != null)
        {
            if (playerData.bonusAttackLevel >= 5)
            {
                Debug.Log("Max lv");
                return;
            }

            if (playerData.gold < GetCostAttackUpgrade())
            {
                Debug.Log($"Không đủ tiền {playerData.gold} target {GetCostAttackUpgrade()}");
                return;
            }

            playerData.gold -= GetCostAttackUpgrade();
            playerData.bonusAttackLevel++;
            playerData.bonusAttack = 0 + (playerData.bonusAttackLevel - 1) * 5;

            string json = JsonUtility.ToJson(playerData);
            PlayerPrefs.SetString(PlayerDataKey, json);

            Debug.Log($"Tile Level: {playerData.heartLevel}, Max Tiles: {playerData.bonusAttack}, Gold left: {playerData.gold}");

            // cập nhật UI 
            ShopManager.Instance.UpdateUpgradeCostAttackUI();
        }
    }

    public void UpgradeAttackSpeedLevel()
    {
        if (playerData != null)
        {
            if (playerData.bonusAttackSpeedLevel >= 5)
            {
                Debug.Log("Max lv");
                return;
            }

            if (playerData.gold < GetCostAttackSpeedUpgrade())
            {
                Debug.Log($"Không đủ tiền {playerData.gold} target {GetCostAttackSpeedUpgrade()}");
                return;
            }

            playerData.gold -= GetCostAttackSpeedUpgrade();
            playerData.bonusAttackSpeedLevel++;
            playerData.bonusAttackSpeed = 0 + (playerData.bonusAttackSpeedLevel - 1) * 5;

            string json = JsonUtility.ToJson(playerData);
            PlayerPrefs.SetString(PlayerDataKey, json);

            Debug.Log($"Tile Level: {playerData.heartLevel}, Max Tiles: {playerData.bonusAttackSpeed}, Gold left: {playerData.gold}");

            // cập nhật UI 
            ShopManager.Instance.UpdateUpgradeCostAttackSpeedUI();
        }
    }
    #endregion

    #region Change Data Function To Testing
    [ContextMenu("Increase Gold")]
    public void IncreaseGold()
    {
        if (PlayerData != null)
        {
            playerData.gold += 10000;
            string json = JsonUtility.ToJson(playerData);
            PlayerPrefs.SetString(PlayerDataKey, json);
        }
        else
            Debug.Log("Đang bị thiết Player Data");
    }
    #endregion

    [ContextMenu("Base data")]
    public void BaseData() // Reset Data Default, Dont use 
    {
        playerData = new PlayerData()
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

        string json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(PlayerDataKey, json);
        PlayerPrefs.Save();
    }

    private void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("IsFirstTime"))
        {
            // Đã có dữ liệu trước đó, load data từ PlayerPrefs
            if (PlayerPrefs.HasKey(PlayerDataKey))
            {
                string json = PlayerPrefs.GetString(PlayerDataKey);
                playerData = JsonUtility.FromJson<PlayerData>(json);
            }
            EnemyLibrary.Instance.LoadDictionary();
            foreach (var e in EnemyLibrary.Instance.allEnemies)
            {

                Debug.Log($"Key: {e.Key} == Value: {e.Value}");
            }
        }
        else
        {
            // Lần đầu chạy game -> Khởi tạo dữ liệu mặc định rồi lưu lại
            BaseData();
            PlayerPrefs.SetInt("IsFirstTime", 1); // Đánh dấu đã khởi tạo dữ liệu
            PlayerPrefs.Save();

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
        EnemyLibraryManager.Instance.Init();
    }
}

[System.Serializable]
public class PlayerData
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
