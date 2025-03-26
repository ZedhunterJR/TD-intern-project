using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : Singleton<PathManager>
{
    public Dictionary<Vector2, PathEntity> PathEntityDictionary = new();
    //private Dictionary<Vector2, PathEntity> SpecialPathEntityDictionary = new();

    [SerializeField] Sprite _spriteCaslte;
    [SerializeField] Sprite _spriteSpawner;

    private void Update()
    {
        PathEffectUpdate();
    }
    public void Init(List<GameObject> pathEntities)
    {
        PathEntityDictionary = new();
        foreach (var entity in pathEntities)
        {
            var objClass = entity.GetComponent<PathEntity>();
            objClass.Init();
            SetGraphic(PathType.None, objClass);
            PathEntityDictionary.Add(entity.transform.position, objClass);
        }
    }

    public PathType GetCurrentStandingPath(Vector2 pos)
    {
        try
        {
            return PathEntityDictionary[pos].currentPathType;
        }
        catch 
        {
            //Debug.LogError("somehow this happen?" + pos);
            return PathType.None;
        }
    }
    public PathEntity GetCurrentPathEntity(Vector2 pos)
    {
        try
        {
            return PathEntityDictionary[pos];
        }
        catch
        {
            return null;
        }
    }

    public void SetGraphic(PathType pathType, PathEntity entity)
    {
        entity.currentPathType = pathType;
        entity.spineAniCon.gameObject.SetActive(true);
        entity.defaultSprite.SetActive(false);
        switch (pathType)
        {
            case PathType.Lava:
                entity.spineAniCon.SetSkinName("lava");
                break;
            case PathType.Pond:
                entity.spineAniCon.SetSkinName("pond");
                break;
            case PathType.DirtyMist:
                entity.spineAniCon.SetSkinName("dirty mist");
                break;
            case PathType.CrystalField:
                entity.spineAniCon.SetSkinName("crystal field");
                break;
            case PathType.Swamp:
                entity.spineAniCon.SetSkinName("swamp");
                break;
            default:
                entity.spineAniCon.gameObject.SetActive(false);
                entity.defaultSprite.SetActive(true);
                break;

        }
    }

    public Vector2 GetNearestTileCenter(Vector2 position, float tileSize = 1)
    {
        float tileX = Mathf.Round(position.x / tileSize) * tileSize;
        float tileY = Mathf.Round(position.y / tileSize) * tileSize;
        return new Vector2(tileX, tileY);
    }

    private float lavaPathTimer = 0f;
    private float dirtPathTimer = 0f;
    private float pondPathTimer = 0f;
    public void PathEffectUpdate()
    {
        if (lavaPathTimer < 0.5f)
            lavaPathTimer += Time.deltaTime;
        else
        {
            foreach (var item in PathEntityDictionary)
            {
                if (item.Value.currentPathType != PathType.Lava)
                    continue;
                var enemies = EnemyManager.Instance.SamePathEnemies(item.Key);
                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<EnemyStat>().Burn();
                }
            }
            lavaPathTimer = 0f;
        }

        if (pondPathTimer < 1f)
            pondPathTimer += Time.deltaTime;
        else
        {
            foreach (var item in PathEntityDictionary)
            {
                if (item.Value.currentPathType != PathType.Pond)
                    continue;
                var enemies = EnemyManager.Instance.SamePathEnemies(item.Key);
                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<EnemyStat>().StackElement(2, Element.Water);
                }
            }
            pondPathTimer = 0f;
        }
        
        if (dirtPathTimer < 5f)
            dirtPathTimer += Time.deltaTime;
        else
        {
            foreach (var item in PathEntityDictionary)
            {
                if (item.Value.currentPathType != PathType.DirtyMist)
                    continue;
                var enemies = EnemyManager.Instance.SamePathEnemies(item.Key);
                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<EnemyStat>().StackElement(5, Element.Earth);
                }
            }
            dirtPathTimer = 0f;
        }
    }
}
