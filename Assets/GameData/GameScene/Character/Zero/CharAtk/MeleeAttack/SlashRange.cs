using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashRange : CoreMonoBehaviour
{
    [SerializeField] public Collider2D _collider2D;
    [SerializeField] public Transform owner;
    [SerializeField] public int damage;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCollider();
    }

    protected virtual void LoadCollider()
    {
        if (this._collider2D != null) return;
        this._collider2D = GetComponent<Collider2D>();
        Debug.LogWarning(transform.name + ": Load Collider", gameObject);
        this._collider2D.isTrigger = true;
        this.TurnOffCollider();
    }
    protected virtual void TurnOffCollider()
    {
        this._collider2D.enabled = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        DestructableObject obj = other.gameObject.GetComponent<DestructableObject>();
        if (obj != null)
        {
            if (!obj.isAbleToTakeDamage) return;
            obj.TakeDamage(this.damage); 
        }
    }
}
