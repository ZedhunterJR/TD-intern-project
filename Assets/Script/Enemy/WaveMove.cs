using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveMove : MonoBehaviour
{
    private Action OnEnemyExit = null;
    [SerializeField] private List<Vector2> waypoints = new List<Vector2>();
    private float ranMod = 0;
    private Vector2 currentWaypointPos;
    private int waypointIndex = 0;

    [HideInInspector] public bool isSummoned = false;
    [HideInInspector] public bool FlipX { get; private set; }

    private Vector2 currentPushBackWaypointPos;
    private float pushBackTimer;
    private float pushBackSpd;
    private bool isPushingBack = false;
    public float DistanceToGoal()
    {
        float distance = 0;
        if (waypointIndex < waypoints.Count)
        {
            distance += Vector2.Distance(gameObject.transform.position, currentWaypointPos);
        }
        for (int i = waypointIndex; i < waypoints.Count - 1; i++)
        {
            Vector3 startPosition = waypoints[i];
            Vector3 endPosition = waypoints[i + 1];
            distance += Vector2.Distance(startPosition, endPosition);
        }
        return distance;
    }

    public void Init(List<Vector2> wps, Action onExit, int currentIndex = 0)
    {
        OnEnemyExit = onExit;
        waypoints = new(wps);
        waypointIndex = currentIndex;

        //random offset on map, if needed
        ranMod = Random.Range(-.2f, .2f);
        for (int i = 0; i < waypoints.Count; i++)
        {
            waypoints[i] += new Vector2(ranMod, ranMod);
        }
        Vector2 tempStartPos = waypoints[waypointIndex];
        currentWaypointPos = tempStartPos;

        //if summoned on the track, set the currentIndex param
        if (currentIndex == 0)
        {
            transform.position = tempStartPos;
        }
    }

    private void Move(float moveSpeed)
    {
        if (waypoints.Count == 0) return; // Prevent errors if waypoints are missing

        if (moveSpeed >= 0) // Normal forward movement
        {
            if (waypointIndex < waypoints.Count)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    currentWaypointPos,
                    moveSpeed * Time.deltaTime);

                if ((Vector2)transform.position == currentWaypointPos)
                {
                    waypointIndex++;
                    if (waypointIndex < waypoints.Count)
                    {
                        FlipX = waypoints[waypointIndex - 1].x > waypoints[waypointIndex].x;
                        currentWaypointPos = waypoints[waypointIndex]; // Update current waypoint position
                        currentPushBackWaypointPos = waypoints[waypointIndex - 1];
                    }
                }
            }
            else
            {
                OnEnemyExit?.Invoke();
            }
        }
        else // Move backward if speed is negative
        {
            if (waypointIndex > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    currentPushBackWaypointPos,
                    -moveSpeed * Time.deltaTime); // Reverse direction

                if ((Vector2)transform.position == currentPushBackWaypointPos && waypointIndex > 1)
                {
                    waypointIndex--;
                    //FlipX = waypoints[waypointIndex].x > waypoints[Mathf.Max(waypointIndex - 1, 0)].x;
                    currentWaypointPos = waypoints[waypointIndex]; //Fix: Update current waypoint position
                    currentPushBackWaypointPos = waypoints[waypointIndex - 1];
                    
                }
            }
        }
    }



    // Update is called once per frame
    public void MoveUpdate(float moveSpeed)
    {
        if (pushBackTimer <= 0)
        {
            Move(moveSpeed);
            if (isPushingBack)
            {
                FlipX = waypoints[waypointIndex - 1].x > waypoints[waypointIndex].x;
                isPushingBack = false;
            }
        }
        else
        {
            pushBackTimer -= Time.deltaTime;
            Move(-pushBackSpd);
        }

    }

    public void ApplyPushback(float distance, float pushBackTimer)
    {
        this.pushBackTimer = pushBackTimer;
        pushBackSpd = distance / pushBackTimer;
        isPushingBack = true;
    }
}
