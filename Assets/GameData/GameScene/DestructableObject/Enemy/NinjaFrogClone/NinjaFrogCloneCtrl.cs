using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaFrogCloneCtrl : EnemyNormalCtrl
{
    [Header("Ninja Frog Clone")]
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Collider2D objCollider2D;
    [SerializeField] public Rigidbody2D rb2D;
    [SerializeField] public NinjaFrogCloneAniCtrl aniCtrl;
    [SerializeField] public NinjaFrogCloneMovementCtrl movement;


    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadSpriteRenderer();
        this.LoadAniCtrl();
        this.LoadCollider2D();
        this.LoadRigidbody2D();
        this.LoadMovement();
        this.LoadInitialStats();
    }

    protected virtual void LoadSpriteRenderer()
    {
        if (this.spriteRenderer != null) return;
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Debug.LogWarning(transform.name + ": Load Sprite Renderer", gameObject);
    }

    protected virtual void LoadAniCtrl()
    {
        if (this.aniCtrl != null) return;
        this.aniCtrl = GetComponentInChildren<NinjaFrogCloneAniCtrl>();
        Debug.LogWarning(transform.name + ": Load Ani Ctrl", gameObject);
    }

    protected virtual void LoadCollider2D()
    {
        if (this.objCollider2D != null) return;
        this.objCollider2D = GetComponent<Collider2D>();
        Debug.LogWarning(transform.name + ": Load Collider2D", gameObject);
        Physics.IgnoreLayerCollision(12, 14);
        Physics2D.IgnoreLayerCollision(12, 14);
        Physics.IgnoreLayerCollision(14, 14);
        Physics2D.IgnoreLayerCollision(14, 14);
    }

    protected virtual void LoadRigidbody2D()
    {
        if (this.rb2D != null) return;
        this.rb2D = GetComponent<Rigidbody2D>();
        Debug.LogWarning(transform.name + ": Load Rigidbody2D", gameObject);
    }

    protected virtual void LoadMovement()
    {
        if (this.movement != null) return;
        this.movement = GetComponentInChildren<NinjaFrogCloneMovementCtrl>();
        Debug.LogWarning(transform.name + ": Load Movement", gameObject);
    }

    protected virtual void LoadInitialStats()
    {
        this.maxHp = 100;
        this.hp = this.maxHp;
    }

    protected override void ResetStats()
    {
        //base.ResetStats();
        // no reset stats for clone
    }
}
