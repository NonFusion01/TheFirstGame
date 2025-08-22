using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : CoreMonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] protected int maxSlots = 10;
    [SerializeField] public List<ItemInventory> itemList;
    [SerializeField] public List<HPStorage> hPStorages;
    [SerializeField] public List<EnergyStorage> energyStorages;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadStorages();
    }

    protected virtual void LoadStorages() 
    {
        if (hPStorages.Count > 0) return;
        HPStorage hPStorage_1 = new HPStorage();
        HPStorage hPStorage_2 = new HPStorage();
        this.hPStorages.Add(hPStorage_1);
        this.hPStorages.Add(hPStorage_2);
        if (energyStorages.Count > 0) return;
        EnergyStorage energyStorage_1 = new EnergyStorage();
        this.energyStorages.Add(energyStorage_1);
    }


    protected override void Start()
    {
        base.Start();
        //this.Test();
    }

    protected virtual void Test()
    {
        this.energyStorages[0].currentEnergyStorage = 120;
    }

    public virtual void AddItem(itemName itemName, int count)
    {
        ItemInventory newItem = new ItemInventory();
        var itemSO = Resources.LoadAll<DropItemSO>("ScriptableObject/DropItems/");
        foreach (DropItemSO item in itemSO)
        {
            if (item.ItemName != itemName) continue;
            newItem.itemSO = item;
            newItem.itemMaxCount = item.ItemMaxCount;
            newItem.dropRate = item.DropRate;
        }
        newItem.itemName = itemName.ToString();
        newItem.itemCount = count;
        this.AddItem(newItem, count);
    }

    public virtual void AddItem(ItemInventory addItem, int count)
    {
        if (!this.CheckAvailableToAdd(addItem, count)) return;
        foreach (ItemInventory item in this.itemList)
        {
            if (item.itemName != addItem.itemName) continue;  
            if (item.itemCount == item.itemMaxCount) continue;
            item.itemCount += count;
            count = 0;
            if (item.itemCount > item.itemMaxCount)
            {
                count = item.itemCount - item.itemMaxCount;
                item.itemCount = item.itemMaxCount;
            }
        }
        if (count > 0) this.CreateNewItem(addItem, count);
    }

    public virtual void DeductItem(itemName itemName, int count)
    {
        ItemInventory newItem = new ItemInventory();
        var itemSO = Resources.LoadAll<DropItemSO>("ScriptableObject/DropItems/");
        foreach (DropItemSO item in itemSO)
        {
            if (item.ItemName != itemName) continue;
            newItem.itemSO = item;
            newItem.itemMaxCount = item.ItemMaxCount;
            newItem.dropRate = item.DropRate;
        }
        newItem.itemName = itemName.ToString();
        newItem.itemCount = count;
        this.DeductItem(newItem, count);
    }

    public virtual void DeductItem(ItemInventory deductItem, int count)
    {
        if (!this.CheckAvailableToDeduct(deductItem, count)) return;
        foreach(ItemInventory item in this.itemList) 
        {
            if (item.itemName != deductItem.itemName) continue;
            item.itemCount -= count;
            count = 0;
            if (item.itemCount < 0)
            {
                count = - item.itemCount;
                item.itemCount = 0;
            }
        }
        this.itemList.RemoveAll(item => item.itemCount == 0);
    }

    protected virtual bool CheckAvailableToAdd(ItemInventory addItem, int count)
    {
        int totalEmptySlots = 0;
        int emptySlotsinInventory = this.maxSlots - this.itemList.Count;
        totalEmptySlots += emptySlotsinInventory * addItem.itemMaxCount;

        bool isItemExisted = false;
        foreach (ItemInventory item in this.itemList)
        {
            if (item.itemName != addItem.itemName) continue;
            isItemExisted = true;
            totalEmptySlots += item.itemMaxCount - item.itemCount;
        }
        if (totalEmptySlots >= count) return true;
        else
        {
            if (totalEmptySlots == 0 && isItemExisted) Debug.Log("Item Full Slots. Cannot Add");
            if (totalEmptySlots == 0 && !isItemExisted) Debug.Log("No Item Found");
            if (totalEmptySlots > 0) Debug.Log("Not Enough Space");
            return false;
        }
    }

    protected virtual bool CheckAvailableToDeduct(ItemInventory deductItem, int count)
    {
        int existingItemCount = 0;
        foreach(ItemInventory item in this.itemList)
        {
            if (item.itemName != deductItem.itemName) continue;
            existingItemCount += item.itemCount;
        }
        if (existingItemCount >= count) return true;
        else
        {
            Debug.Log("Not Enough Item");
            return false;
        }
    }

    protected virtual void CreateNewItem(ItemInventory item, int count)
    {
        ItemInventory newItem = new ItemInventory();
        newItem.itemSO = item.itemSO;
        newItem.itemName = item.itemName;
        newItem.itemCount = 0;
        newItem.itemMaxCount = item.itemMaxCount;
        newItem.dropRate = item.dropRate;
        this.itemList.Add(newItem);
        this.AddItem(newItem, count);
    }

    protected virtual void AttachItemsInInventory()
    {

    }

    protected virtual void DetachItemsInInventory(int count)
    {

    }
}
