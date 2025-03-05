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
    public string enemyInitialSkin;
    public float hpBarPosY;

    [Header("Stats")]
    public float maxHp;
    public float baseMoveSpeed;
}
