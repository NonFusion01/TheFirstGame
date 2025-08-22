using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DamageByImpact : CoreMonoBehaviour
{
    [SerializeField] protected Collider2D objCollider;
    [SerializeField] protected int damage = 2;
    protected bool isImpactedByPlayer = false;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCollider();
    }

    protected virtual void LoadCollider()
    {
        if (this.objCollider != null) return;
        this.objCollider = GetComponent<Collider2D>();
        Debug.LogWarning(transform.name + ": Load Collider", gameObject);
        this.objCollider.isTrigger = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        CharController character = other.GetComponent<CharController>();
        if (character == null) return;
        this.isImpactedByPlayer = true;
        StartCoroutine(this.ContinuousDmg(character));
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        CharController character = other.GetComponent<CharController>();
        if (character == null) return;
        this.isImpactedByPlayer = false;
    }

    protected IEnumerator ContinuousDmg(CharController character)
    {
        yield return null;
        character.charTakeDamage.CharTakingDamage(this.damage);
        if (this.isImpactedByPlayer) StartCoroutine(this.ContinuousDmg(character));
    }
}
