using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharObtain : CoreMonoBehaviour
{
    [SerializeField] protected Inventory inventory;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadInventory();
    }

    protected virtual void LoadInventory()
    {
        if (this.inventory != null) return;
        this.inventory = transform.parent.GetComponentInChildren<Inventory>();
        Debug.LogWarning(transform.name + ": Load Inventory", gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        Item item = other.GetComponent<Item>();
        if (item == null) return;
        this.AddItemToInventory(item);
        Debug.Log("Player Pick Item");
        //remove item on scene
    }

    protected virtual void AddItemToInventory(Item item)
    {
        ItemInventory newItem = new ItemInventory();
        newItem.itemSO = item.ItemSO;
        newItem.itemName = item.name;
        newItem.itemCount = item.itemCount;
        newItem.itemMaxCount = item.ItemSO.ItemMaxCount;
        newItem.dropRate = item.DropRate;
        this.inventory.AddItem(newItem, newItem.itemCount);
    }
}
