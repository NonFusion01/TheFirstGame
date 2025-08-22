using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPItem : Item
{
    [SerializeField] protected Inventory inventory;
    [SerializeField] public int hpGainAmount;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadInventory();
    }

    protected virtual void LoadInventory()
    {
        if (this.inventory != null) return;
        this.inventory = GameObject.Find("Player").GetComponentInChildren<Inventory>();
        Debug.LogWarning(transform.name + ": Load Inventory", gameObject);
    }

    protected virtual void RestoreHP()
    {
        CharManager.Instance._charStats.currentHP += hpGainAmount;
        if (CharManager.Instance._charStats.currentHP > CharManager.Instance._charStats.maxHP)
        {
            int hpRemain = CharManager.Instance._charStats.currentHP - CharManager.Instance._charStats.maxHP;
            this.AddHPToStoragre(hpRemain);
            CharManager.Instance._charStats.currentHP = CharManager.Instance._charStats.maxHP;
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        CharController charCtrl = other.GetComponent<CharController>();
        if (charCtrl == null) return;
        this.RestoreHP();
        Debug.Log("Restore: " + this.hpGainAmount + " HP");
        ItemDropSpawner.Instance.ReturnObjectToPool(this.transform);
    }

    protected virtual void AddHPToStoragre(int amount)
    {
        this.inventory.hPStorages[0].AddHP(amount);
        if (this.inventory.hPStorages[0].currentHPStorage > this.inventory.hPStorages[0].maxHPStorage)
        {
            int hpRemain = this.inventory.hPStorages[0].currentHPStorage - this.inventory.hPStorages[0].maxHPStorage;
            this.inventory.hPStorages[0].currentHPStorage = this.inventory.hPStorages[0].maxHPStorage;
            this.inventory.hPStorages[1].AddHP(hpRemain);
            if (this.inventory.hPStorages[1].currentHPStorage > this.inventory.hPStorages[1].maxHPStorage)
            {
                this.inventory.hPStorages[1].currentHPStorage = this.inventory.hPStorages[1].maxHPStorage;
            }
        }
    }
}
