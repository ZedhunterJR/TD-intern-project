using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[CreateAssetMenu]
public class TowerData : ScriptableObject
{
    [Header("Graphic")]
    public string towerName;
    public SkeletonDataAsset towerType;
    public string towerInitialSkin; 

    [Header("Stats")]
    public float baseDamage;
    [Tooltip("Attacks per second")] public float baseAtkSpd; //attack per sec
    [Tooltip("Radius in world unit")] public float range; //range in point radius

    public string attackScriptName; //derived from TowerAttack.cs
}
