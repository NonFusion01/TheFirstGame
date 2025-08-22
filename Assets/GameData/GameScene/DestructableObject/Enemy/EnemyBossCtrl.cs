using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossCtrl : Enemy
{
    [Header("Boss Enemy")]
    //Big HP Bar, no reset stats
    [SerializeField] protected BossHPBarCtrl bossHPBarCtrl;
    [SerializeField] protected SpriteRenderer icon;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBossHPBarCtrl();
        this.LoadIcon();
    }

    protected virtual void LoadBossHPBarCtrl()
    {
        if (this.bossHPBarCtrl != null) return;
        this.bossHPBarCtrl = GameObject.Find("BossHPBarCtrl").GetComponent<BossHPBarCtrl>();
        Debug.LogWarning(transform.name + ": Load Boss HP Bar Ctrl", gameObject);
    }

    protected virtual void LoadIcon()
    {
        if (this.icon != null) return;
        this.icon = transform.parent.Find("Icon").GetComponent<SpriteRenderer>();
        Debug.LogWarning(transform.name + ": Load Icon", gameObject);
        this.bossHPBarCtrl.bossImage.sprite = this.icon.sprite;
        this.icon.gameObject.SetActive(false);
    }
}
