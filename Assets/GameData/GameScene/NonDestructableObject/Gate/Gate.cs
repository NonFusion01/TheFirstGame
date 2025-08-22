using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : CoreMonoBehaviour
{
    [SerializeField] protected Transform startPosition;
    [SerializeField] protected Transform stopPosition;
    protected float distance;
    protected float movingTime = 1f;
    protected float progressTime = 0f;
    public bool isAbleToMove = false;
    public bool isMoved = false;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadStartPosition();
        this.LoadStopPosition();
    }

    protected virtual void LoadStartPosition()
    {
        if (this.startPosition != null) return;
        this.startPosition = transform.parent.Find("StartPosition");
        Debug.LogWarning(transform.name + ": Load Start Position", gameObject);
    }
    protected virtual void LoadStopPosition()
    {
        if (this.stopPosition != null) return;
        this.stopPosition = transform.parent.Find("StopPosition");
        Debug.LogWarning(transform.name + ": Load Stop Position", gameObject);
    }

    public IEnumerator MoveGate()
    {
        this.distance = Vector3.Distance(this.transform.position, this.stopPosition.position);
        while (distance > 0.1f)
        {
            yield return null;
            this.progressTime += Time.deltaTime;
            if (this.progressTime > this.movingTime) this.progressTime = this.movingTime;
            float completePercent = this.progressTime / this.movingTime;
            this.transform.position = Vector3.Lerp(this.startPosition.position, this.stopPosition.position, completePercent);
            this.distance = Vector3.Distance(this.transform.position, this.stopPosition.position);
        }
        if (this.distance <= 0.1f)
        {
            this.progressTime = 0f;
            this.transform.position = this.stopPosition.position;
            this.isMoved = true;
            this.isAbleToMove = false;
        }
    }

    public virtual void ResetGatePosition()
    {
        this.progressTime = 0f;
        this.transform.position = this.startPosition.position;
        this.isMoved = false;
        this.isAbleToMove = true;
    }
}
