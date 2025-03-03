using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    //temp
    public List<GameObject> AllEnemies = new();

    public List<WaypointPath> waypoints = new();

    public int num;

    [ContextMenu("Get Coordinate")]
    private void GetCoor()
    {
        List<Vector2> list = new List<Vector2>();
        foreach (Transform t in transform)
        {
            if (t != transform)
            list.Add(t.position);
        }
        if (num >= waypoints.Count)
        {
            waypoints.Add(new(list));
        }
        else
        {
            waypoints[num] = new(list);
        }
    }

    [ContextMenu("Test Spawn")]
    private void TestSpawn()
    {
        var e = Resources.Load<GameObject>("Prefab/enemy_template");
        var ins = Instantiate(e);
        AllEnemies.Add(ins);
    }
}

[System.Serializable]
public class WaypointPath
{
    public List<Vector2> points = new();
    public WaypointPath(List<Vector2> points)
    {
        this.points = new(points);
    }
}
