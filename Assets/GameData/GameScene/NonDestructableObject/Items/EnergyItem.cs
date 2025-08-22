using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyItem : Item
{
    [SerializeField] protected Inventory inventory;
    [SerializeField] protected CharSkillSelection charSkillSelection;
    [SerializeField] protected int energyGainAmount;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadInventory();
        this.LoadCharSkillSelection();
    }

    protected virtual void LoadInventory()
    {
        if (this.inventory != null) return;
        this.inventory = GameObject.Find("Player").GetComponentInChildren<Inventory>();
        Debug.LogWarning(transform.name + ": Load Inventory", gameObject);
    }

    protected virtual void LoadCharSkillSelection()
    {
        if (this.charSkillSelection != null) return;
        this.charSkillSelection = GameObject.Find("Player").GetComponentInChildren<CharSkillSelection>();
        Debug.LogWarning(transform.name + ": Load SKill Selection", gameObject);
    }

    protected virtual void RestoreEnergyForChosenSkill(int i)
    {
        if (i == 0) return;
        this.charSkillSelection.SkillList[i].currentEnergy += energyGainAmount;
        if (this.charSkillSelection.SkillList[i].currentEnergy > this.charSkillSelection.SkillList[i].MaxEnergy)
        {
            int remainAmount = this.charSkillSelection.SkillList[i].currentEnergy - this.charSkillSelection.SkillList[i].MaxEnergy;
            this.charSkillSelection.SkillList[i].currentEnergy = this.charSkillSelection.SkillList[i].MaxEnergy;
            this.RestoreEnergyForOtherSkills(remainAmount);
        }
    }

    protected virtual void RestoreEnergyForOtherSkills(int remainAmount)
    {
        if (remainAmount == 0) return; //no energy left to distribute
        int skillNotAtMaxEnergy = 0;
        foreach (CharBaseSkill skill in this.charSkillSelection.SkillList)
        {
            if (skill.currentEnergy < skill.MaxEnergy)
            {
                skillNotAtMaxEnergy++;
            }
        }

        if (skillNotAtMaxEnergy == 0)
        {
            // add remainAmount to EnergyStorage
            this.inventory.energyStorages[0].currentEnergyStorage += remainAmount;
            if (this.inventory.energyStorages[0].currentEnergyStorage > this.inventory.energyStorages[0].MaxEnergyStorage)
            {
                this.inventory.energyStorages[0].currentEnergyStorage = this.inventory.energyStorages[0].MaxEnergyStorage;
            }
        }
        else
        {
            int remainAmount2 = 0;
            foreach (CharBaseSkill skill in this.charSkillSelection.SkillList)
            {
                if (skill.currentEnergy < skill.MaxEnergy)
                {
                    skill.currentEnergy += remainAmount / skillNotAtMaxEnergy;
                    if (skill.currentEnergy > skill.MaxEnergy) 
                    {
                        remainAmount2 += skill.currentEnergy - skill.MaxEnergy;
                        skill.currentEnergy = skill.MaxEnergy;
                    }
                }
            }
            this.RestoreEnergyForOtherSkills(remainAmount2);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        CharController charCtrl = other.transform.GetComponent<CharController>();
        if (charCtrl == null) return;
        if (charCtrl.charSkillSelection.index == 0)
        {
            Debug.Log("Distribute: " + this.energyGainAmount + " Energy for other skills");
            this.RestoreEnergyForOtherSkills(this.energyGainAmount);
        }
        else
        {
            Debug.Log("Restore: " + this.energyGainAmount + " Energy for chosen skill");
            this.RestoreEnergyForChosenSkill(charCtrl.charSkillSelection.index);
        }
        ItemDropSpawner.Instance.ReturnObjectToPool(this.transform);
    }
}
