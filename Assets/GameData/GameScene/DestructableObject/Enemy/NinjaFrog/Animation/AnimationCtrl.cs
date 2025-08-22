using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCtrl : CoreMonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected bool isNonLoopAnimation = false;
    [SerializeField] protected string currentAnimation = "";

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadAnimator();
    }

    protected virtual void LoadAnimator()
    {
        if (this.animator != null) return;
        this.animator = GetComponent<Animator>();
        Debug.LogWarning(transform.name + ": Load Animator", gameObject);
    }

    public virtual void NonLoopAnimationPlay(string animationName)
    {
        StartCoroutine(PrepareAndPlayNonLoopAnimation(animationName));
    }

    public IEnumerator PrepareAndPlayNonLoopAnimation(string animationName)
    {
        this.isNonLoopAnimation = true;
        this.PlayNonLoopAnimation(animationName);
        yield return new WaitForEndOfFrame();
        var currentAnimationInfo = this.animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimationInfo.IsName(animationName))
        {
            var animationDuration = currentAnimationInfo.length;
            yield return new WaitForSeconds(animationDuration);
            this.isNonLoopAnimation = false;
        }
        else
        {
            yield return null;
            this.isNonLoopAnimation = false;
        }
    }

    public virtual void AnimationStop()
    {
        //this.PlayLoopAnimation("NinjaFrogIdleAnimation");
    }

    public virtual void PlayLoopAnimation(string animationName)
    {
        if (this.currentAnimation != animationName && this.isNonLoopAnimation == false)
        {
            this.currentAnimation = animationName;
            this.animator.Play(animationName);
        }
    }

    public virtual void PlayNonLoopAnimation(string animationName)
    {
        if (this.currentAnimation != animationName)
        {
            this.currentAnimation = animationName;
            this.animator.Play(animationName);
        }
    }
}
