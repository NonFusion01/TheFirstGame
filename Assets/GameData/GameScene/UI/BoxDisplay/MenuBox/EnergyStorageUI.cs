using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyStorageUI : CoreMonoBehaviour
{
    [SerializeField] protected Image energyBar;
    [SerializeField] protected Inventory inventory;


    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadEnergyBar();
        this.LoadInventory();
    }

    protected virtual void LoadEnergyBar()
    {
        if (this.energyBar != null) return;
        this.energyBar = transform.Find("EnergyBar").GetComponent<Image>();
        Debug.LogWarning(transform.name + ": Load Energy Bar", gameObject);
    }

    protected virtual void LoadInventory()
    {
        if (this.inventory != null) return;
        this.inventory = GameObject.Find("Player").GetComponentInChildren<Inventory>();
        Debug.LogWarning(transform.name + ": Load Inventory", gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.UpdateEnergy();
    }

    public virtual void UpdateEnergy()
    {
        int maxEnergy = this.inventory.energyStorages[0].MaxEnergyStorage;
        int currentEnergy = this.inventory.energyStorages[0].currentEnergyStorage;
        this.energyBar.fillAmount = (float)currentEnergy / maxEnergy;
    }
}
