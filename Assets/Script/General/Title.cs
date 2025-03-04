using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Title : MonoBehaviour
{
    private void OnMouseUp()
    {
        SceneManager.LoadScene("Game");
    }
    private void Start()
    {
        //MoveBackAndForth(true);
    }
    void MoveBackAndForth(bool forth)
    {
        if (forth)
            gameObject.transform.DOMoveX(-5, 20f).OnComplete(() => MoveBackAndForth(false));
        else
            gameObject.transform.DOMoveX(5, 20f).OnComplete(() => MoveBackAndForth(true));
    }
}
