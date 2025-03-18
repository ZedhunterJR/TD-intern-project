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
                    
                }
                break;
            case "ABI_012": //sand cover
                {
                    
                }
                break;
            case "ABI_016": //hp up 1
                {
                    
                }
                break;
            case "ABI_017": //hp up 2
                {
                    
                }
                break;
            case "ABI_018": //hp up 3
                {
                    
                }
                break;
            case "ABI_019": //spd 1
                {
                    
                }
                break;
            case "ABI_020": //spd 2
                {
                    
                }
                break;
            case "ABI_021": //spd 3
                {
                    
                }
                break;
            case "ABI_022": //pond create
                {
                    enemy.AbilityUpdates.Add(new(() =>
                    {
                        var path = PathManager.Instance.GetCurrentPathEntity(enemy.CurrentPositionInAbs);
                        if (path != null)
                            path.InflictLandMaking(PathType.Pond);
                    }, 5f));
                }
                break;
            case "ABI_023": //lava create
                {
                    enemy.AbilityUpdates.Add(new(() =>
                    {
                        var path = PathManager.Instance.GetCurrentPathEntity(enemy.CurrentPositionInAbs);
                        if (path != null)
                            path.InflictLandMaking(PathType.Lava);
                    }, 5f));
                }
                break;
            case "ABI_025": //sand create
                {
                    enemy.AbilityUpdates.Add(new(() =>
                    {
                        var path = PathManager.Instance.GetCurrentPathEntity(enemy.CurrentPositionInAbs);
                        if (path != null)
                            path.InflictLandMaking(PathType.DirtyMist);
                    }, 5f));
                }
                break;
            case "ABI_027": //water diving
                {
                    enemy.EnteringTile += (path) =>
                    {
                        if (path == PathType.Pond)
                            enemy.isUntargetable = 3;
                    };
                }
                break;
            case "ABI_028": //fire dash
                {
                    enemy.EnteringTile = (path) =>
                    {
                        if (path == PathType.Lava)
                            enemy.AddEffect(new(2f, 2f));
                    };
                }
                break;
            case "ABI_030": //swamp swimmer
                {
                    enemy.EnteringTile = (path) =>
                    {
                        if (path == PathType.Swamp)
                            enemy.AddEffect(new(2f, 2f));
                    };
                }
                break;
            case "ABI_032": //pond gift
                {
                    enemy.PreDestruction += () =>
                    {
                        var path = PathManager.Instance.GetCurrentPathEntity(enemy.CurrentPositionInAbs);
                        if (path != null)
                            path.InflictLandMaking(PathType.Pond);
                    };
                }
                break;
            case "ABI_033": //lava gift
                {
                    enemy.PreDestruction += () =>
                    {
                        var path = PathManager.Instance.GetCurrentPathEntity(enemy.CurrentPositionInAbs);
                        if (path != null)
                            path.InflictLandMaking(PathType.Lava);
                    };
                }
                break;
            case "ABI_035": //sand gift
                {
                    enemy.PreDestruction += () =>
                    {
                        var path = PathManager.Instance.GetCurrentPathEntity(enemy.CurrentPositionInAbs);
                        if (path != null)
                            path.InflictLandMaking(PathType.DirtyMist);
                    };
                }
                break;
        }
    }
}
