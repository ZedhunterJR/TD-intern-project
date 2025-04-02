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
    [SerializeField] RectTransform heartEffect;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] RectTransform currencyEffect;
    [SerializeField] TextMeshProUGUI waveDetailText;
    [SerializeField] TextMeshProUGUI spawnCostText;
    [SerializeField] Image waveCircleTimer;

    [Header("Event")]
    [SerializeField] RectTransform eventPanelRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration;

    [SerializeField] RectTransform settingPopupRect;
    [SerializeField] CanvasGroup canvasDarkPanel;
    [SerializeField] float topPosYSetting, middlePosYSetting;

    [Header("Tower Pool")]
    [SerializeField] List<Image> towerImages;
    [SerializeField] Image coverPanel;

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

    public void HpUpdateEffect(int value)
    {
        if (value < 0)
        {
            var img = heartEffect.GetComponent<TextMeshProUGUI>();
            img.text = "-" + value;
            heartEffect.anchoredPosition = new Vector3(10f, 0f);
            img.color = new Color32(255, 0, 0, 255);

            heartEffect.DOAnchorPosY(-15f, 2f).SetEase(Ease.OutExpo).SetUpdate(true);
            img.DOFade(0f, 2f).SetEase(Ease.OutExpo).SetUpdate(true);
        }
        if (value > 0)
        {
            var img = heartEffect.GetComponent<TextMeshProUGUI>();
            heartEffect.anchoredPosition = new Vector3(10f, -15f);
            img.color = new Color32(0, 255, 0, 255);

            heartEffect.DOAnchorPosY(0f, 2f).SetEase(Ease.InExpo).SetUpdate(true);
            img.DOFade(0f, 2f).SetEase(Ease.InExpo).SetUpdate(true);
        }
    }

    public void UpdateCurrencyText(int currency)
    {
        currencyText.text = $"{currency}";
    }
    public void UpdateCurrencyEffect(int value)
    {
        if (value < 0)
        {
            var img = currencyEffect.GetComponent<TextMeshProUGUI>();
            img.text = "-" + value;
            currencyEffect.anchoredPosition = new Vector3(27f, 0f);
            img.color = new Color32(255, 0, 0, 255);

            currencyEffect.DOAnchorPosY(-15f, 2f).SetEase(Ease.OutExpo).SetUpdate(true);
            img.DOFade(0f, 2f).SetEase(Ease.OutExpo).SetUpdate(true);
        }
        if (value > 0)
        {
            var img = currencyEffect.GetComponent<TextMeshProUGUI>();
            currencyEffect.anchoredPosition = new Vector3(27f, -15f);
            img.color = new Color32(0, 255, 0, 255);

            currencyEffect.DOAnchorPosY(0f, 2f).SetEase(Ease.InExpo).SetUpdate(true);
            img.DOFade(0f, 2f).SetEase(Ease.InExpo).SetUpdate(true);
        }
    }
    public void UpdateWaveDetailText(int currentWave)
    {
        waveDetailText.text = $"Wave {currentWave}";
    }

    public void UpdateSpawnCostText(int cost)
    {
        spawnCostText.text = $"- {cost}";
    }

    private float updateTimerInterval = 0f;
    public void UpdateWaveCircle(float fillAmount)
    {
        if (updateTimerInterval > 1 / 30f)
        {
            waveCircleTimer.fillAmount = fillAmount;
            updateTimerInterval = 0f;
        }
        else
            updateTimerInterval += Time.deltaTime;
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

    #region Tower Pool
    public void UpdateTowerPoolUI(List<TowerData> towerData)
    {
        Sequence seq = DOTween.Sequence();
        coverPanel.color = coverPanel.color.SetAlpha(0f);
        seq.Append(coverPanel.DOFade(1f, 1f)).SetUpdate(true)
            .Append(coverPanel.DOFade(0f, 1f)).SetUpdate(true)
            .InsertCallback(1f, () =>
            {
                foreach (var item in towerData)
                {
                    if (item.element == Element.Water)
                        towerImages[0].sprite = item.towerSprite;
                    if (item.element == Element.Fire)
                        towerImages[1].sprite = item.towerSprite;
                    if (item.element == Element.Earth)
                        towerImages[2].sprite = item.towerSprite;
                }
            }).SetUpdate(true);
    }
    #endregion
}
