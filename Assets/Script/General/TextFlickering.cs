using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextFlickering : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Flickering(true);
    }

    void Flickering(bool flick)
    {
        if (flick)
            GetComponent<TextMeshProUGUI>().DOColor(new Color32(255, 255, 255, 0), 1f)
                .OnComplete(() => Flickering(false));
        else
            GetComponent<TextMeshProUGUI>().DOColor(Color.white, 1f)
                .OnComplete(() => Flickering(true));
    }
}
