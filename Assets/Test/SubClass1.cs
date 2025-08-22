using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubClass1 : BaseClass
{
    // Start is called before the first frame update
    void Start()
    {
        this.damage = 2f;
        InvokeRepeating(nameof(GetDebug), 2f, 1f);

    }
    protected virtual void GetDebug()
    {
        Debug.Log("Damage in subclass 1 is:" + this.damage);
    }
}
