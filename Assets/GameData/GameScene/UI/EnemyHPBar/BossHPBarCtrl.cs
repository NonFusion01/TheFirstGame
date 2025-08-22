using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBarCtrl : CoreMonoBehaviour
{
    [SerializeField] public EnemyBossCtrl enemyBossCtrl;
    [SerializeField] public Image bossImage;
    [SerializeField] protected Image bossHPBar;
    [SerializeField] public int hp;
    [SerializeField] public int maxHp;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadEnemyBossCtrl();
        this.LoadBossImage();
        this.LoadBossHPBar();
    }

    protected virtual void LoadEnemyBossCtrl()
    {
        if (this.enemyBossCtrl != null) return;
        this.enemyBossCtrl = GameObject.Find("EnemyBoss").GetComponentInChildren<EnemyBossCtrl>();
        Debug.LogWarning(transform.name + ": Load Enemy Boss Ctrl", gameObject);
    }

    protected virtual void LoadBossImage()
    {
        if (this.bossImage != null) return;
        this.bossImage = transform.Find("BossIcon").GetComponent<Image>();
        Debug.LogWarning(transform.name + ": Load Boss Image", gameObject);
    }

    protected virtual void LoadBossHPBar()
    {
        if (this.bossHPBar != null) return;
        this.bossHPBar = transform.Find("BossHPBar").Find("HP").GetComponent<Image>();
        Debug.LogWarning(transform.name + ": Load Boss HP Bar", gameObject);
    }

    protected override void Start()
    {
        base.Start();
        this.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        this.UpdateBossHPBar();
    }

    protected virtual void UpdateBossHPBar()
    {
        this.hp = this.enemyBossCtrl.Hp;
        this.maxHp = this.enemyBossCtrl.MaxHp;
        this.bossHPBar.fillAmount = (float)this.hp / (float)this.maxHp;
    }
}
