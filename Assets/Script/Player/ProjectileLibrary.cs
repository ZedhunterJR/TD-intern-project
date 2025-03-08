using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectileLibrary
{
    private static ProjectileLibrary instance;
    public static ProjectileLibrary Instance
    {
        get
        {
            if (instance == null)
                instance = new();
            return instance;
        }
    }
    //lob at target enemy, if enemy die, use the last position
    public void ProjectileLob(GameObject projectile, GameObject target, bool rotation = true, float controlHeight = 5f, float lifeSpan = 1f, float controlRotation = 0)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        var startPos = projectile.transform.position;
        sc.lifeSpan = lifeSpan;
        sc.UpdateFunc = () =>
        {
            if (target != null || !target.activeSelf)
            {
                sc.direction = target.transform.position;
            }
            var start = startPos;
            Vector3 end = sc.direction;
            var control = CalculateControlPoint(start, end, controlHeight, controlRotation);

            float count = sc.LifeSpanInInterpolation;

            Vector3 m1 = Vector2.Lerp(start, control, count);
            Vector3 m2 = Vector2.Lerp(control, end, count);

            projectile.transform.position = Vector2.Lerp(m1, m2, count);

            if (rotation)
            {
                var dir = m2 - m1;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        };
    }
    //lob at target position
    public void ProjectileLob(GameObject projectile, Vector3 endPos, bool rotation = true, float controlHeight = 5f, float controlRotation = 0f, float lifeSpan = 1f)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        var startPos = projectile.transform.position;
        var controlSign = endPos.x - startPos.x >= 0f ? -1 : 1;
        var control = CalculateControlPoint(startPos, endPos, controlHeight, controlRotation);
        sc.lifeSpan = lifeSpan;
        sc.UpdateFunc = () =>
        {
            float count = sc.LifeSpanInInterpolation;

            Vector3 m1 = Vector2.Lerp(startPos, control, count);
            Vector3 m2 = Vector2.Lerp(control, endPos, count);

            projectile.transform.position = Vector2.Lerp(m1, m2, count);

            if (rotation)
            {
                var dir = m2 - m1;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        };
    }
    //straight and pierce
    public void ProjectileStraight(GameObject projectile, Vector2 dir, bool rotation = true, float lifeSpan = 1f)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        sc.lifeSpan = lifeSpan;
        dir.Normalize();
        sc.UpdateFunc = () =>
        {
            projectile.transform.position += sc.speed * Time.deltaTime * dir.ToVector3();

            if (rotation)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        };
    }
    //straight at target
    public void ProjectileStraightNoHitbox(GameObject projectile, GameObject target, bool rotation = true, float lifeSpan = 1f)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        var startPos = projectile.transform.position;
        sc.lifeSpan = lifeSpan;
        sc.UpdateFunc = () =>
        {
            if (target != null || !target.activeSelf)
            {
                sc.direction = target.transform.position;
            }

            float count = sc.LifeSpanInInterpolation;
            projectile.transform.position = Vector2.Lerp(startPos, sc.direction, count);

            if (rotation)
            {
                var dir = (Vector3)sc.direction - startPos;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        };
    }
    //straight at target position
    public void ProjectileStraightNoHitbox(GameObject projectile, Vector3 pos, bool rotation = true, float lifeSpan = 1f)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        var startPos = projectile.transform.position;
        sc.lifeSpan = lifeSpan;
        sc.UpdateFunc = () =>
        {
            float count = sc.LifeSpanInInterpolation;
            projectile.transform.position = Vector2.Lerp(startPos, pos, count);

            if (rotation)
            {
                var dir = pos - startPos;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        };
    }
    //sine wave at target
    public void ProjectileSineWaveNoHitbox(GameObject projectile, GameObject target, bool rotation = true, float amplitude = 0.5f, float frequency = 2f, float lifeSpan = 1f)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        var startPos = projectile.transform.position;
        sc.lifeSpan = lifeSpan;
        sc.UpdateFunc = () =>
        {
            if (target != null)
            {
                if (target != null || !target.activeSelf)
                {
                    sc.direction = target.transform.position;
                }
            }

            float count = sc.LifeSpanInInterpolation;
            Vector2 straightPath = Vector2.Lerp(startPos, sc.direction, count);

            // Calculate sine wave offset
            Vector2 dir = (Vector3)sc.direction - startPos;
            Vector2 perpendicularDir = new Vector2(-dir.y, dir.x).normalized; // Perpendicular to the main direction
            float sineOffset = Mathf.Sin(count * frequency * Mathf.PI * 2) * amplitude;

            // Apply the sine wave offset
            projectile.transform.position = straightPath + perpendicularDir * sineOffset;

            if (rotation)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        };
    }

    private Vector2 CalculateControlPoint(Vector2 start, Vector2 end, float controlHeight, float controlRotation)
    {
        Vector2 midPoint = (start + end) / 2;
        if (controlRotation == 0)
            return midPoint + new Vector2(0, controlHeight);

        Vector2 offset = new Vector2(0, controlHeight);
        // Rotate the offset around the midpoint by `controlRotation`
        Vector2 rotatedOffset = Quaternion.Euler(0, 0, controlRotation) * offset;
        // Final control point
        return midPoint + rotatedOffset;
    }
}
