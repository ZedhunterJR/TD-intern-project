using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEntity : MonoBehaviour
{
    //ref to invoke at runtime
    public SpineAnimationController spineAniCon;
    public GameObject defaultSprite;

    public PathType currentPathType = PathType.None;
    public void Init()
    {
        currentPathType = PathType.None;
        spineAniCon = GetComponentInChildren<SpineAnimationController>();
        defaultSprite = transform.Find("default_sprite").gameObject;
        //SetGraphic(currentPathType);
    }
   
    /*
    public void InflictLandMaking(PathType pathType)
    {
        //condition for None type
        if (currentPathType == PathType.None)
        {
            SetGraphic(pathType);
            return;
        }

        //condition for combination
        var pathSet = new HashSet<PathType> { currentPathType, pathType };

        if (pathSet.SetEquals(new HashSet<PathType> { PathType.Lava, PathType.DirtyMist }))
            SetGraphic(PathType.CrystalField);
        if (pathSet.SetEquals(new HashSet<PathType> { PathType.Pond, PathType.DirtyMist }))
            SetGraphic(PathType.Swamp);
        if (pathSet.SetEquals(new HashSet<PathType> { PathType.Lava, PathType.Pond }))
            SetGraphic(PathType.None);

    }
    */
}

public enum PathType
{
    None,
    Lava,
    Pond,
    DirtyMist,
    CrystalField,
    Swamp,
}
