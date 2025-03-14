using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    //when init, point this to another list like
    // AllEnemies = GameManager.AllEnemies
    // this will serve as reference point
    [HideInInspector] public List<GameObject> AllEnemies = new();

    public float detectionRange = 3f;
    //private float enemyUpdateTimer;
    public List<GameObject> FirstTargets()
    {
        var result = EnemiesInRange(); // Directly initialize with EnemiesInRange
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
        var result = EnemiesInRange();
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
    public GameObject StrongTarget()
    {
        var targets = StrongTargets();
        if (targets.Count > 0)
            return targets[0];
        return null;
    }

    List<GameObject> EnemiesInRange()
    {

        List<GameObject> possibleEnemies = new();

        foreach (GameObject enemy in new List<GameObject>(AllEnemies))
        {
            if (enemy == null || enemy.GetComponent<EnemyStat>().isUntargetable) 
                continue; // Avoid null reference errors

            float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance < detectionRange * detectionRange)
            {
                possibleEnemies.Add(enemy); // Add only if not already in the list
            }

        }
        return possibleEnemies;
    }

}