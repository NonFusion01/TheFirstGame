using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubClass2 : MonoBehaviour
{
    public BaseClass baseClass;
    void Start()
    {
        this.baseClass.damage = 3.6f;
        InvokeRepeating(nameof(GetDebug), 2f, 1f);

    }
    protected virtual void GetDebug()
    {
        Debug.Log("Damage in baseclass is:" + this.baseClass.damage);
    }
}
    
