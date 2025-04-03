using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] RectTransform tutorialPanel;

    [SerializeField] List<GameObject> tutorialPanels;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [SerializeField] Button closeButton;

    private int currentPanelIndex = 0;

    private void Awake()
    {
        ShowPanelTutorial(currentPanelIndex);

        nextButton.onClick.AddListener(NextPanel);
        prevButton.onClick.AddListener(PrevPanel);
        closeButton.onClick.AddListener(CloseTutorial);
    }

    public void ShowPanelTutorial(int index)
    {
        for (int i = 0; i < tutorialPanels.Count; i++) tutorialPanels[i].SetActive(i == index);

        prevButton.gameObject.SetActive(index > 0);
        nextButton.gameObject.SetActive(index < tutorialPanels.Count - 1);
    }

    void NextPanel()
    {
        if (currentPanelIndex < tutorialPanels.Count - 1)
        {
            currentPanelIndex++;
            ShowPanelTutorial(currentPanelIndex);
        }
    }

    void PrevPanel()
    {
        if(currentPanelIndex > 0)
        {
            currentPanelIndex--;
            ShowPanelTutorial(currentPanelIndex);
        }
    }

    void CloseTutorial()
    {
        //PlayerPrefs.SetInt("IsFirstTime", 2); // Lưu trạng thái đã xem tutorial
    }

    public int ResetIndex()
    {
        currentPanelIndex = 0;
        return currentPanelIndex;
    }
}
