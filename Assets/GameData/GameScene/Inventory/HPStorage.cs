using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HPStorage 
{
    public int maxHPStorage = 100;
    public int currentHPStorage;

    public virtual void AddHP(int addAmount)
    {
        this.currentHPStorage += addAmount;
    }
}
