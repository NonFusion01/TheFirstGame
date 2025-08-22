using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyNormalCtrl
{
    [Header("Bat")]
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected float moveSpeed = 1f;

    protected Vector3 startPos;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadStat();
        this.LoadShootPoint();
    }

    protected virtual void LoadStat()
    {
        this.rb.gravityScale = 0;

        this.maxHp = 40;
        this.hp = this.maxHp;
        this.damage = 5;
    }

    protected virtual void LoadShootPoint()
    {
        if (this.shootPoint != null) return;
        this.shootPoint = transform.Find("ShootPoint");
        Debug.LogWarning(transform.name + ": Load Shoot Point", gameObject);
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
        if (this.hp == 0) this.DropItems();
    }

    protected virtual void LoadStartPosition()
    {
        this.startPos = this.transform.position;
    }

    protected IEnumerator ChasePlayer()
    {
        if (this.target != null) this.FollowTarget();
        yield return new WaitForSeconds(1f);
        StartCoroutine(ChasePlayer());
    }

    protected virtual void FollowTarget()
    {
        if (this.target == null) return;
        if (this.underCrowdControl) return;
        float distance = Vector3.Distance(this.transform.position, this.target.position);
        Vector3 direction = this.target.position - this.transform.position;
        if (distance < 2f)
        {
            this.rb.velocity = Vector3.zero;
            this.ShootTarget(direction);
        }
        else
        {
            this.rb.velocity = direction.normalized * this.moveSpeed;
            if (this.rb.velocity.x > 0f) this.model.flipX = true;
            else this.model.flipX = false;
        }
    }

    protected virtual void ShootTarget(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Transform newBullet = BulletSpawner.Instance.Spawn("Bullet_Enemy", this.shootPoint.position, Quaternion.identity);
        newBullet.rotation = Quaternion.Euler(0, 0, angle);
        newBullet.gameObject.SetActive(true);
        BulletCtrl bulletCtrl = newBullet.GetComponent<BulletCtrl>();
        bulletCtrl.SetOwner(this.transform);
        bulletCtrl.remainingTime = 5f;
        bulletCtrl.isImpactable = true;
        bulletCtrl.bulletRigidbody2D.velocity = direction.normalized;
        bulletCtrl.damage = this.damage;
    }

    protected virtual void ReturnToStartPosition()
    {
        if (this.underCrowdControl) return;
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

    protected virtual void DropItems()
    {
        Transform dropItem = ItemDropSpawner.Instance.RandomPrefabByRate();
        Transform newDropItem = ItemDropSpawner.Instance.Spawn(dropItem, this.transform.position, Quaternion.identity);
        Item item = newDropItem.GetComponent<Item>();
        item.itemCount = 1;
        newDropItem.gameObject.SetActive(true);
    }
}
