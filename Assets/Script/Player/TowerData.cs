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
    public Element element;
    public float projSpwPosY;

    [Header("Stats")]
    public float[] baseDamage = new float[3];
    [Tooltip("Attacks per second")] public float[] baseAtkSpd = new float[3]; //attack per sec
    [Tooltip("Radius in world unit")] public float[] range = new float[3]; //range in point radius
    [Tooltip("Status effect stack")] public int[] statusEffectStack = new int[3];

    public string attackScriptName; //derived from TowerAttack.cs
    public string specialAbility;
}
