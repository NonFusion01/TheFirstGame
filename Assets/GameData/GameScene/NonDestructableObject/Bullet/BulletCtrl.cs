using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BulletCtrl : CoreMonoBehaviour
{
    [SerializeField] public Collider2D bulletCollider2D;
    [SerializeField] public Rigidbody2D bulletRigidbody2D;
    [SerializeField] public Transform owner;
    [SerializeField] public bool isImpactable;
    [SerializeField] protected Transform impactPoint;
    [SerializeField] public float remainingTime;
    [SerializeField] public int damage = 5;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCollider();
        this.LoadRigidbody();
        this.LoadImpactPoint();
    }

    protected virtual void LoadCollider()
    {
        if (this.bulletCollider2D != null) return;
        this.bulletCollider2D = GetComponent<Collider2D>();
        Debug.LogWarning(transform.name + ": Load Collider", gameObject);
        this.bulletCollider2D.isTrigger = true;
    }

    protected virtual void LoadRigidbody()
    {
        if (this.bulletRigidbody2D != null) return;
        this.bulletRigidbody2D = GetComponent<Rigidbody2D>();
        Debug.LogWarning(transform.name + ": Load Rigidbody", gameObject);
        this.bulletRigidbody2D.gravityScale = 0;
    }

    protected virtual void LoadImpactPoint()
    {
        if (this.impactPoint != null) return;
        this.impactPoint = transform.Find("ImpactPoint");
        Debug.LogWarning(transform.name + ": Load Impact Point", gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Physics2D.IgnoreLayerCollision(0, 10); //ignore player and bg
        this.remainingTime = 1f;
    }

    public virtual void SetOwner(Transform owner)
    {
        this.owner = owner;
    }

    protected virtual void Update()
    {
        this.remainingTime -= Time.deltaTime;
        if (this.remainingTime <= 0) BulletSpawner.Instance.ReturnObjectToPool(this.transform);
    }

    protected void OnTriggerEnter2D (Collider2D other)
    {
        this.CheckEnemyShootPlayer(other);
        this.CheckPlayerShootObj(other);
        if (!this.isImpactable) return;
        //check if it hit terrain layer
        if (other.gameObject.layer == 11)
        {
            this.SpawnBulletImpact();
            BulletSpawner.Instance.ReturnObjectToPool(this.transform);
        }

        if (other.gameObject.GetComponent<Block>() != null)
        {
            this.SpawnBulletImpact();
            BulletSpawner.Instance.ReturnObjectToPool(this.transform);
        }
    }

    protected virtual void CheckEnemyShootPlayer(Collider2D other)
    {
        Enemy enemy = this.owner.GetComponent<Enemy>();
        CharController character = other.gameObject.GetComponent<CharController>();
        if ((enemy != null) && (character != null))
        {
            if (!character.charTakeDamage.isAbleToTakeDmg) return;
            character.charTakeDamage.CharTakingDamage(this.damage);
            if (this.isImpactable) BulletSpawner.Instance.ReturnObjectToPool(this.transform);
        }
    }

    protected virtual void CheckPlayerShootObj(Collider2D other)
    {
        CharController character = this.owner.GetComponent<CharController>();
        DestructableObject obj = other.gameObject.GetComponent<DestructableObject>();
        if ((character != null) && (obj != null))
        {
            if (!obj.isAbleToTakeDamage) return;
            obj.TakeDamage(this.damage);
            if (!this.isImpactable) return;
            this.SpawnBulletImpact();
            BulletSpawner.Instance.ReturnObjectToPool(this.transform);
        }
    }

    protected virtual void SpawnBulletImpact()
    {
        Transform newFX = FXSpawner.Instance.Spawn("FXBulletImpact_1", this.impactPoint.position, this.transform.rotation);
        newFX.gameObject.SetActive(true);
        newFX.localScale = this.transform.localScale;
    }
}
