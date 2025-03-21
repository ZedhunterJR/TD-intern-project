using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerFire : TowerAttack
{
    protected override void OnAwake()
    {
        if (!isPlaced)
        {
            var res = Resources.Load<GameObject>("Prefab/projectile_object");
            var proj = Instantiate(res);
            proj.transform.Find("spine_animation").transform.localScale = new Vector3(0.3f, 0.3f);
            PoolManager.Instance.RegisterProjectilePool(proj, "Fire_thrower_bullet", 5, "tower_fire_proj");
            isPlaced = true;
            return;
        }
        if (stat.level == 2)
        {
            var sur = GetSurroundingPositions(transform.position);
            List<PathEntity> paths = new List<PathEntity>();
            foreach (var item in sur)
            {
                var pIns = PathManager.Instance.GetCurrentPathEntity(item);
                if (pIns != null)
                    paths.Add(pIns);
            }
            StartCoroutine(BurnThePath(paths));
        }
    }
    protected override void Attack(GameObject target)
    {
        base.Attack(target);
        //might need pooling for projectile
        var instance = PoolManager.Instance.GetProjectileFromPool("tower_fire_proj");
        instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
        TowerBehaviorLibrary.Instance.ProjectileLob(instance, target);
        instance.SetActive(true);
        var projSc = instance.GetComponent<ProjectileAdvanced>();
        projSc.PreDestruct = () =>
        {
            DealDmg(projSc.currentTarget, projSc.transform.position);
            PoolManager.Instance.ReturnProjectileToPool(instance, "tower_fire_proj");
        };
    }
    private IEnumerator BurnThePath(List<PathEntity> paths)
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < paths.Count; i++)
        {
            int index = i;
            var instance = PoolManager.Instance.GetProjectileFromPool("tower_fire_proj");
            instance.transform.position = transform.position + new Vector3(0, stat.data.projSpwPosY);
            TowerBehaviorLibrary.Instance.ProjectileLob(instance, paths[index].gameObject.transform.position);
            instance.SetActive(true);
            var projSc = instance.GetComponent<ProjectileAdvanced>();
            projSc.PreDestruct = () =>
            {
                PathManager.Instance.SetGraphic(PathType.Lava, paths[index]);
                PoolManager.Instance.ReturnProjectileToPool(instance, "tower_fire_proj");
            };

            yield return new WaitForSeconds(1f); // Wait for 0.5s between each trigger
        }
    }
    private List<Vector2> GetSurroundingPositions(Vector2 position)
    {
        List<Vector2> surroundingPositions = new List<Vector2>
        {
            position + new Vector2Int(-1,  1), // Top-left
            position + new Vector2Int( 0,  1), // Top
            position + new Vector2Int( 1,  1), // Top-right
            position + new Vector2Int(-1,  0), // Left
            position + new Vector2Int( 1,  0), // Right
            position + new Vector2Int(-1, -1), // Bottom-left
            position + new Vector2Int( 0, -1), // Bottom
            position + new Vector2Int( 1, -1)  // Bottom-right
        };

        return surroundingPositions;
    }
}
