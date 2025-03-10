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
    }

    public PathEntity GetCurrentStandingPath(Vector2 pos)
    {
        //convert that pos into closet path position, not finished yet
        var pathPos = pos;

        return PathEntityDictionary[pathPos];
    }

    //when enemy first enter path
    public void ApplyPathEffect(GameObject enemy, PathType pathType)
    {

    }
    //when enemy leave path
    public void UndoPathEffect(GameObject enemy, PathType pathType)
    {

    }
}
