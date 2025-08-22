using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnergyStorage 
{
    [SerializeField] protected int maxEnergyStorage = 500;
    public int MaxEnergyStorage => maxEnergyStorage;
    public int currentEnergyStorage;
}
