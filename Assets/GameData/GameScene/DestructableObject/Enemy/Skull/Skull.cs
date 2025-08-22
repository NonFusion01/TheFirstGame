using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skull : EnemyNormalCtrl
{
    [Header("Skull")]
    [SerializeField] protected float moveSpeed = 1f;
    protected Vector3 startPos;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadStat();
    }

    protected virtual void LoadStat()
    {
        this.rb.gravityScale = 0;

        this.maxHp = 30;
        this.hp = this.maxHp;
        this.damage = 10;
    }

    protected override void Start()
    {
        this.LoadStartPosition();
        StartCoroutine(ChasePlayer());
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.ReturnToStartPosition();
    }

    protected virtual void LoadStartPosition()
    {
        this.startPos = this.transform.position;
    }

    protected IEnumerator ChasePlayer()
    {
        if (this.target != null) this.FollowTarget();
        yield return new WaitForSeconds(1f);
        this.rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ChasePlayer());
    }

    protected virtual void StayAtPosition()
    {
        if (this.target == null) return;
        float distance = Vector3.Distance(this.transform.position, this.target.position);
        if (distance < 0.1f)
        {
            this.rb.velocity = Vector3.zero;
        }
    }

    protected virtual void FollowTarget()
    {
        if (this.target == null) return;
        float distance = Vector3.Distance(this.transform.position, this.target.position);
        Vector3 direction = this.target.position - this.transform.position;
        if (distance < 0.5f)
        {
            this.rb.velocity = Vector3.zero;
        }
        else
        {
            this.rb.velocity = direction.normalized * this.moveSpeed;
            if (this.rb.velocity.x > 0f) this.model.flipX = true;
            else this.model.flipX = false;
        }
    }

    protected virtual void ReturnToStartPosition()
    {
        if (this.transform.position == this.startPos) return;
        if (this.target != null) return;
        float distance = Vector3.Distance(this.transform.position, this.startPos);
        Vector3 direction = this.startPos - this.transform.position;
        if (distance <= 0.05f)
        {
            this.rb.velocity = Vector3.zero;
            this.transform.position = this.startPos;
        }
        else
        {
            this.rb.velocity = direction.normalized * this.moveSpeed;
            if (this.rb.velocity.x > 0f) this.model.flipX = true;
            else this.model.flipX = false;
        }
    }

}
