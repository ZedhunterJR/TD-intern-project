using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] TileManager tileManager;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] PoolManager poolManager;

    // Win/Lose Condition 
    [Header("Win / Lose Condition")]
    [SerializeField] int baseHealth = 3;
    private int currentHealth;

    // Game Status
    private GAME_STATUS status;

    private void Awake()
    {
        currentHealth = baseHealth;
        status = GAME_STATUS.Init;
    }

    private void Start()
    {
        if (tileManager != null)
            tileManager.OnStart();
        if (enemyManager != null)
            enemyManager.OnStart();
        if (poolManager != null)
            poolManager.OnStart();
    }

    private void Update()
    {
        if (tileManager != null)
            tileManager.OnUpdate();
        if (enemyManager != null)
            enemyManager.OnUpdate();
        if (poolManager != null)
            poolManager.OnUpdate();
    }

    #region Condition Win Lose and Change Game Status
    public void TakeDame()
    {
        currentHealth -= 1;
        currentHealth = Mathf.Clamp(currentHealth, 0, baseHealth);
        Debug.Log(currentHealth);
        if (currentHealth == 0)
        {
            ChangeStatus(GAME_STATUS.Lose);
        }
    }

    public void ChangeStatus(GAME_STATUS newStatus)
    {
        if (status != newStatus)
        {
            status = newStatus;
            switch (status)
            {
                case GAME_STATUS.Init:
                    break;
                case GAME_STATUS.Playing:
                    break;
                case GAME_STATUS.Pause:
                    break;
                case GAME_STATUS.Win:
                    break;
                case GAME_STATUS.Lose:
                    Debug.Log($"Change status done!! {status}");
                    break;
                default:
                    break;
            }
        }
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
