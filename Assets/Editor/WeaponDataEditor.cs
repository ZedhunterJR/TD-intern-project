using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //reference script
        var refScript = target as WeaponData;

        //appearance
        refScript.showApp = EditorGUILayout.Foldout(refScript.showApp, "Appearance");
        if (refScript.showApp)
        {
            refScript.rarity = EditorGUILayout.IntField("Rarity", refScript.rarity);
            refScript.weaponName = EditorGUILayout.TextField("Name", refScript.weaponName);
            refScript.color = EditorGUILayout.ColorField("Projectile Color", refScript.color);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Projectile Sprite");
            refScript.projectile = (Sprite)EditorGUILayout.ObjectField(refScript.projectile, typeof(Sprite), allowSceneObjects: false);
            EditorGUILayout.EndHorizontal();
            refScript.rotationValue = EditorGUILayout.FloatField(new GUIContent("Projectile rotation", "0 - no rotation, 1 - rotate at target, else - constant rotation"), refScript.rotationValue);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Tower Sprite");
            refScript.sprite = (Sprite)EditorGUILayout.ObjectField(refScript.sprite, typeof(Sprite), allowSceneObjects: false);
            EditorGUILayout.EndHorizontal();
            refScript.aniCon = (RuntimeAnimatorController)EditorGUILayout.ObjectField("Tower Animation", refScript.aniCon, typeof(RuntimeAnimatorController), allowSceneObjects: false);
        }
        //standard stat
        refScript.showStat = EditorGUILayout.Foldout(refScript.showStat, "Stat");
        if (refScript.showStat)
        {
            refScript.baseDmg = EditorGUILayout.FloatField("Damage", refScript.baseDmg);
            refScript.atkspd = EditorGUILayout.FloatField("Attack Speed", refScript.atkspd);
            refScript.pierce = EditorGUILayout.FloatField("Pierce", refScript.pierce);
            refScript.range = EditorGUILayout.FloatField("Range", refScript.range);
        }
        EditorGUILayout.Space();

        //weapon type
        refScript.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", refScript.weaponType);
        //straight
        if (refScript.weaponType == WeaponType.Straight)
        {
            refScript.speed = EditorGUILayout.FloatField("Projectile speed", refScript.speed);
            refScript.hitBoxSize = EditorGUILayout.FloatField("Hitbox size", refScript.hitBoxSize);
            refScript.lifeSpan = EditorGUILayout.FloatField("Life span", refScript.lifeSpan);
        }
        //seek
        if (refScript.weaponType == WeaponType.Seeking)
        {
            refScript.speed = EditorGUILayout.FloatField("Projectile speed", refScript.speed);
            refScript.hitBoxSize = EditorGUILayout.FloatField("Hitbox size", refScript.hitBoxSize);
            refScript.lifeSpan = EditorGUILayout.FloatField("Life span", refScript.lifeSpan);
            refScript.accuracy = EditorGUILayout.FloatField(new GUIContent("Seek Value", "0 - No seek, 1 - Instant turn to target"), refScript.accuracy);
        }
        //lobbed
        if (refScript.weaponType == WeaponType.Lobbed)
        {
            refScript.lifeSpan = EditorGUILayout.FloatField("Life span", refScript.lifeSpan);
            refScript.highPoint = EditorGUILayout.FloatField("Highest Point", refScript.lifeSpan);
            refScript.accuracy = EditorGUILayout.FloatField(new GUIContent("Lob accuracy", "0 - seek, 1 - moment of firing spot, else - around target pos"), refScript.accuracy);
        }
    }
}