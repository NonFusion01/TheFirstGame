using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharBaseSkill : CoreMonoBehaviour
{
    [SerializeField] protected SpriteRenderer icon;
    public SpriteRenderer Icon => icon;
    [SerializeField] protected CharController charCtrl;
    [SerializeField] public bool isInSkillDuration = false;
    [SerializeField] protected int maxEnergy = 100;
    public int MaxEnergy => maxEnergy;
    [SerializeField] public int currentEnergy;
    [SerializeField] protected int energyRequired;
    [SerializeField] protected float skillCooldown;
    [SerializeField] protected float timeRemaining;
    [SerializeField] protected bool isOnCooldown = false;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadIcon();
        this.LoadCharCtrl();
    }

    protected virtual void LoadIcon()
    {
        if (this.icon != null) return;
        this.icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
        Debug.LogWarning(transform.name + ": Load Icon", gameObject);
        this.icon.gameObject.SetActive(false);
    }

    protected virtual void LoadCharCtrl()
    {
        if (this.charCtrl != null) return;
        this.charCtrl = transform.parent.parent.GetComponent<CharController>();
        Debug.LogWarning(transform.name + ": Load Char Ctrl", gameObject);
    }

    public abstract void CastSkill();

    public virtual void CancelSkill()
    {

    }
}
