using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyBar : CoreMonoBehaviour
{
    [SerializeField] protected Image energyBar;
    [SerializeField] protected CharSkillSelection charSkillSelection;
    [SerializeField] protected List<Image> energyUI;
    protected int index;
    protected int maxEnergy;
    protected int newEnergy;
    protected int oldEnergy = 0;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadEnergyBar();
        this.LoadEnergyUI();
        this.LoadCharSkillSelection();
    }

    protected virtual void LoadEnergyBar()
    {
        if (this.energyBar != null) return;
        this.energyBar = transform.Find("Energy").GetComponent<Image>();
        Debug.LogWarning(transform.name + ": Load Energy Bar", gameObject);
    }

    protected virtual void LoadEnergyUI()
    {
        if (this.energyUI.Count > 0) return;
        foreach (Transform transform in this.transform)
        {
            if (transform.name == "SkillIcon") continue;
            Image image = transform.GetComponent<Image>();
            this.energyUI.Add(image);
        }
        Debug.LogWarning(transform.name + ": Load Energy UI", gameObject);
    }

    protected virtual void LoadCharSkillSelection()
    {
        if (this.charSkillSelection != null) return;
        this.charSkillSelection = GameObject.Find("Player").GetComponentInChildren<CharSkillSelection>();
        Debug.LogWarning(transform.name + ": Load Char Skill Selection", gameObject);
    }

    protected virtual void FixedUpdate()
    {
        this.UpdateEnergyBar();
    }

    protected virtual void UpdateEnergyBar()
    {
        this.index = this.charSkillSelection.index;
        this.maxEnergy = this.charSkillSelection.SkillList[this.index].MaxEnergy;
        this.newEnergy = this.charSkillSelection.SkillList[this.index].currentEnergy;
        if (this.maxEnergy == 0)
        {
            this.HideEnergyBar();
            return;
        }
        else this.ShowEnergyBar();

        if (this.newEnergy != this.oldEnergy)
        {
            this.energyBar.fillAmount = (float)this.newEnergy / this.maxEnergy;
            this.oldEnergy = this.newEnergy;
        }
    }

    protected virtual void HideEnergyBar()
    {
        foreach (Image image in this.energyUI)
        {
            image.enabled = false;
        }
    }

    protected virtual void ShowEnergyBar()
    {
        foreach (Image image in this.energyUI)
        {
            image.enabled = true;
        }
    }
}
