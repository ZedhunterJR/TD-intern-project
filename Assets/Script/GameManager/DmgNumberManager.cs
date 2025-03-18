using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DmgNumberManager : Singleton<DmgNumberManager>
{
    [SerializeField] private GameObject dmgNumPrefab;
    [SerializeField] private Transform dmgNumPoolTransform;

    private Queue<GameObject> dmgNumPool = new Queue<GameObject>();

    private void Awake()
    {
        CreateForPool(10);
    }

    private void CreateForPool(int num)
    {
        for (int i = 0; i  < num; i++)
        {
            var go = Instantiate(dmgNumPrefab, dmgNumPoolTransform);
            dmgNumPrefab.SetActive(false);
            dmgNumPool.Enqueue(go);
        }
    }
    public GameObject GetFromPool()
    {
        if (dmgNumPool.Count <= 0)
        {
            CreateForPool(5);
            return GetFromPool();
        }
        var go = dmgNumPool.Dequeue();
        return go;
    }
    public void ReturnToPool(GameObject go)
    {
        go.SetActive(false);
        dmgNumPool.Enqueue(go);
    }

    public void DmgNumber(Color color, float dmg, Vector2 pos)
    {
        var dNum = GetFromPool();
        dNum.transform.position = pos;
        dNum.SetActive(true);
        var txt = dNum.GetComponent<TextMeshPro>();
        txt.color = color;
        txt.text = dmg.DecimalFormat(0);

        dNum.transform.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();

        float randomDirection = UnityEngine.Random.Range(-0.3f, 0.3f); // Slightly left or right

        seq.Append(dNum.transform.DOScale(new Vector3(0.7f, 0.7f), 0.2f).SetEase(Ease.OutBack)) // Small impact scale-up
           .Join(dNum.transform.DOLocalMove(new Vector3(dNum.transform.localPosition.x + randomDirection,
                                                        dNum.transform.localPosition.y + 0.5f), 0.5f)
                  .SetEase(Ease.OutQuad)) // Float up slightly to one side
           .Insert(0.5f, txt.DOFade(0, 0.3f)) // Fade out near end
           .OnComplete(() => ReturnToPool(dNum)); // Return to pool after animation
    }

}
