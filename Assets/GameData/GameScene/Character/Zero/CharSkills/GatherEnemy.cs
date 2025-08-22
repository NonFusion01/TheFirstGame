using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherEnemy : CoreMonoBehaviour
{
    [SerializeField] protected Collider2D objCollider;
    [SerializeField] protected float effectRemainingTime = 5f;
    [SerializeField] protected float gatherForce = 2f;
    [SerializeField] protected int damage = 1;
    protected bool isGathering = false;
    protected bool inflictContinuousDamage = false;

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
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.isGathering = true;
        this.inflictContinuousDamage = true;
        StartCoroutine(EffectRemaining());
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInChildren<Enemy>();
        EnemyBossCtrl enemyBoss = other.GetComponentInChildren<EnemyBossCtrl>();
        if (enemyBoss != null)
        {
            StartCoroutine(InflictContinuousDamage(enemy, 1f));
            return;
        }
        if (enemy != null)
        {
            enemy.underCrowdControl = true;
            StartCoroutine(Gather(enemy, 0.5f));
            StartCoroutine(InflictContinuousDamage(enemy, 1f));
        }
    }

    protected IEnumerator EffectRemaining()
    {
        yield return new WaitForSeconds(this.effectRemainingTime);
        this.isGathering = false;
        this.inflictContinuousDamage = false;
        yield break;
    }

    protected IEnumerator Gather(Enemy enemy, float time)
    {
        Vector3 direction = this.transform.position - enemy.transform.position;
        enemy.rb.velocity = direction.normalized * this.gatherForce * direction.magnitude;
        yield return new WaitForSeconds(time);
        if (this.isGathering) StartCoroutine(Gather(enemy, time));
        else 
        {
            enemy.underCrowdControl = false;
            enemy.rb.velocity = Vector3.zero;
            yield break;
        }
    }

    protected IEnumerator InflictContinuousDamage(Enemy enemy, float time)
    {
        enemy.TakeDamage(this.damage);
        yield return new WaitForSeconds(time);
        if (this.inflictContinuousDamage) StartCoroutine(InflictContinuousDamage(enemy, time));
    }
}
