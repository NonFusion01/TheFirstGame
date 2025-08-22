using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformCtrl : CoreMonoBehaviour
{
    [SerializeField] protected Collider2D objCollider;
    [SerializeField] protected Rigidbody2D objRb;
    [SerializeField] protected List<Transform> movePoints;
    [SerializeField] protected Transform point1;
    [SerializeField] protected Transform point2;
    [SerializeField] protected float platformMoveSpeed = 1f;

    // lerp function
    protected int pointID;
    protected string direction;

    // velocity method
    [SerializeField] protected bool isAbleToMove = true;
    [SerializeField] protected bool isMoving = false;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCollider();
        this.LoadRigidBody();
        this.LoadMovePoints();
    }

    protected virtual void LoadCollider()
    {
        if (this.objCollider != null) return;
        this.objCollider = GetComponent<Collider2D>();
        Debug.LogWarning(transform.name + ": Load Collider", gameObject);
    }

    protected virtual void LoadRigidBody()
    {
        if (this.objRb != null) return;
        this.objRb = GetComponent<Rigidbody2D>();
        Debug.LogWarning(transform.name + ": Load Rigidbody", gameObject);
        this.objRb.bodyType = RigidbodyType2D.Kinematic;
    }

    protected virtual void LoadMovePoints()
    {
        if (this.movePoints.Count != 0) return;
        Transform points = transform.parent.Find("MovePoints");
        foreach(Transform point in points)
        {
            this.movePoints.Add(point);
        }
        this.HideObject(points);
    }

    protected virtual void HideObject(Transform obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        this.SetInitialValue();
    }

    protected virtual void SetInitialValue()
    {
        this.direction = "forward";
        this.pointID = 0;
        this.point1 = this.movePoints[0];
        this.point2 = this.movePoints[1];
        if (this.point1 == null || this.point2 == null) Debug.LogWarning("Point Missing");
    }

    protected virtual void FixedUpdate()
    {
        if (Vector3.Distance(this.transform.position, point2.position) <= 0.05f) this.CheckNextPath();
        if (!this.isAbleToMove) return;
        if (this.isMoving) return;
        this.isMoving = true;
        this.isAbleToMove = false;
        this.MovingPlatform(point1, point2);
    }

    //velocity function
    protected virtual void MovingPlatform(Transform startPoint, Transform endPoint)
    {
        
        this.transform.position = startPoint.position;
        Vector3 direction = endPoint.position - startPoint.position;
        this.objRb.velocity = direction.normalized * this.platformMoveSpeed;
    }

    protected virtual void CheckNextPath()
    {
        this.isMoving = false;
        this.ChangeDirection();
        this.ChangePath();
        this.isAbleToMove = true;
    }

    protected virtual void ChangeDirection()
    {
        if ((this.pointID == this.movePoints.Count - 2) && (this.direction == "forward"))
        {
            this.direction = "backward";
            this.pointID += 2;
            return;
        }
        if ((this.pointID == 1) && (this.direction == "backward"))
        {
            this.direction = "forward";
            this.pointID -= 2;
            return;
        }
    }

    protected virtual void ChangePath()
    {
        if (this.direction == "forward")
        {
            this.pointID++;
            point1 = this.movePoints[pointID];
            point2 = this.movePoints[pointID + 1];
        }
        if (this.direction == "backward")
        {
            this.pointID--;
            point1 = this.movePoints[pointID];
            point2 = this.movePoints[pointID - 1];
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        CharController character = other.transform.GetComponent<CharController>();
        if (character == null) return;
        if (point2.position.x > point1.position.x)
        {
            character.charMovement.groundVelocity = this.platformMoveSpeed;

        }
        if (point2.position.x < point1.position.x)
        {
            character.charMovement.groundVelocity = -this.platformMoveSpeed;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D other)
    {
        CharController character = other.transform.GetComponent<CharController>();
        if (character == null) return;
        character.charMovement.groundVelocity = 0;
    }
}
