using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Range : MonoBehaviour
{
    //when init, point this to another list like
    // AllEnemies = GameManager.AllEnemies
    // this will serve as reference point
    [HideInInspector] public List<GameObject> AllEnemies = new();

    [SerializeField] private List<GameObject> EnemiesInRange = new();

    public float detectionRange = 3f;
    //private float enemyUpdateTimer;
    // Start is called before the first frame update
    void Start()
    {
        EnemiesInRange = new List<GameObject>();
        //temp all enemies
        AllEnemies = EnemyManager.Instance.AllEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemiesInRange();
    }
    public List<GameObject> FirstTargets()
    {
        var result = new List<GameObject>(EnemiesInRange); // Directly initialize with EnemiesInRange
        result.Sort(
            delegate (GameObject e1, GameObject e2)
            {
                // Null checks to push null values to the back
                if (e1 == null && e2 == null) return 0;  // Both null, equal ranking
                if (e1 == null) return 1;  // e1 is null, push to back
                if (e2 == null) return -1; // e2 is null, push to back

                // Compare based on DistanceToGoal
                return e1.GetComponent<WaveMove>().DistanceToGoal()
                    .CompareTo(e2.GetComponent<WaveMove>().DistanceToGoal());
            }
        );
        return result;
    }

    public List<GameObject> StrongTargets()
    {
        var result = new List<GameObject>();
        result.AddRange(EnemiesInRange);
        result.Sort(
            delegate (GameObject e1, GameObject e2)
            {
                // Null checks to push null values to the back
                if (e1 == null && e2 == null) return 0;  // Both null, equal ranking
                if (e1 == null) return 1;  // e1 is null, push to back
                if (e2 == null) return -1; // e2 is null, push to back

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

    public GameObject FirstTarget()
    {
        var targets = FirstTargets();
        if (targets.Count > 0)
            return targets[0];
        return null;
    }
    public GameObject LastTarget()
    {
        var targets = LastTargets();
        if (targets.Count > 0)
            return targets[0];
        return null;
    }

    void UpdateEnemiesInRange()
    {
        // Using a HashSet for quick lookup
        HashSet<GameObject> currentEnemies = new HashSet<GameObject>(EnemiesInRange);

        foreach (GameObject enemy in AllEnemies)
        {
            if (enemy == null) continue; // Avoid null reference errors

            float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance < detectionRange * detectionRange)
            {
                if (!currentEnemies.Contains(enemy))
                {
                    EnemiesInRange.Add(enemy); // Add only if not already in the list
                }
            }
            else
            {
                if (currentEnemies.Contains(enemy))
                {
                    EnemiesInRange.Remove(enemy); // Remove enemies that moved out of range
                }
            }
        }
    }
}
