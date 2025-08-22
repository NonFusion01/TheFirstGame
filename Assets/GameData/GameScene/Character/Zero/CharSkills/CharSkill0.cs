using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSkill0 : CharBaseSkill
{
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadInitialValue();
    }

    protected virtual void LoadInitialValue()
    {
        this.maxEnergy = 0;
    }

    public override void CastSkill()
    {
        //Alternative for normal attack
    }

}
