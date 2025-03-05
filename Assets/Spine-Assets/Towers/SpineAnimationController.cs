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

    [ContextMenu("Build")]
    public void SpawnAnimtion()
    {
        PlayAnimationOnce("Build", "Idle");
    }

    [ContextMenu("attack")]
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
}
