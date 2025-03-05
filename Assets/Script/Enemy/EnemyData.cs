using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public Sprite sprite;
    public string enemyName;
    public float maxHp;
    public float baseMoveSpeed;
}
