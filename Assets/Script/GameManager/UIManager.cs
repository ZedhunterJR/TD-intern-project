using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [Header ("Win / Lose Panel")]
    [SerializeField] GameObject panelResult;
    [SerializeField] RectTransform panelResultRect;
    [SerializeField] Ease easeType = Ease.OutBack;

    [Header ("UI Resource")]
    [SerializeField] TextMeshProUGUI heartText;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] TextMeshProUGUI waveDetailText;

    [Header("Event")]
    [SerializeField] RectTransform eventPanelRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration;

    #region UI for Result
    public void ShowResultPanel()
    {
        panelResultRect.localScale = Vector3.zero;
        panelResultRect.position = Vector3.zero;
        panelResultRect.DOScale(Vector3.one, tweenDuration).SetEase(easeType).SetUpdate(true);
    }
    #endregion

    #region UI for Event
    public void ActiveEventPanel()
    {
        GameManager.Instance.ChangeStatus(GAME_STATUS.Pause);
        EventPanelIntro();
    }

    public void DeactiveEvenPanel()
    {
        EventPanelOutro();
        GameManager.Instance.ChangeStatus(GAME_STATUS.Playing);
    }

    void EventPanelIntro()
    {
        eventPanelRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    void EventPanelOutro()
    {
        Sequence outro = DOTween.Sequence();

        outro.Append(eventPanelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true));
    }
    #endregion

    #region Update UI 
    public void UpdateHeartText(int currentHeath)
    {
        heartText.text = $"{currentHeath}";
    }

    public void UpdateCurrencyText(int currency)
    {
        currencyText.text = $"{currency}";
    }

    public void UpdateWaveDetailText(int currentWave)
    {
        waveDetailText.text = $"Wave {currentWave}";
    }
    #endregion

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
                panelResult.SetActive (true);
                ShowResultPanel();
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
                panelResult.SetActive(false);
                break;
            default:
                break;
        }
    }
    #endregion
}
