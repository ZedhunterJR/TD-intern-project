using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEntity : MonoBehaviour
{
    //ref to invoke at runtime
    private SpineAnimationController spineAniCon;
    private GameObject defaultSprite;

    private PathType currentPathType = PathType.None;
    public PathType CurrentPathType => currentPathType;
    private void Awake()
    {
        spineAniCon = GetComponentInChildren<SpineAnimationController>();
        defaultSprite = transform.Find("default_sprite").gameObject;
        SetGraphic(PathType.None);
    }
    private void SetGraphic(PathType pathType)
    {
        currentPathType = pathType;
        spineAniCon.gameObject.SetActive(true);
        defaultSprite.SetActive(false);
        switch (currentPathType)
        {
            case PathType.Lava:
                spineAniCon.SetSkinName("lava");
                break;
            case PathType.Pond:
                spineAniCon.SetSkinName("pond");
                break;
            case PathType.DirtyMist:
                spineAniCon.SetSkinName("dirty mist");
                break;
            case PathType.CrystalField:
                spineAniCon.SetSkinName("crystal field");
                break;
            case PathType.Swamp:
                spineAniCon.SetSkinName("swamp");
                break;
            default:
                spineAniCon.gameObject.SetActive(false);
                defaultSprite.SetActive(true);
                break;

        }
    }
    public void InflictLandMaking(PathType pathType)
    {
        //condition for combination
        var pathSet = new HashSet<PathType> { currentPathType, pathType };

        if (pathSet.SetEquals(new HashSet<PathType> { PathType.Lava, PathType.DirtyMist }))
            SetGraphic(PathType.CrystalField);
        if (pathSet.SetEquals(new HashSet<PathType> { PathType.Pond, PathType.DirtyMist }))
            SetGraphic(PathType.Swamp);
        if (pathSet.SetEquals(new HashSet<PathType> { PathType.Lava, PathType.Pond }))
            SetGraphic(PathType.None);

        //condition for None type
        if (currentPathType == PathType.None)
            SetGraphic(pathType);

    }
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
