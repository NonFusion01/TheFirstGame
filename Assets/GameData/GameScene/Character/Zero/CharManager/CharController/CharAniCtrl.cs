using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAniCtrl : CoreMonoBehaviour
{
    [SerializeField] protected CharController charCtrl;
    //animation
    [Header("Animation")]
    public string currentAnimation = "";
    public bool isNonLoopAnimation = false;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharCtrl();
    }

    protected virtual void LoadCharCtrl()
    {
        if (this.charCtrl != null) return;
        this.charCtrl = transform.parent.GetComponentInChildren<CharController>();
        Debug.LogWarning(transform.name + ": Load Char Ctrl", gameObject);
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
        var currentAnimationInfo = CharManager.Instance._charAnimator.GetCurrentAnimatorStateInfo(0);
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
        if (!this.charCtrl.isOnGround) return;
        if (this.charCtrl.isActionOcurr) return;
        this.PlayLoopAnimation("IdleAnimation");
    }

    public virtual void PlayLoopAnimation(string animationName)
    {
        if (this.currentAnimation != animationName && this.isNonLoopAnimation == false)
        {
            this.currentAnimation = animationName;
            CharManager.Instance._charAnimator.CrossFade(animationName, 0, 0);
        }
    }

    public virtual void PlayNonLoopAnimation(string animationName)
    {
        if (this.currentAnimation != animationName)
        {
            this.currentAnimation = animationName;
            CharManager.Instance._charAnimator.CrossFade(animationName, 0, 0);
        }
    }
}
