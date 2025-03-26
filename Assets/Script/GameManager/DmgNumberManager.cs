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
        var txt = dNum.GetComponentInChildren<TextMeshPro>();
        txt.color = color;
        txt.text = dmg.DecimalFormat(0);

        var baseSize = GetDamageNumberSize(dmg);
        dNum.transform.localScale = baseSize.Vec3();

        Sequence seq = DOTween.Sequence();

        float randomDirection = UnityEngine.Random.Range(-0.3f, 0.3f); // Slightly left or right

        seq.Append(dNum.transform.DOScale((baseSize * 0.7f).Vec3(), 0.2f).SetEase(Ease.OutBack)) // Small impact scale-up
           .Join(dNum.transform.DOLocalMove(new Vector3(dNum.transform.localPosition.x + randomDirection,
                                                        dNum.transform.localPosition.y + 0.5f), 0.5f)
                  .SetEase(Ease.OutQuad)) // Float up slightly to one side
           .Insert(0.5f, txt.DOFade(0, 0.3f)) // Fade out near end
           .OnComplete(() => ReturnToPool(dNum)); // Return to pool after animation
    }
    public float GetDamageNumberSize(float damage)
    {
        float[] damageThresholds = { 10f, 50f, 100f, 200f, 500f };
        float[] sizeThresholds = { 0.7f, 0.85f, 1f, 1.3f, 1.6f };

        // Clamp damage within min and max bounds
        if (damage <= damageThresholds[0]) return sizeThresholds[0];
        if (damage >= damageThresholds[damageThresholds.Length - 1]) return sizeThresholds[sizeThresholds.Length - 1];

        // Find the correct range
        for (int i = 0; i < damageThresholds.Length - 1; i++)
        {
            if (damage < damageThresholds[i + 1])
            {
                float t = (damage - damageThresholds[i]) / (damageThresholds[i + 1] - damageThresholds[i]);
                return Mathf.Lerp(sizeThresholds[i], sizeThresholds[i + 1], t);
            }
        }

        return 1f; // Fallback, should never reach here
    }
}
