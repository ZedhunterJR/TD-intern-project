using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAnim : Singleton<SceneAnim>
{
    [SerializeField] RectTransform blackScreen;
    private bool isFirstTime = false;

    public void Awake()
    {
        SceneManager.activeSceneChanged += (a, b) =>
        {
            if(!isFirstTime)
            {
                isFirstTime = true;
                return;
            }

            Outro();
        };
    }

    public void Intro()
    {
        blackScreen.anchoredPosition = new Vector2(-3000, 0f);
        blackScreen.DOAnchorPos(Vector2.zero, 2f).SetUpdate(true).SetEase(Ease.OutExpo);
    }

    public void Outro()
    {
        blackScreen.anchoredPosition = new Vector2(0f, 0f);
        blackScreen.DOAnchorPos(new Vector2(3000f, 0), 2f).SetUpdate(true).SetEase(Ease.InExpo);
    }
}
