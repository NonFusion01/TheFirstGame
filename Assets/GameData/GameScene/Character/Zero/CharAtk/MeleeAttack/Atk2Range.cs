using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk2Range : SlashRange
{
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadDamage();
    }

    protected virtual void LoadDamage()
    {
        this.damage = 15;
    }

    public virtual void SetOwner(Transform owner)
    {
        this.owner = owner;
    }

}
