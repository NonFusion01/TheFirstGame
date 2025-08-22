using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPBarCtrl : CoreMonoBehaviour
{
    [SerializeField] protected Slider slider;
    [SerializeField] public Transform owner;
    [SerializeField] protected FollowTarget followTarget;

    [SerializeField] public HPBarDespawn hpBarDespawn;
    [SerializeField] public int maxHP;
    [SerializeField] public int currentHP;
    protected float lastHPPrecent = 0;
    [SerializeField] protected float currentHPPercent;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadSlider();
        this.LoadHPBarDespawn();
        this.LoadFollowTarget();
    }

    protected virtual void LoadSlider()
    {
        if (this.slider != null) return;
        this.slider = GetComponentInChildren<Slider>();
        Debug.LogWarning(transform.name + ": Load Slider", gameObject);
    }

    protected virtual void LoadHPBarDespawn()
    {
        if (this.hpBarDespawn != null) return;
        this.hpBarDespawn = GetComponentInChildren<HPBarDespawn>();
        Debug.LogWarning(transform.name + ": Load HPBarDespawn", gameObject);
    }

    protected virtual void LoadFollowTarget()
    {
        if (this.followTarget != null) return;
        this.followTarget = GetComponentInChildren<FollowTarget>();
        Debug.LogWarning(transform.name + ": Load FollowTarget", gameObject);
    }

    public virtual void SetOwner(Transform owner)
    {
        this.owner = owner;
        this.followTarget.SetTarget(this.owner);
    }

    public virtual void UpdateHP()
    {
        this.currentHPPercent = (float) this.currentHP / (float) this.maxHP;
        if (this.currentHPPercent != this.lastHPPrecent)
        {
            this.slider.value = this.currentHPPercent;
            this.lastHPPrecent = this.currentHPPercent;
            this.hpBarDespawn.remainingTime = 5f;
        }
    }
}
