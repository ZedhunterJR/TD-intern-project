using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List<WaypointPath> waypoints = new();
    [SerializeField] List<Vector2Int> points = new();
    [SerializeField] GameObject pathPrefab;
    [SerializeField] List<GameObject> pathList = new();

    public int num;

    public void Start()
    {
        foreach (var p in waypoints)
        {
            points = ConvertToIntPoints(p.points);
        }

        GeneratePath();
    }

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

    public void GeneratePath()
    {
        //print(points[0]);
        pathList = new();
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2Int start = points[i];
            Vector2Int end = points[(i + 1)];

            if (start.x == end.x) // Đi theo trục Y
            {
                int minY = Mathf.Min(start.y, end.y);
                int maxY = Mathf.Max(start.y, end.y);

                for (int y = minY + 1; y < maxY; y++)
                {
                    GameObject path = Instantiate(pathPrefab, new Vector3(start.x, y, 0), Quaternion.identity);
                    pathList.Add(path);
                }
            }
            else if (start.y == end.y) // Đi theo trục X
            {
                int minX = Mathf.Min(start.x, end.x);
                int maxX = Mathf.Max(start.x, end.x);

                for (int x = minX + 1; x < maxX; x++)
                {
                    GameObject path = Instantiate(pathPrefab, new Vector3(x, start.y, 0), Quaternion.identity);
                    pathList.Add(path);
                }
            }
        }

        foreach (var item in points)
        {
            GameObject path = Instantiate(pathPrefab, new Vector3(item.x, item.y), Quaternion.identity);
            pathList.Add(path);
        }

        PathManager.Instance.Init(pathList);
    }

    List<Vector2Int> ConvertToIntPoints(List<Vector2> vector2List)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        foreach (Vector2 v in vector2List)
        {
            result.Add(Vector2Int.RoundToInt(v));
        }
        return result;
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
