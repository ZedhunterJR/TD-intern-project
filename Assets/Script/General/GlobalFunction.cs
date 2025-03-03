using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GlobalFunction : MonoBehaviour
{
    public static GlobalFunction Instance { get; private set; }

    private Transform screenClutter;

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void Awake()
    {
        Instance = this;

    }
    void Update()
    {
        
    }

    
    public GameObject GetClosestTarget(Vector2 spot, float minDist, List<GameObject> blackList)
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("enemy");
        GameObject re = null;
        foreach (GameObject enemy in all)
        {
            if (blackList.Contains(enemy))
                continue;
            float dist = Vector2.Distance(spot, enemy.transform.position);
            if (dist < minDist)
            {
                re = enemy;
                minDist = dist;
            }
        }
        return re;
    }

    
}


