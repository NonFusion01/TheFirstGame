using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalCtrl : Enemy
{
    [Header("Normal Enemy")]
    //small HP Bar, reset stats
    [SerializeField] protected Transform hpBar;

    //check reset stats time
    protected float resetTime = 5f;
    protected float resetTimeLeft;

    protected override void Start()
    {
        base.Start();
        this.ResetStats();
    }

    protected virtual void FixedUpdate()
    {
        this.resetTimeLeft -= Time.fixedDeltaTime;
        if (this.resetTimeLeft < 0) this.resetTimeLeft = 0;
        if (this.resetTimeLeft == 0) this.ResetStats();
        this.CheckStatus();
    }

    protected virtual void ResetStats()
    {
        this.resetTimeLeft = this.resetTime;
        if (this.hpBar == null) return;
        if (this.isDead) return;
        this.hp = this.maxHp;
        this.hpBar.gameObject.SetActive(false);
    }

    protected virtual void CheckStatus()
    {
        if (this.hp == 0)
        {
            this.isDead = true;
            this.rb.velocity = Vector2.zero;
            this.transform.gameObject.SetActive(false);
        }
        else this.isDead = false;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        this.resetTimeLeft = this.resetTime;
        if (this.hpBar == null)
        {
            this.hpBar = HPBarSpawner.Instance.Spawn("EnemyHPBar_1", this.transform.position, Quaternion.identity);
            this.hpBar.gameObject.SetActive(true);
        }
        if (this.hpBar != null) this.hpBar.gameObject.SetActive(true);

        HPBarCtrl hpBarCtrl = hpBar.GetComponent<HPBarCtrl>();
        hpBarCtrl.SetOwner(this.transform);
        hpBarCtrl.maxHP = this.maxHp;
        hpBarCtrl.currentHP = this.hp;
        hpBarCtrl.UpdateHP();
    }
}
