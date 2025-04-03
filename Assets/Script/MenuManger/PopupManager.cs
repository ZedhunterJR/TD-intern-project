using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] GameObject popupSetting, popupShop;
    [SerializeField] RectTransform settingPopupRect, shopPopupRect, libPopupRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float topPosYSetting, middlePosYSetting;
    [SerializeField] float tweenDuration;
    [SerializeField] CanvasGroup canvasDarkPanel;

    RectTransform tutorialPopupRect;

    public CanvasGroup CanvasDarkPanel => canvasDarkPanel;

    private void Awake()
    {
        Button clostButton = GameObject.FindGameObjectWithTag("ButtonClose").GetComponent<Button>();
        clostButton.onClick.RemoveAllListeners();
        clostButton.onClick.AddListener(DeactiveSetting);

        Button buttonCancel = GameObject.FindGameObjectWithTag("ButtonCancel").GetComponent<Button>();
        buttonCancel.onClick.RemoveAllListeners();
        buttonCancel.onClick.AddListener(QuitGame);
        popupSetting = GameObject.FindGameObjectWithTag("PanelSetting");
        settingPopupRect = GameObject.FindGameObjectWithTag("PanelSetting").GetComponent<RectTransform>();

        tutorialPopupRect = GameObject.FindGameObjectWithTag("PanelTutorial").GetComponent<RectTransform>();
        Button clostTutorial = GameObject.FindGameObjectWithTag("ButtonTutorialClose").GetComponent<Button>();
        clostTutorial.onClick.RemoveAllListeners();
        clostTutorial.onClick.AddListener(PanelTutorialOutro);

        Button guideTutorial = GameObject.FindGameObjectWithTag("ButtonGuide").GetComponent<Button>();
        guideTutorial.onClick.RemoveAllListeners();
        guideTutorial.onClick.AddListener(PanelTutorialIntro);
    }

    public void ActiveSetting()
    {
        PopupSettingIntro();
    }

    public void DeactiveSetting()
    {
        PopupSettingOutro();

    }

    public void ActiveShop()
    {
        PopupShopIntro();
    }

    public void DeactiveShop()
    {
        PopupShopOutro();
    }

    void PopupSettingIntro()
    {
        canvasDarkPanel.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        canvasDarkPanel.alpha = 0f;

        canvasDarkPanel.DOFade(1, tweenDuration).SetUpdate(true);
        settingPopupRect.DOAnchorPosY(middlePosYSetting, tweenDuration).SetUpdate(true);
    }

    void PopupSettingOutro()
    {
        Sequence outro = DOTween.Sequence();
        canvasDarkPanel.DOFade(0, tweenDuration).SetUpdate(true).OnComplete(() =>
        {
            canvasDarkPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(3000,3000,1000);
        });

        settingPopupRect.DOAnchorPosY(topPosYSetting, tweenDuration).SetUpdate(true);
        
    }

    void PopupShopIntro()
    {
        canvasDarkPanel.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        canvasDarkPanel.alpha = 0f;

        canvasDarkPanel.DOFade(1, tweenDuration).SetUpdate(true);
        shopPopupRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    void PopupShopOutro()
    {
        canvasDarkPanel.DOFade(0, tweenDuration).SetUpdate(true).OnComplete(() =>
        {
            canvasDarkPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(3000, 3000, 1000);
        });

        shopPopupRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true);
    }

    public void PopupLibIntro()
    {
        canvasDarkPanel.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        canvasDarkPanel.alpha = 0f;

        canvasDarkPanel.DOFade(1, tweenDuration).SetUpdate(true);
        libPopupRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    public void PopupLibOutro()
    {
        canvasDarkPanel.DOFade(0, tweenDuration).SetUpdate(true).OnComplete(() =>
        {
            canvasDarkPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(3000, 3000, 1000);
        });

        libPopupRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true);
    }

    public void PanelTutorialIntro()
    {
        TutorialManager.Instance.ShowPanelTutorial(TutorialManager.Instance.ResetIndex());
        tutorialPopupRect.DOAnchorPosY(0, 0.7f).SetUpdate(true);
    }

    public void PanelTutorialOutro()
    {
        tutorialPopupRect.DOAnchorPosY(2000, 0.7f).SetUpdate(true);
    }

    public void QuitGame()
    {
        MenuManager.Instance.QuitGame();
    }
}
