using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarUI : CoreMonoBehaviour
{
    
    [SerializeField] protected CharSkillSelection charSkillSelection;
    [SerializeField] protected Image energyBar;
    [SerializeField] protected Image icon;
    [SerializeField] protected List<Image> energyBarUI;
    [SerializeField] protected string skillName;
    [SerializeField] protected int index;
    protected int maxEnergy;
    protected int currentEnergy;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharSkillSelection();
        this.LoadIcon();
        this.LoadEnergyBar();
        this.LoadEnergyBarUI();
    }

    protected virtual void LoadCharSkillSelection()
    {
        if (this.charSkillSelection != null) return;
        this.charSkillSelection = GameObject.Find("Player").GetComponentInChildren<CharSkillSelection>();
        Debug.LogWarning(transform.name + ": Load Skill", gameObject);
    }

    protected virtual void LoadEnergyBar()
    {
        if (this.energyBar != null) return;
        this.energyBar = transform.Find("EnergyBar").GetComponent<Image>();
        Debug.LogWarning(transform.name + ": Load Energy Bar", gameObject);
    }

    protected virtual void LoadIcon()
    {
        if (this.icon != null) return;
        this.icon = transform.Find("Icon").GetComponent<Image>();
        Debug.LogWarning(transform.name + ": Load Icon", gameObject);
        this.index = this.transform.GetSiblingIndex() - 1;
        this.icon.sprite = this.charSkillSelection.SkillList[this.index].Icon.sprite;
    }

    protected virtual void LoadEnergyBarUI()
    {
        if (this.energyBarUI.Count > 0) return;
        Image BG_1 = transform.Find("BG_1").GetComponent<Image>();
        Image BG_2 = transform.Find("BG_2").GetComponent<Image>();
        this.energyBarUI.Add(BG_1);
        this.energyBarUI.Add(BG_2);
        this.energyBarUI.Add(energyBar);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.UpdateEnergyBar();
    }

    protected virtual void FixedUpdate()
    {
        this.UpdateEnergyBar();
    }

    public virtual void UpdateEnergyBar()
    {
        this.maxEnergy = this.charSkillSelection.SkillList[this.index].MaxEnergy;
        if (this.maxEnergy == 0) this.HideEnergyBar();
        else this.ShowEnergyBar();  
        this.currentEnergy = this.charSkillSelection.SkillList[this.index].currentEnergy;
        this.energyBar.fillAmount = (float)this.currentEnergy / this.maxEnergy;
    }

    protected virtual void HideEnergyBar()
    {
        foreach (Image image in this.energyBarUI)
        {
            image.enabled = false;
        }
    }

    protected virtual void ShowEnergyBar()
    {
        foreach (Image image in this.energyBarUI)
        {
            image.enabled = true;
        }
    }
}
