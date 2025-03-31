using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] Button button;
    [SerializeField] ShopManager shopManager;
    private bool isSceneChange = false;

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
        if (isSceneChange) { return; }
        isSceneChange = true;

        this.Invoke(() => SceneManager.LoadScene("FinalPlayingScene"), 2f, true);
        SceneAnim.Instance.Intro();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
