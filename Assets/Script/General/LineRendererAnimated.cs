using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererAnimated : MonoBehaviour
{
    public List<Sprite> sprites;
    public List<Vector3> positions = new();
    public float interval = 0.1f;
    private float timer;
    private int index = 0;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            index++;
            if (index >= sprites.Count)
                index = 0;
            lineRenderer.material.SetTexture("_MainTex", sprites[index].texture);
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());

            timer = 0;
        }
    }

    public void Init(List<Sprite> ss, List<Vector3> pos, float size, float interval)
    {
        sprites = ss;
        positions = pos;
        lineRenderer.startWidth = size;
        lineRenderer.endWidth = size;
        this.interval = interval;
    }
    public void UpdatePos(List<Vector3> pos)
    {
        positions = pos;
    }
}
