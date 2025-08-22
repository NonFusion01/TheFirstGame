using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPStorageUI : CoreMonoBehaviour
{
    [SerializeField] protected Image hPBar;
    [SerializeField] protected Inventory inventory;
    [SerializeField] protected int index;


    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadEnergyBar();
        this.LoadInventory();
    }

    protected virtual void LoadEnergyBar()
    {
        if (this.hPBar != null) return;
        this.hPBar = transform.Find("HPBar").GetComponent<Image>();
        Debug.LogWarning(transform.name + ": Load HP Bar", gameObject);
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
        this.UpdateHP();
    }

    public virtual void UpdateHP()
    {
        int maxHP = this.inventory.hPStorages[index].maxHPStorage;
        int currentHP = this.inventory.hPStorages[index].currentHPStorage;
        this.hPBar.fillAmount = (float)currentHP / maxHP;
    }
}
