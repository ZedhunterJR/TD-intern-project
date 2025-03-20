using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] GameObject popupSetting;
    [SerializeField] RectTransform settingPopupRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration; 
    [SerializeField] CanvasGroup canvasGroup; 
    

    public void ActiveSetting()
    {
        popupSetting.SetActive(true);
        PopupIntro();
    }

    public async void DeactiveSetting()
    {
        await PopupOutro();
        popupSetting.SetActive(false);
    }

    void PopupIntro()
    {
        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        settingPopupRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    async Task PopupOutro()
    {
        canvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        await settingPopupRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    public void QuitGame()
    {
        MenuManager.Instance.QuitGame();
    }
}
