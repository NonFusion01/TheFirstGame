using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : CoreMonoBehaviour
{
    [SerializeField] protected Image hpBar;
    protected int newHp;
    protected int oldHp = 0;
    protected int maxHp;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadHpBar();
    }

    protected virtual void LoadHpBar()
    {
        if (this.hpBar != null) return;
        this.hpBar = GetComponent<Image>();
        Debug.LogWarning(transform.name + ": Load HP Bar", gameObject);
    }

    protected override void Start()
    {
        base.Start();
        this.SetValue();
    }

    protected virtual void FixedUpdate()
    {
        this.UpdateHpBar();
    }

    protected virtual void SetValue()
    {
        this.maxHp = CharManager.Instance._charStats.maxHP;
    }

    protected virtual void UpdateHpBar()
    {
        this.newHp = CharManager.Instance._charStats.currentHP;
        if (this.newHp != this.oldHp)
        {
            this.hpBar.fillAmount = (float)this.newHp / this.maxHp;
            this.oldHp = this.newHp;
        }
    }
}
