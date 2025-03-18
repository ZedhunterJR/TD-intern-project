using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    [Header("Graphic")]
    public string enemyName;
    public SkeletonDataAsset enemyType;
    public string skinName;
    public float hpBarPosY;

    [Header("Stats")]
    public int tier;
    public float maxHp;
    public float baseMoveSpeed;
    public Element element;

    [Header("Behavior")]
    public string ability;
}

public enum Element
{
    Fire,
    Water,
    Earth
}
