using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] Button button;
    [SerializeField] ShopManager shopManager;

    private void Awake()
    {
        if (shopManager != null) shopManager.OnAwake();
    }
    private void Start()
    {
        button.onClick.AddListener(LoadPlayScene);
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene("FinalPlayingScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
