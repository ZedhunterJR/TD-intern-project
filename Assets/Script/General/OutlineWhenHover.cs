using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineWhenHover : MonoBehaviour
{
    SpriteRenderer ren; 

    private void Start()
    {
        ren = transform.Find("sprite").GetComponent<SpriteRenderer>();
        ren.material.color = new Color32(0, 0, 0, 0);
    }

    private void OnMouseEnter()
    {
        ren.material.color = new Color32(255, 255, 255, 255);
    }
    private void OnMouseExit()
    {
        ren.material.color = new Color32(0, 0, 0, 0);
    }
}
