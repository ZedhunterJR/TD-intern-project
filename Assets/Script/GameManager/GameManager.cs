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

    // Win/Lose Condition 
    [Header("Win / Lose Condition")]
    [SerializeField] float baseHealth = 3;
    private float currentHealth;
    [SerializeField] Image healthBar; 

    // Game Status
    private GAME_STATUS status;

    private void Awake()
    {
        currentHealth = baseHealth;
        status = GAME_STATUS.Init;

        if(waveManager != null)
            waveManager.OnAwake();
    }

    private void Start()
    {
        if (tileManager != null)
            tileManager.OnStart();
        if (enemyManager != null)
            enemyManager.OnStart();
        if (poolManager != null)
            poolManager.OnStart();

        ChangeStatus(GAME_STATUS.Playing);
    }

    private void Update()
    {
        if (status == GAME_STATUS.Playing)
        {
            if (tileManager != null)
                tileManager.OnUpdate();
            if (enemyManager != null)
                enemyManager.OnUpdate();
            if (poolManager != null)
                poolManager.OnUpdate();
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
                    break;
                case GAME_STATUS.Pause:
                    Debug.Log($"Change status done!! {status}");
                    break;
                case GAME_STATUS.Win:
                    Debug.Log($"Change status done!! {status}");
                    break;
                case GAME_STATUS.Lose:
                    Debug.Log($"Change status done!! {status}");
                    uiManager.ActivePanel(GAME_STATUS.Lose);
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
}

public enum GAME_STATUS
{
    Init,
    Playing,
    Pause,
    Win,
    Lose
}
