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
    [SerializeField] TextMeshProUGUI spawnCostText;
    [SerializeField] Image waveCircleTimer;
    [SerializeField] TextMeshProUGUI currentBuildingText;

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

    [Header("DUmb effect")]
    [SerializeField] private GameObject effectText;
    private Queue<GameObject> effectQueue = new();

    private void Awake()
    {
        settingPopupRect = GameObject.FindGameObjectWithTag("PanelSetting").GetComponent<RectTransform>();
        Button clostButton = GameObject.FindGameObjectWithTag("ButtonClose").GetComponent<Button>();
        clostButton.onClick.RemoveAllListeners();
        clostButton.onClick.AddListener(PopupSettingOutro);

        for (int i = 0; i < 4; i++)
        {
            var instance = Instantiate(effectText, heartText.transform.parent);
            effectQueue.Enqueue(instance);
        }

        this.UpdateCurrentBuildingText();
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
        var effect = GetEffectTextFromPool();
        var txt = effect.GetComponent<TextMeshProUGUI>();
        var rt = effect.GetComponent<RectTransform>();
        effect.SetActive(true);
        effect.transform.SetParent(heartText.transform.parent);
        if (value < 0)
        {
            txt.text = "" + value;
            rt.anchoredPosition = new Vector3(10f, 0f);
            txt.color = new Color32(255, 0, 0, 255);

            rt.DOAnchorPosY(-15f, 2f).SetEase(Ease.OutExpo).SetUpdate(true);
            txt.DOFade(0f, 2f).SetEase(Ease.OutExpo).SetUpdate(true).
                OnComplete(() => ReturnEffectTextToPool(effect));
        }
        if (value > 0)
        {
            txt.text = "+" + value;
            rt.anchoredPosition = new Vector3(10f, -15f);
            txt.color = new Color32(0, 255, 0, 255);

            rt.DOAnchorPosY(0f, 2f).SetEase(Ease.InExpo).SetUpdate(true);
            txt.DOFade(0f, 2f).SetEase(Ease.InExpo).SetUpdate(true).
                OnComplete(() => ReturnEffectTextToPool(effect));
        }
    }

    public void UpdateCurrencyText(int currency)
    {
        currencyText.text = $"{currency}";
    }
    public void UpdateCurrencyEffect(int value)
    {
        var effect = GetEffectTextFromPool();
        var txt = effect.GetComponent<TextMeshProUGUI>();
        var rt = effect.GetComponent<RectTransform>();
        effect.SetActive(true);
        effect.transform.SetParent(currencyText.transform.parent);
        if (value < 0)
        {
            txt.text = "" + value;
            rt.anchoredPosition = new Vector3(27f, 0f);
            txt.color = new Color32(255, 0, 0, 255);

            rt.DOAnchorPosY(-15f, 2f).SetEase(Ease.OutExpo).SetUpdate(true);
            txt.DOFade(0f, 2f).SetEase(Ease.OutExpo).SetUpdate(true).
                OnComplete(() => ReturnEffectTextToPool(effect));
        }
        if (value > 0)
        {
            txt.text = "+" + value;
            rt.anchoredPosition = new Vector3(27f, -15f);
            txt.color = new Color32(0, 255, 0, 255);

            rt.DOAnchorPosY(0f, 2f).SetEase(Ease.InExpo).SetUpdate(true);
            txt.DOFade(0f, 2f).SetEase(Ease.InExpo).SetUpdate(true).
                OnComplete(() => ReturnEffectTextToPool(effect));
        }
    }
    private GameObject GetEffectTextFromPool()
    {
        if (effectQueue.Count <= 0)
        {
            var instance = Instantiate(effectText, heartText.transform.parent);
            effectQueue.Enqueue(instance);
            return GetEffectTextFromPool();
        }
        return effectQueue.Dequeue();
    }
    private void ReturnEffectTextToPool(GameObject effect)
    {
        effect.SetActive(false);
        effectQueue.Enqueue(effect);
    }

    public void NoCurrencyEffect()
    {
        Vector3 originalPos = currencyText.rectTransform.localPosition;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(currencyText.DOColor(Color.red, 0.2f))
            .Append(currencyText.DOColor(new Color(129f / 255f, 117f / 255f, 89f / 255f), 1f))
            .Join(currencyText.rectTransform.DOShakePosition(1f, 5f, 10, 90, false, true));

        sequence.Play();
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

    public void UpdateCurrentBuildingText()
    {
        currentBuildingText.text = $"Built Towers {TowerManager.Instance.towers.Count}/{GameManager.Instance.playerData.maxTileBuilding}";
    }

    public void NoBuildingEffect()
    {
        Vector3 originalPos = currentBuildingText.rectTransform.localPosition;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(currentBuildingText.DOColor(Color.red, 0.2f))
            .Append(currentBuildingText.DOColor(Color.white, 1f))
            .Join(currentBuildingText.rectTransform.DOShakePosition(1f, 5f, 10, 90, false, true));

        sequence.Play();
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
