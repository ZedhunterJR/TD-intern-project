using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] GameObject popupSetting, popupShop;
    [SerializeField] RectTransform settingPopupRect, shopPopupRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration; 
    [SerializeField] CanvasGroup canvasGroup, canvasGroupShop; 
    

    public void ActiveSetting()
    {
        popupSetting.SetActive(true);
        PopupSettingIntro();
    }

    public async void DeactiveSetting()
    {
        await PopupSettingOutro();
        popupSetting.SetActive(false);
    }

    public void ActiveShop()
    {
        popupShop.SetActive(true);
        PopupShopIntro();
    }

    public async void DeactiveShop()
    {
        await PopupShopOutro();
        popupShop.SetActive(false);
    }

    void PopupSettingIntro()
    {
        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        settingPopupRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    async Task PopupSettingOutro()
    {
        canvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        await settingPopupRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    void PopupShopIntro()
    {
        canvasGroupShop.DOFade(1, tweenDuration).SetUpdate(true);
        shopPopupRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    async Task PopupShopOutro()
    {
        canvasGroupShop.DOFade(0, tweenDuration).SetUpdate(true);
        await shopPopupRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    public void QuitGame()
    {
        MenuManager.Instance.QuitGame();
    }
}
