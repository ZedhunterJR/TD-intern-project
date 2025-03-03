using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMove : MonoBehaviour
{
    private List<Vector2> waypoints = new List<Vector2>();
    public int waypointNum;
    private float ranMod;
    private Vector2 currentWaypointPos;
    public int waypointIndex = 0;

    public float moveSpeed;

    public bool isSummoned = false;

    public int hpDmg = 1;
    
    public float DistanceToGoal()
    {
         float distance = 0;
         if (waypointIndex < waypoints.Count)
         {
               distance += Vector2.Distance(gameObject.transform.position, currentWaypointPos);
         }
         for (int i = waypointIndex; i < waypoints.Count - 1; i++)
         {
               Vector3 startPosition = waypoints [i];
               Vector3 endPosition = waypoints [i + 1];
               distance += Vector2.Distance(startPosition, endPosition);
         }
    return distance;
    }

    private void Move()
    {
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
                if (waypointIndex < waypoints.Count) {
                    //Debug.Log(waypoints[waypointIndex].transform.position.x - waypoints[waypointIndex + 1].transform.position.x);
                    if (waypoints[waypointIndex-1].x - waypoints[waypointIndex].x > 0)
                    {
                        transform.Find("sprite").GetComponent<SpriteRenderer>().flipX = true;
                    }
                    else
                    {
                        transform.Find("sprite").GetComponent<SpriteRenderer>().flipX = false;
                    }
                }

            }
        }
		else
		{
            //EventManager.Instance.modiHp(hpDmg);
			Destroy(gameObject);
		}
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Waypoints obj = GameObject.FindGameObjectWithTag("waypoint").GetComponent<Waypoints>();
        switch (waypointNum)
        {
            case 0:
                waypoints.AddRange(obj.wp0); break;
            case 1:
                waypoints.AddRange(obj.wp1); break;
            case 2:
                waypoints.AddRange(obj.wp2); break;
            case 3:
                waypoints.AddRange(obj.wp3); break;
        }
        
        //temporary
        ranMod = Random.Range(0.4f, -0.4f);
        //ranMod = 0f;

        Vector2 tempStartPos = waypoints[waypointIndex];
        tempStartPos.y = waypoints[waypointIndex].y + ranMod;
        tempStartPos.x = waypoints[waypointIndex].x + ranMod;

        if (isSummoned == false) {
        transform.position = tempStartPos;
        }
    }

    // Update is called once per frame
    void Update()
    {        
        moveSpeed = GetComponent<EnemyStat>().moveSpeed;
        if (waypointIndex < waypoints.Count)
        {
             currentWaypointPos.x = waypoints[waypointIndex].x + ranMod;
             currentWaypointPos.y = waypoints[waypointIndex].y + ranMod;
        }
        if (!GetComponent<EnemyStat>().isStunned) {
            Move();
            //GetComponentInChildren<SimpleAnim>().isStunned = false;
        }
        //else {GetComponentInChildren<SimpleAnim>().isStunned = true;}
    
    }
}
