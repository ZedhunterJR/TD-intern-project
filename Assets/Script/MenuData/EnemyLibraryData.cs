using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLibraryData : ScriptableObject
{
    public string enemyName; 
    public int enemyHealth;
    public float enemyMoveSpeed;
    public Element element;
    public Sprite sprite;
    public string description;
}