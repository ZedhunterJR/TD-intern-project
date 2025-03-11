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

    //when enemy first enter path
    public void ApplyPathEffect(GameObject enemy, PathType pathType)
    {
        switch (pathType)
        {
            case PathType.None:
                break;
            case PathType.Lava:
                print("entering lava");
                break;
            case PathType.Pond:
                break;
            case PathType.DirtyMist:
                break;
            case PathType.CrystalField:
                break;
            case PathType.Swamp:
                break;
            default:
                break;
        }
    }
    //when enemy leave path
    public void UndoPathEffect(GameObject enemy, PathType pathType)
    {
        switch (pathType)
        {
            case PathType.None:
                break;
            case PathType.Lava:
                print("leaving lava");
                break;
            case PathType.Pond:
                break;
            case PathType.DirtyMist:
                break;
            case PathType.CrystalField:
                break;
            case PathType.Swamp:
                break;
            default:
                break;
        }
    }
}
