using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List<Vector2> wp0 = new List<Vector2>();
    public List<Vector2> wp1 = new List<Vector2>();
    public List<Vector2> wp2 = new List<Vector2>();
    public List<Vector2> wp3 = new List<Vector2>();

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
        switch (num)
        {
            case 0:
                wp0.Clear();
                wp0.AddRange(list); break;
            case 1:
                wp1.Clear();
                wp1.AddRange(list); break;
            case 2:
                wp2.Clear();
                wp2.AddRange(list); break;
        }
    }
}
