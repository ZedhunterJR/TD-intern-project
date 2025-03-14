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
        Vector2 tempStartPos = waypoints[waypointIndex];
        tempStartPos.y = waypoints[waypointIndex].y + ranMod;
        tempStartPos.x = waypoints[waypointIndex].x + ranMod;

        //if summoned on the track, set the currentIndex param
        if (currentIndex == 0)
        {
            transform.position = tempStartPos;
        }
    }

    private void Move(float moveSpeed)
    {
        //print(waypointIndex); print(waypoints.Count);
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops
        if (waypointIndex < waypoints.Count)
        {

            // Move Enemy from current waypoint to the next one
            // using MoveTowards method
            transform.position = Vector2.MoveTowards(transform.position,
               currentWaypointPos,
               moveSpeed * Time.deltaTime);

            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if ((Vector2)transform.position == currentWaypointPos)
            {
                waypointIndex += 1;
                if (waypointIndex < waypoints.Count)
                {
                    //Debug.Log(waypoints[waypointIndex].transform.position.x - waypoints[waypointIndex + 1].transform.position.x);
                    if (waypoints[waypointIndex - 1].x - waypoints[waypointIndex].x > 0)
                        FlipX = true;
                    else
                        FlipX = false;
                }
            }
        }
        else
        {
            OnEnemyExit?.Invoke();
            //Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void MoveUpdate(float moveSpeed)
    {
        //moveSpeed = GetComponent<EnemyStat>().moveSpeed;
        if (waypointIndex < waypoints.Count)
        {
            currentWaypointPos.x = waypoints[waypointIndex].x + ranMod;
            currentWaypointPos.y = waypoints[waypointIndex].y + ranMod;
        }
        Move(moveSpeed);

    }
}
