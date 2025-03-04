using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Range : MonoBehaviour
{
    //private SpriteRenderer sprite;
    public List<GameObject> enemiesInRange;

    //public GameObject strongTarget;
    //public GameObject lastTarget;

    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new List<GameObject>();
        //sprite = transform.parent.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i].gameObject == null)
            {
                enemiesInRange.RemoveAt(i);
            }
        }
        
        /*if (firstTarget != null)
        {
            if (firstTarget.transform.position.x - transform.position.x > 0)
                sprite.flipX = false;
            else 
                sprite.flipX = true;
        }*/
    }
    public List<GameObject> FirstTargets()
    {
        var result = new List<GameObject>();
        result.AddRange(enemiesInRange);
        result.Sort(
            delegate (GameObject e1, GameObject e2)
            {
                return e1.GetComponent<WaveMove>().DistanceToGoal().CompareTo
                    (e2.GetComponent<WaveMove>().DistanceToGoal());
            }
        );
        return result;
    }
    public List<GameObject> StrongTargets()
    {
        var result = new List<GameObject>();
        result.AddRange(enemiesInRange);
        result.Sort(
            delegate (GameObject e1, GameObject e2)
            {
                return e1.GetComponent<EnemyStat>().currentHp.CompareTo
                    (e2.GetComponent<EnemyStat>().currentHp);
            }
        );
        return result;
    }
    public List<GameObject> LastTargets()
    {
        var result = FirstTargets();
        result.Reverse();
        return result;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        Transform parent = other.gameObject.transform;
        if (parent != null)
        if (parent.tag == "enemy")
        {
            enemiesInRange.Add(parent.gameObject);
        }
        
    }
    void OnTriggerExit2D (Collider2D other)
    {
        Transform parent = other.gameObject.transform;
        if (parent != null)
        if (parent.tag == "enemy")
        {
            enemiesInRange.Remove(parent.gameObject);
        }

    }


    
}
