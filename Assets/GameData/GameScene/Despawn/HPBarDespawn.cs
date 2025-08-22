using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarDespawn : DespawnByTime
{
    [SerializeField] protected HPBarCtrl hpBarCtrl;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadHPBarCtrl();
    }

    protected virtual void LoadHPBarCtrl()
    {
        if (this.hpBarCtrl != null) return;
        this.hpBarCtrl = GetComponent<HPBarCtrl>();
        Debug.LogWarning(transform.name + ": Load HP Bar Ctrl", gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.remainingTime = 5f;
    }

    protected virtual void Update()
    {
        this.remainingTime -= Time.deltaTime;
        if (this.remainingTime < 0) this.remainingTime = 0;
        if (this.remainingTime <= 0) this.canDespawn = true;
        if (this.hpBarCtrl.owner.gameObject.activeSelf == false) this.canDespawn = true;
    }

    protected override void DespawnObject()
    {
        HPBarSpawner.Instance.ReturnObjectToPool(this.transform);
        this.canDespawn = false;
    }
}
