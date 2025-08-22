using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk1Range : SlashRange
{
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadDamage();
    }

    protected virtual void LoadDamage()
    {
        this.damage = 10;
    }

    public virtual void SetOwner(Transform owner)
    {
        this.owner = owner;
    }
}
