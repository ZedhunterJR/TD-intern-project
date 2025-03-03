using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum WeaponType
{
    Straight,
    Seeking,
    Lobbed
}

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public bool showApp = true;
    public int rarity;
    public string weaponName;
    public Color color = Color.white;
    public Sprite sprite;
    public RuntimeAnimatorController aniCon;
    public Sprite projectile;
    public float rotationValue;

    public bool showStat = true;
    public float baseDmg;
    public float atkspd;
    public float pierce = 1;
    public float range;

    //on hold
    public bool showValue = true;

    public WeaponType weaponType;

    //straight - seek
    public float speed;
    public float hitBoxSize;
    public float lifeSpan;
    //seek 0 (no seek) - 1 (instant turn to target when in range)
    public float accuracy;

    //lobbed - use upper lifespan
    public float highPoint;
    //0: seek, 1: at last pos, other: around last pos - use upper

}

