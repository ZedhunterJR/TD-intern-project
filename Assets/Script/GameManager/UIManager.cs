using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Win / Lose Panel")]
    [SerializeField] GameObject panelResult;
    [SerializeField] RectTransform panelResultRect;
    [SerializeField] Ease easeType = Ease.OutBack;
    [SerializeField] Button homeButton;
    [SerializeField] TextMeshProUGUI waveText, goldText;
    public Button HomeButton => homeButton;

    [Header("UI Resource")]
    [SerializeField] TextMeshProUGUI heartText;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] TextMeshProUGUI waveDetailText;

    [Header("Event")]
    [SerializeField] RectTransform eventPanelRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration;

    [SerializeField] RectTransform settingPopupRect;
    [SerializeField] CanvasGroup canvasDarkPanel;
    [SerializeField] float topPosYSetting, middlePosYSetting;



    private void Awake()
    {
        settingPopupRect = GameObject.FindGameObjectWithTag("PanelSetting").GetComponent<RectTransform>();
        Button clostButton = GameObject.FindGameObjectWithTag("ButtonClose").GetComponent<Button>();
        clostButton.onClick.RemoveAllListeners();
        clostButton.onClick.AddListener(PopupSettingOutro);
    }

    #region Setting Button
    public void PopupSettingIntro()
    {
        GameManager.Instance.PauseGame(1);
        canvasDarkPanel.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        canvasDarkPanel.alpha = 0f;

        canvasDarkPanel.DOFade(1, tweenDuration).SetUpdate(true);
        settingPopupRect.DOAnchorPosY(middlePosYSetting, tweenDuration).SetUpdate(true);
    }

    public void PopupSettingOutro()
    {
        Debug.Log("Outro Setting");
        Sequence outro = DOTween.Sequence();
        canvasDarkPanel.DOFade(0, tweenDuration).SetUpdate(true).OnComplete(() =>
        {
            canvasDarkPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(3000, 3000, 1000);
        });

        settingPopupRect.DOAnchorPosY(topPosYSetting, tweenDuration).SetUpdate(true);
        GameManager.Instance.PauseGame(2);
    }
    #endregion

    #region UI for Result
    public void ShowResultPanel()
    {
        panelResultRect.localScale = Vector3.zero;
        panelResultRect.position = Vector3.zero;
        panelResultRect.DOScale(Vector3.one, 1).SetEase(easeType).SetUpdate(true);

        UpdateResult();
    }

    public void UpdateResult()
    {
        waveText.text = $"Wave {WaveManager.Instance.currentWave}";
        goldText.text = $"{WaveManager.Instance.currentWave * 20 + (WaveManager.Instance.currentWave - 1) * 5}";
        GameManager.Instance.SaveRewards(WaveManager.Instance.currentWave * 20 + (WaveManager.Instance.currentWave - 1) * 5);
    }
    #endregion

    #region UI for Event
    public void ActiveEventPanel()
    {
        GameManager.Instance.ChangeStatus(GAME_STATUS.Pause);
        IGEventManager.Instance.ActivateCardPanel();
        EventPanelIntro();
        canvasDarkPanel.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        canvasDarkPanel.alpha = 0f;

        canvasDarkPanel.DOFade(1, tweenDuration).SetUpdate(true);
    }

    public void DeactiveEvenPanel()
    {
        EventPanelOutro();
        canvasDarkPanel.DOFade(0, tweenDuration).SetUpdate(true).OnComplete(() =>
        {
            canvasDarkPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(3000, 3000, 1000);
        });
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
                break;
            default:
                break;
        }
    }
    #endregion


}
