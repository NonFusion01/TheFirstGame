using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemInventory 
{
    public DropItemSO itemSO;
    public string itemName;
    public int itemCount;
    public int itemMaxCount;
    public float dropRate;

    public virtual ItemInventory Clone()
    {
        ItemInventory newItem = new ItemInventory();
        newItem.itemSO = this.itemSO;
        newItem.itemCount = this.itemCount;
        newItem.itemMaxCount = this.itemMaxCount;
        newItem.dropRate = this.dropRate;
        return newItem;
    }
}
