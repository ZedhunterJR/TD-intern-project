using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add enemy ability here, probably two type
/// one time when init, and lingering (update) ability
/// </summary>
public class EnemyAbilityLibrary
{
    private static EnemyAbilityLibrary instance;
    public static EnemyAbilityLibrary Instance
    {
        get
        {
            if (instance == null)
                instance = new EnemyAbilityLibrary();
            return instance;
        }
    }

    public void GetAbility(EnemyStat enemy, string abilityId)
    {
        switch (abilityId)
        {
            case "ABI_001": //wet body
                {
                    
                }
                break;
            case "ABI_002": //water absorb
                {
                    enemy.PreMitiDmgFunc = (d, s) =>
                    {
                        if (s.element == Element.Water)
                            return 0;
                        return d;
                    };
                }
                break;
            case "ABI_003": //healing water
                {
                    
                }
                break;
            case "ABI_004": //fire body
                {

                }
                break;
            case "ABI_005": //fire absorb
                {
                    enemy.PreMitiDmgFunc = (d, s) =>
                    {
                        if (s.element == Element.Fire)
                            return 0;
                        return d;
                    };
                }
                break;
            case "ABI_006": //fire boost
                {

                }
                break;
            case "ABI_010": //sand body
                {

                }
                break;
            case "ABI_011": //dirt vacumm
                {
                    enemy.PreMitiDmgFunc = (d, s) =>
                    {
                        if (s.element == Element.Earth)
                            return 0;
                        return d;
                    };
                }
                break;
            case "ABI_012": //sand cover
                {

                }
                break;
            case "ABI_016": //hp up 1
                {
                    enemy.maxHealth += 1;
                    enemy.currentHp = enemy.maxHealth;
                    enemy.UpdateHp(0);
                }
                break;
            case "ABI_017": //hp up 2
                {
                    enemy.maxHealth += 2;
                    enemy.currentHp = enemy.maxHealth;
                    enemy.UpdateHp(0);
                }
                break;
            case "ABI_018": //hp up 3
                {
                    enemy.maxHealth += 3;
                    enemy.currentHp = enemy.maxHealth;
                    enemy.UpdateHp(0);
                }
                break;
            case "ABI_019": //spd 1
                {
                    enemy.maxSpeed += 1;
                }
                break;
            case "ABI_020": //spd 2
                {
                    enemy.maxSpeed += 2;
                }
                break;
            case "ABI_021": //spd 3
                {
                    enemy.maxSpeed += 3;
                }
                break;
            case "ABI_022": //pond create
                {

                }
                break;
            case "ABI_023": //lava create
                {

                }
                break;
            case "ABI_025": //sand create
                {

                }
                break;
            case "ABI_027": //water diving
                {

                }
                break;
            case "ABI_028": //fire dash
                {

                }
                break;
            case "ABI_030": //swamp swimmer
                {

                }
                break;
            case "ABI_032": //pond gift
                {

                }
                break;
            case "ABI_033": //lava gift
                {

                }
                break;
            case "ABI_035": //sand gift
                {

                }
                break;
        }
    }
}
