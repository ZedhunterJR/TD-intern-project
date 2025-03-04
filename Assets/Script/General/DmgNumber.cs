using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DmgNumber : MonoBehaviour
{
    private float lifeSpan = 1f;
    private TMP_Text dmgText;
    private RectTransform rectTransform;
    public bool sprite;
    // Start is called before the first frame update
    void Start()
    {
        dmgText = GetComponent<TextMeshPro>();
        rectTransform = GetComponent<RectTransform>();

        dmgText.DOFade(0, lifeSpan).OnComplete(Destruct);

        float ran = Random.Range(-0.2f, 0.2f);
        rectTransform.DOMove(new Vector2(transform.position.x + ran, transform.position.y + 0.5f), lifeSpan/2f);

        if (sprite)
        {
            GetComponentInChildren<SpriteRenderer>().DOFade(0, lifeSpan);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Destruct()
    {
        transform.DOKill();
        Destroy(gameObject);
    }
}
