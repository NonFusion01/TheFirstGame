using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDrop", menuName = "SO/ItemDrop")]
public class DropItemSO : ScriptableObject
{
    [SerializeField] protected itemName itemName;
    public itemName ItemName => itemName;
    [SerializeField] protected itemType itemType;
    [SerializeField] protected Sprite itemSprite;
    public Sprite ItemSprite => itemSprite;
    [SerializeField] protected int itemMaxCount;
    public int ItemMaxCount => itemMaxCount;
    [SerializeField] protected int dropRate;
    public int DropRate => dropRate;
}

public enum itemName
{
    none = 0,
    HpSmall = 1,
    HpMedium = 2,
    HpLarge = 3,
    EnergySmall = 4,
    EnergyMedium = 5,
    EnergyLarge = 6,
}

public enum itemType
{
    none = 0,
    hpItem = 1,
    energyItem = 2,
}
