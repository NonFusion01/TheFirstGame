using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : DestructableObject
{
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadStat();
    }

    protected virtual void LoadStat()
    {
        this.maxHp = 10;
        this.hp = this.maxHp;
    }
    
    protected virtual void Update()
    {
        this.CheckHP();
    }

    protected virtual void CheckHP()
    {
        if (this.hp > 0) return;
        this.SpawnExplosionEffect();
        this.gameObject.SetActive(false);
    }

    protected virtual void SpawnExplosionEffect()
    {
        Transform newFX = FXSpawner.Instance.Spawn("ExplosionEffect", this.transform.position, Quaternion.identity);
        newFX.gameObject.SetActive(true);
        newFX.transform.localScale = Vector3.one;
    }
}
