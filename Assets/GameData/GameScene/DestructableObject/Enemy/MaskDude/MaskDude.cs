using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskDude : EnemyNormalCtrl
{
    [Header("Mask Dude")]
    [SerializeField] protected List<Transform> movePoints;
    [SerializeField] protected Transform endPoint;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float bulletSpeed = 2f;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadStat();
        this.LoadMovePoints();
        this.LoadShootPoint();
    }

    protected virtual void LoadStat()
    {
        this.maxHp = 50;
        this.hp = this.maxHp;
        this.damage = 5;
    }

    protected virtual void LoadShootPoint()
    {
        if (this.shootPoint != null) return;
        this.shootPoint = transform.Find("ShootPoint");
        Debug.LogWarning(transform.name + ": Load Shoot Point", gameObject);
    }

    protected virtual void LoadMovePoints()
    {
        if (this.movePoints.Count > 0) return;
        Transform points = transform.parent.Find("MovePoints");
        foreach (Transform point in points)
        {
            this.movePoints.Add(point);
        }
        Debug.LogWarning(transform.name + ": Load Move Points", gameObject);
        this.endPoint = this.movePoints[0];
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(MoveToDesignatedPoint(this.endPoint));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.UpdateSpriteRenderer();
    }

    protected virtual void UpdateSpriteRenderer()
    {
        if (this.rb.velocity.x > 0.1)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
            this.NonLoopAnimationPlay("MaskDudeRunAnimation");
        }
        else if (this.rb.velocity.x < -0.1)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
            this.NonLoopAnimationPlay("MaskDudeRunAnimation");
        }
        else 
        { 
            this.PlayLoopAnimation("MaskDudeIdleAnimation");
        }
    }

    protected IEnumerator MoveToDesignatedPoint(Transform endPoint)
    {
        yield return new WaitForSeconds(2f);
        // decide move direction
        Vector3 vector = endPoint.position - this.transform.position;
        float distance = vector.magnitude;
        Vector3 direction = new Vector3(vector.x, 0, 0).normalized;
        while((this.target == null) && distance >= 0.1f)
        {
            distance = Vector3.Distance(this.transform.position, endPoint.position);
            this.rb.velocity = direction * this.moveSpeed;
            yield return null;
        }
        if (this.target != null)
        {
            this.rb.velocity = Vector3.zero;
            StartCoroutine(Shoot(target));
            StartCoroutine(MoveToDesignatedPoint(this.endPoint));
            yield break;
        }
        if (distance < 0.1f)
        {
            if (this.endPoint == this.movePoints[0]) 
            { 
                this.endPoint = this.movePoints[1];
                StartCoroutine(MoveToDesignatedPoint(this.endPoint));
                yield break;
            }
            if (this.endPoint == this.movePoints[1])
            {
                this.endPoint = this.movePoints[0];
                StartCoroutine(MoveToDesignatedPoint(this.endPoint));
                yield break;
            }
        }
        
    }

    protected IEnumerator Shoot(Transform target)
    {
        if (this.underCrowdControl) yield break;
        Vector3 vector3 = target.position - this.transform.position;
        Vector3 direction = new Vector3(vector3.x / Mathf.Abs(vector3.x), 1, 1);
        this.transform.localScale = direction;
        yield return new WaitForSeconds(1f);
        Transform newBullet = BulletSpawner.Instance.Spawn("Bullet_Enemy", this.shootPoint.position, Quaternion.identity);
        newBullet.localScale = direction; 
        newBullet.gameObject.SetActive(true);
        BulletCtrl bulletCtrl = newBullet.GetComponent<BulletCtrl>();
        bulletCtrl.SetOwner(this.transform);
        bulletCtrl.remainingTime = 5f;
        bulletCtrl.isImpactable = true;
        bulletCtrl.bulletRigidbody2D.velocity = new Vector3(direction.x, 0, 0) * this.bulletSpeed;
        bulletCtrl.damage = this.damage;
        yield break;
    }
}
