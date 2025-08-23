using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : CoreMonoBehaviour
{
    [Header("Destructable Object")]
    [SerializeField] protected Collider2D objCollider;
    [SerializeField] public SpriteRenderer model;
    public Vector4 currentColor = new Vector4(1, 1, 1, 1);
    [SerializeField] protected int maxHp;
    public int MaxHp => maxHp;
    [SerializeField] protected int hp;
    public int Hp => hp;

    public bool isAbleToTakeDamage = true;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCollider();
        this.LoadModel();
    }

    protected virtual void LoadCollider()
    {
        if (this.objCollider != null) return;
        this.objCollider = GetComponentInChildren<Collider2D>();
        Debug.LogWarning(transform.name + ": Load Collider", gameObject);
    }
    protected virtual void LoadModel()
    {
        if (this.model != null) return;
        this.model = GetComponentInChildren<SpriteRenderer>();
        Debug.LogWarning(transform.name + ": Load Model", gameObject);
    }

    public virtual void TakeDamage(int damage)
    {
        this.hp -= damage;
        if (this.hp < 0) this.hp = 0;
        if (this.gameObject.activeSelf) StartCoroutine(TakeDamageOnScene());
    }

    protected IEnumerator TakeDamageOnScene()
    {
        this.currentColor = new Vector4(1, 0, 0, currentColor.w);
        this.model.color = this.currentColor;
        yield return new WaitForSeconds(0.2f);
        this.currentColor = new Vector4(1, 1, 1, currentColor.w);
        this.model.color = this.currentColor;
    }
}
