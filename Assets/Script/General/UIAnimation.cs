using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnimation
{
    public static UIAnimation Instance { get 
        {
            instance ??= new();
            return instance;
        } }
    private static UIAnimation instance;


    public static void Slide(RectTransform rt, Vector2 start, Vector2 end)
    {
        rt.anchoredPosition = start;
        rt.DOAnchorPos(end, 1);
    }
}
