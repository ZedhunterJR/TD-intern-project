using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class GameManager : Singleton<GameManager>
{
    [SerializeField] TileManager tileManager;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] TowerManager towerManager;
    [SerializeField] PoolManager poolManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] WaveManager waveManager;
    [SerializeField] PathManager pathManager;
    [SerializeField] IGEventManager eventManager;

    // Win/Lose Condition 
    [Header("Win / Lose Condition")]
    [SerializeField] int baseHealth = 3;
    private int currentHealth;
    [SerializeField] Image healthBar;

    // currency
    private int currencyGold;

    // Game Status
    private GAME_STATUS status;

    // Data
    PlayerData playerData = new PlayerData();
    public PlayerData PlayerData => playerData;

    private Button quitGame;
    private void Awake()
    {
        quitGame = GameObject.FindGameObjectWithTag("ButtonCancel").GetComponent<Button>();
        quitGame.onClick.RemoveAllListeners();
        quitGame.onClick.AddListener(() => 
        {
            uiManager.PopupSettingOutro();
            this.Invoke(() =>
            {

                SceneManager.LoadScene("MenuScene");
            }, 2f, true);
        });
        uiManager.HomeButton.onClick.RemoveAllListeners();
        uiManager.HomeButton.onClick.AddListener(() =>
        {
            this.Invoke(() =>
            {

                SceneManager.LoadScene("MenuScene");
            }, 2f, true);
        });

        LoadDataFromPlayerprefs();
        //print("??");
        if (status == GAME_STATUS.Init)
        {
            currentHealth = playerData.maxHeart;
            currencyGold = 100;
            uiManager.UpdateCurrencyText(currencyGold);
            uiManager.UpdateHeartText(currentHealth);
            status = GAME_STATUS.Init;
            Time.timeScale = 1;

            //if (pathManager != null)
            //    pathManager.OnAwake();
        }

    }

    private void Start()
    {
        ChangeStatus(GAME_STATUS.Playing);
        LoadRandomMap();
    }

    private void Update()
    {
        if (status == GAME_STATUS.Playing)
        {
        }
    }

    #region Condition Win Lose and Change Game Status
    public void TakeDame()
    {
        currentHealth -= 1;
        currentHealth = Mathf.Clamp(currentHealth, 0, baseHealth);
        //UpdateHealthBar();
        //Debug.Log(currentHealth);
        uiManager.UpdateHeartText(currentHealth);
        if (currentHealth == 0)
        {
            ChangeStatus(GAME_STATUS.Lose);
        }
    }

    public void ChangeStatus(GAME_STATUS newStatus)
    {
        //Debug.Log($"Current status: {status}");
        if (status != newStatus)
        {
            status = newStatus;
            switch (status)
            {
                case GAME_STATUS.Init:
                    Debug.Log($"Change status done!! {status}");
                    break;
                case GAME_STATUS.Playing:
                    Debug.Log($"Change status done!! {status}");
                    PauseGame(2);
                    break;
                case GAME_STATUS.Pause:
                    Debug.Log($"Change status done!! {status}");
                    PauseGame(1);
                    break;
                case GAME_STATUS.Win:
                    Debug.Log($"Change status done!! {status}");
                    PauseGame(1);
                    break;
                case GAME_STATUS.Lose:
                    Debug.Log($"Change status done!! {status}");
                    uiManager.ActivePanel(GAME_STATUS.Lose);
                    PauseGame(1);
                    break;
                default:
                    break;
            }
        }

    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / baseHealth;
    }
    #endregion

    private bool gameIsPause = false;
    /// <summary>
    /// 0 = invert, 1 = pause, 2 = unpause
    /// </summary>
    /// <param name="pauseStatus"></param>
    public void PauseGame(int pauseStatus)
    {
        switch (pauseStatus)
        {
            case 0: gameIsPause = !gameIsPause; break;
            case 1: gameIsPause = true; break;
            case 2: gameIsPause = false; break;
        }
        if (gameIsPause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    #region Load Random Map
    void LoadRandomMap()
    {
        GameObject[] maps = Resources.LoadAll<GameObject>("Map");

        if (maps.Length == 0)
        {
            Debug.Log("Không tìm thấy map nào");
            return;
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, maps.Length);
        }
        while (randomIndex == PlayerPrefs.GetInt("LastMapIndex", -1)); // Kiểm tra map có trùng lần trước không

        // Lưu lại map đã chơi để tránh trùng
        PlayerPrefs.SetInt("LastMapIndex", randomIndex);
        //PlayerPrefs.Save();

        int lastMapIndex = PlayerPrefs.GetInt("LastMapIndex", -1);
        Debug.Log("LastMapIndex hiện tại: " + lastMapIndex);

        GameObject selectedMap = maps[randomIndex];

        // Instantiate map vào game
        Instantiate(selectedMap, Vector3.zero, Quaternion.identity);
    }
    #endregion

    #region Save Rewards After Finishing Game
    public void SaveRewards(int rewards)
    {
        playerData.gold += rewards;
        
        string json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("PlayerData", json);
    }
    #endregion

    #region Load Data From Player Prefs 
    void LoadDataFromPlayerprefs()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            string json = PlayerPrefs.GetString("PlayerData");
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
    }
    #endregion

    #region currency
    public bool ModifyGold(int value) // -10
    {
        if (-value > currencyGold)
        {
            return false;
        }

        currencyGold += value;
        uiManager.UpdateCurrencyText(currencyGold);
        return true;
    }

    #endregion

}

public enum GAME_STATUS
{
    Init,
    Playing,
    Pause,
    Win,
    Lose
}



