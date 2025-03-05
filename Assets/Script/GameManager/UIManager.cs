using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header ("Win / Lose Panel")]
    [SerializeField] GameObject panelLose;

    #region Win / Lose Panel
    public void ActivePanel(GAME_STATUS status = GAME_STATUS.Playing)
    {
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
                panelLose.SetActive (true);
                break;
            default:
                break;
        }
    }

    public void InactivePanel(GAME_STATUS status = GAME_STATUS.Playing)
    {
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
                panelLose.SetActive(false);
                break;
            default:
                break;
        }
    }
    #endregion
}
