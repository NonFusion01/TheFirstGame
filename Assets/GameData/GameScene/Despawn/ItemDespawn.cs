using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDespawn : DespawnByTime
{
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
    }

    protected override void DespawnObject()
    {
        ItemDropSpawner.Instance.ReturnObjectToPool(this.transform);
        this.canDespawn = false;
    }
}
