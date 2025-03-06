using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(SkeletonAnimation))]
public class SpineAnimationController : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;
    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public void Init(TowerData data)
    {
        skeletonAnimation.skeletonDataAsset = data.towerType;
        skeletonAnimation.initialSkinName = data.towerInitialSkin;
    }
    public void Init(EnemyData data)
    {
        skeletonAnimation.skeletonDataAsset = data.enemyType;
        skeletonAnimation.initialSkinName = data.enemyInitialSkin;
    }

    public void SpawnAnimation()
    {
        PlayAnimationOnce("Build", "Idle");
    }

    public void AttackAnimation()
    {
        PlayAnimationOnce("Attack", "Idle");
    }

    public void PlayAnimationOnce(string animationName, string fallbackAnimation)
    {
        var state = skeletonAnimation.AnimationState;

        // Play the new animation once (false = don't loop)
        state.SetAnimation(0, animationName, false);

        // Queue "Idle" to play after (true = loop)
        state.AddAnimation(0, fallbackAnimation, true, 0);
    }
    public void PlayAnimation(string animationName)
    {
        var state = skeletonAnimation.AnimationState;
        state.SetAnimation(0, animationName, true);
    }
}
