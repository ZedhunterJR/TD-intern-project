using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerData : ScriptableObject
{
    public Sprite sprite;
    public string towerName;
    public float baseDamage;
    public float baseAtkSpd; //attack per sec
    public float range; //range in point radius

    public string attackScriptName; //derived from TowerAttack.cs
}
