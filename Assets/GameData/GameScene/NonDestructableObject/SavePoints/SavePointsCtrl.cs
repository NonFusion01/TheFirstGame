using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointsCtrl : CoreMonoBehaviour
{
    [SerializeField] protected Transform player;
    [SerializeField] protected SavePoint currentSavePoint;
    public SavePoint CurrentSavePoint => this.currentSavePoint;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayer();
    }

    protected virtual void LoadPlayer()
    {
        if (this.player != null) return;
        this.player = GameObject.Find("Player").transform;
        Debug.LogWarning(transform.name + ": Load Player Transform", gameObject);
    }

    protected override void Start()
    {
        base.Start();
        this.currentSavePoint = transform.GetChild(0).GetComponent<SavePoint>();
        this.SetPlayerPosition();
    }

    public virtual void SetCurrentSavePoint(SavePoint savePoint)
    {
        if (savePoint == this.currentSavePoint) return;
        if (savePoint.index < this.currentSavePoint.index) return;
        this.currentSavePoint = savePoint;
    }

    public virtual void SetPlayerPosition()
    {
        this.player.position = this.currentSavePoint.transform.position;
    }
}
