using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : CoreMonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] protected float moveTime = 1f;

    public virtual void SetTarget(Transform target)
    {
        this.target = target;
    }

    protected virtual void FixedUpdate()
    {
        this.MoveToTarget();
    }

    protected virtual void MoveToTarget()
    {
        this.moveTime -= Time.fixedDeltaTime;
        this.transform.position = Vector3.Lerp(this.transform.position, this.target.position, this.moveTime);
        if (Vector3.Distance(this.transform.position, this.target.position) < 0.01f)
        {
            this.transform.position = this.target.position;
            this.moveTime = 1f;
        }
    }
}
