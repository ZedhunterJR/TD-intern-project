using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : Singleton<PathManager>
{
    public Dictionary<Vector2, PathEntity> PathEntityDictionary = new();

    public void Init(List<GameObject> pathEntities)
    {
        PathEntityDictionary = new();
        foreach (var entity in pathEntities)
        {
            var objClass = entity.GetComponent<PathEntity>();
            PathEntityDictionary.Add(entity.transform.position, objClass);
        }

        //temp, remove later
        var ranPath = PathEntityDictionary.GetRandomValue();
        ranPath.InflictLandMaking(PathType.Lava);
    }

    public PathType GetCurrentStandingPath(Vector2 pos)
    {
        try
        {
            return PathEntityDictionary[pos].CurrentPathType;
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

    //when enemy first enter path
    public void ApplyPathEffect(GameObject enemy, PathType pathType)
    {
        switch (pathType)
        {
            case PathType.Lava:
                enemy.GetComponent<EnemyStat>().InflictVisibleStatusEffect(VisibleStatusEffect.Heated);
                break;
            case PathType.Pond:
                enemy.GetComponent<EnemyStat>().InflictVisibleStatusEffect(VisibleStatusEffect.Wet);
                break;
            case PathType.DirtyMist:
                enemy.GetComponent<EnemyStat>().InflictVisibleStatusEffect(VisibleStatusEffect.Dirted);
                break;
            case PathType.CrystalField:
                enemy.GetComponent<EnemyStat>().InflictVisibleStatusEffect(VisibleStatusEffect.Crystalized);
                break;
            case PathType.Swamp:
                enemy.GetComponent<EnemyStat>().InflictVisibleStatusEffect(VisibleStatusEffect.Glutinous);
                break;
            default:
                break;
        }
    }
    //when enemy leave path
    public void UndoPathEffect(GameObject enemy, PathType pathType)
    {
        //yeah turn out this is currently not needed
        //still keep it here if future demand
    }
}
