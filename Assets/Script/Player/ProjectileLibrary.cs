using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    public void ProjectileLob(GameObject projectile, GameObject target, bool rotation = true, float height = 5f)
    {
        var sc = projectile.GetComponent<ProjectileAdvanced>();
        var startPos = projectile.transform.position;
        sc.UpdateFunc += () =>
        {
            if (target != null)
            {
                sc.direction = target.transform.position;
            }
            var start = startPos;
            Vector3 end = sc.direction;
            var control = (start + end) / 2 + new Vector3(0, height);

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
}
