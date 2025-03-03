using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    //audio
    public Slider masterSlider, musicSlider, sfxSlider;

    //other stuff
    private bool isPaused = false;
    private RectTransform pause;

    [Header("Test only")]
    public AudioClip clipToPlay;
    [ContextMenu("Play Music")]
    public void PlayMusic()
    {
        AudioManager.Instance.PlayMusic(clipToPlay);
    }
    [ContextMenu("Play SFX")]
    public void PlaySFX()
    {
        AudioManager.Instance.PlaySFX(clipToPlay);
    }
    private void Start()
    {
        pause = GameObject.Find("pause").GetComponent<RectTransform>();
        /*
        UIAnimation.hoverOutline(pause.gameObject.transform.Find("restart").gameObject);
        pause.gameObject.transform.Find("restart").GetComponent<ButtonUI>().ClickFunc = () =>
        {
            SceneManager.LoadScene("Game");
            Time.timeScale = 1;
        };
        UIAnimation.hoverOutline(pause.gameObject.transform.Find("home").gameObject);
        pause.gameObject.transform.Find("home").GetComponent<ButtonUI>().ClickFunc = () =>
        {
            SceneManager.LoadScene("Title");
            Time.timeScale = 1;
        };
        */

        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            PauseGame();
        }

        //test only

    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            pause.anchoredPosition = Vector3.zero;
            Time.timeScale = 0;
            isPaused = true;
        } else
        {
            pause.anchoredPosition = new Vector2(9999, 0);
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}
