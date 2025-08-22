using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : CoreMonoBehaviour
{
    [SerializeField] protected bool canDespawn = false;

    protected virtual void FixedUpdate()
    {
        if (!this.canDespawn) return;
        this.DespawnObject();
    }

    protected virtual void DespawnObject()
    {

    }
}
