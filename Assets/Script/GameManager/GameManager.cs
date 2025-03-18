using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] TileManager tileManager;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] TowerManager towerManager;
    [SerializeField] PoolManager poolManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] WaveManager waveManager;
    [SerializeField] PathManager pathManager;

    // Win/Lose Condition 
    [Header("Win / Lose Condition")]
    [SerializeField] float baseHealth = 3;
    private float currentHealth;
    [SerializeField] Image healthBar;

    // Game Status
    private GAME_STATUS status;

    private void Awake()
    {
        if (status == GAME_STATUS.Init)
        {
            currentHealth = baseHealth;
            status = GAME_STATUS.Init;
            Time.timeScale = 1;

            //if (pathManager != null)
            //    pathManager.OnAwake();
        }
    }

    private void Start()
    {
        ChangeStatus(GAME_STATUS.Playing);
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
        UpdateHealthBar();
        Debug.Log(currentHealth);
        if (currentHealth == 0)
        {
            ChangeStatus(GAME_STATUS.Lose);
        }
    }

    public void ChangeStatus(GAME_STATUS newStatus)
    {
        Debug.Log($"Current status: {status}");
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
        PlayerPrefs.Save();

        GameObject selectedMap = maps[randomIndex];

        // Instantiate map vào game
        Instantiate(selectedMap, Vector3.zero, Quaternion.identity);
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
