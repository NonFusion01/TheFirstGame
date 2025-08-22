using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : CoreMonoBehaviour
{
    [SerializeField] protected Image hpBar;
    [SerializeField] protected CharController charCtrl;
    [SerializeField] protected Image charIcon;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadHPBar();
        this.LoadCharCtrl();
        this.LoadCharIcon();
    }

    protected virtual void LoadHPBar()
    {
        if (this.hpBar != null) return;
        this.hpBar = transform.Find("HPBar").GetComponent<Image>();
        Debug.LogWarning(transform.name + ": Load HP Bar", gameObject);
    }

    protected virtual void LoadCharCtrl()
    {
        if (this.charCtrl != null) return;
        this.charCtrl = GameObject.Find("Player").GetComponentInChildren<CharController>();
        Debug.LogWarning(transform.name + ": Load Char Ctrl", gameObject);
    }

    protected virtual void LoadCharIcon()
    {
        if (this.charIcon != null) return;
        this.charIcon = transform.Find("CharIcon").GetComponent<Image>();
        Debug.LogWarning(transform.name + ": Load Char Icon", gameObject);
        //this.charIcon.sprite = CharManager.Instance._charIcon.sprite;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.UpdateIcon();
        this.UpdateHP();
    }

    protected virtual void FixedUpdate()
    {
        this.UpdateHP();
    }

    protected virtual void UpdateIcon()
    {
        this.charIcon.sprite = CharManager.Instance._charIcon.sprite;
    }

    public virtual void UpdateHP()
    {
        int maxHp = CharManager.Instance._charStats.maxHP;
        int currentHp = CharManager.Instance._charStats.currentHP;
        this.hpBar.fillAmount = (float)currentHp / maxHp;
    }
}
