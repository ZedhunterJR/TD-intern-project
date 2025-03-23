using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Range
{
    //when init, point this to another list like
    // AllEnemies = GameManager.AllEnemies
    // this will serve as reference point
    [HideInInspector] private List<GameObject> allEnemies = new();
    private Vector3 pos;

    public float detectionRange = 3f;
    //private float enemyUpdateTimer;
    public List<GameObject> AllEnemies => allEnemies;

    public Range(List<GameObject> allEnemies, Vector3 pos)
    {
        this.allEnemies = allEnemies;
        this.pos = pos;
    }
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

    public List<GameObject> EnemiesInRange()
    {

        List<GameObject> possibleEnemies = new();

        foreach (GameObject enemy in new List<GameObject>(allEnemies))
        {
            if (enemy == null || enemy.GetComponent<EnemyStat>().IsUntargetable) 
                continue; // Avoid null reference errors

            float sqrDistance = (enemy.transform.position - pos).sqrMagnitude;
            if (sqrDistance < detectionRange * detectionRange)
            {
                possibleEnemies.Add(enemy); // Add only if not already in the list
            }

        }
        return possibleEnemies;
    }

}