using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCtrl : CoreMonoBehaviour
{
    [SerializeField] protected CharSkillSelection charSkillSelection;
    [SerializeField] protected Inventory inventory;
    [SerializeField] protected List<Transform> menuList;
    [SerializeField] protected int index = 0;
    protected Image currentIcon;

    //Menu UI
    [SerializeField] protected HPBarUI hPBarUI;
    [SerializeField] protected List<EnergyBarUI> skillEnergyList;
    [SerializeField] protected List<HPStorageUI> hPStorageUIList;
    [SerializeField] protected List<EnergyStorageUI> energyStorageUIList;
    protected Color startColor = Color.white;
    protected Color endColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    float transitionTime = 1f;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharSkillSelection();
        this.LoadInventory();
        this.LoadMenu();
        this.LoadUI();
    }

    protected virtual void LoadCharSkillSelection()
    {
        if (this.charSkillSelection != null) return;
        this.charSkillSelection = GameObject.Find("Player").GetComponentInChildren<CharSkillSelection>();
        Debug.LogWarning(transform.name + ": Load Char Skill Selection", gameObject);
    }

    protected virtual void LoadInventory()
    {
        if (this.inventory != null) return;
        this.inventory = GameObject.Find("Player").GetComponentInChildren<Inventory>();
        Debug.LogWarning(transform.name + ": Load Inventory", gameObject);
    }

    protected virtual void LoadMenu()
    {
        if (this.menuList.Count > 0) return;
        foreach (Transform transform in this.transform)
        {
            if (transform.name == "HP") continue;
            this.menuList.Add(transform);
        }
        Debug.LogWarning(transform.name + ": Load Menu", gameObject);
    }

    protected virtual void LoadUI()
    {
        if (this.hPStorageUIList.Count > 0 && this.energyStorageUIList.Count > 0) return;
        foreach(Transform transform in this.transform)
        {
            HPBarUI hPBarUI = transform.GetComponent<HPBarUI>();
            if (hPBarUI != null) this.hPBarUI = hPBarUI;
            EnergyBarUI energyBarUI = transform.GetComponent<EnergyBarUI>();
            if (energyBarUI != null) this.skillEnergyList.Add(energyBarUI);
            HPStorageUI hpUI = transform.GetComponent<HPStorageUI>();
            if (hpUI != null) this.hPStorageUIList.Add(hpUI);
            EnergyStorageUI energyStorageUI = transform.GetComponent<EnergyStorageUI>();
            if (energyStorageUI != null) this.energyStorageUIList.Add(energyStorageUI);
        }
        Debug.LogWarning(transform.name + ": Load UI", gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.SetInitialValue();
    }

    protected virtual void SetInitialValue()
    {
        this.ResetMenuUI(this.index);
        this.index = 0;
        this.currentIcon = this.menuList[this.index].Find("Icon").GetComponent<Image>();
    }


    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.ResetMenuUI(this.index);
            this.index++;
            if (this.index >= this.menuList.Count) this.index = 0;
            this.currentIcon = this.menuList[this.index].Find("Icon").GetComponent<Image>();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.ResetMenuUI(this.index);
            this.index--;
            if (this.index < 0) this.index = this.menuList.Count - 1;
            this.currentIcon = this.menuList[this.index].Find("Icon").GetComponent<Image>();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            this.SelectMenu(this.index);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.transform.gameObject.SetActive(false);
            GameManagerScript.isGamePaused = false;
        }
        this.UpdateMenuUI(this.currentIcon);
    }

    protected virtual void SelectMenu(int index)
    {
        int count1 = this.charSkillSelection.SkillList.Count; //4
        int count2 = this.inventory.hPStorages.Count;//2
        int count3 = this.inventory.energyStorages.Count;//1
        if ((this.index >= 0) && (this.index < count1)) // 1 2 3
        {
            Debug.Log("Select Skill: " + this.index);
            this.charSkillSelection.index = this.index;
        }
        if ((this.index >= count1) && (this.index < count1 + count2)) // 4 5
        {
            Debug.Log("Use HP Storage: " + (this.index - count1 + 1));
            this.UseHPFromStorage(this.index - count1);
        }
        if ((this.index >= count1 + count2) && (this.index < count1 + count2 + count3)) //6
        {
            Debug.Log("Use Energy Storage: " + (this.index - count1 - count2 + 1));
            this.UseEnergyFromStorage(this.index - count1 - count2);
        }
    }

    protected virtual void UseHPFromStorage(int index)
    {
        int hpRequired = CharManager.Instance._charStats.maxHP - CharManager.Instance._charStats.currentHP;
        if (this.inventory.hPStorages[index].currentHPStorage >= hpRequired)
        {
            this.inventory.hPStorages[index].currentHPStorage -= hpRequired;
            CharManager.Instance._charStats.currentHP = CharManager.Instance._charStats.maxHP;
        }
        else
        {
            CharManager.Instance._charStats.currentHP += this.inventory.hPStorages[index].currentHPStorage;
            this.inventory.hPStorages[index].currentHPStorage = 0;
        }
        this.hPStorageUIList[index].UpdateHP();
        this.hPBarUI.UpdateHP();
    }

    protected virtual void UseEnergyFromStorage(int index)
    {
        int energyRequired = 0;
        int notMaxEnergySkills = 0;
        foreach (CharBaseSkill skill in this.charSkillSelection.SkillList)
        {
            if (skill.currentEnergy < skill.MaxEnergy)
            {
                notMaxEnergySkills++;
                energyRequired += skill.MaxEnergy - skill.currentEnergy;
            }
        }
        if (energyRequired == 0 || this.inventory.energyStorages[index].currentEnergyStorage == 0) return;

        //distribute energy for skills that are not at max energy
        foreach (CharBaseSkill skill in this.charSkillSelection.SkillList)
        {
            if (skill.currentEnergy >= skill.MaxEnergy) continue;
            skill.currentEnergy += this.inventory.energyStorages[index].currentEnergyStorage / notMaxEnergySkills;
            Debug.Log("Skill: " + skill.name + " Receive Energy: " + this.inventory.energyStorages[index].currentEnergyStorage / notMaxEnergySkills);
        }
        this.inventory.energyStorages[index].currentEnergyStorage = 0;

        // if any skill has more energy than max, add back excess energy to storage
        foreach (CharBaseSkill skill in this.charSkillSelection.SkillList)
        {
            if (skill.currentEnergy <= skill.MaxEnergy) continue;
            this.inventory.energyStorages[index].currentEnergyStorage += skill.currentEnergy - skill.MaxEnergy;
            skill.currentEnergy = skill.MaxEnergy;
        }
        this.UseEnergyFromStorage(index);
        this.energyStorageUIList[index].UpdateEnergy();
        foreach (EnergyBarUI energyBarUI in this.skillEnergyList)
        {
            energyBarUI.UpdateEnergyBar();
        }
    }

    protected virtual void UpdateMenuUI(Image icon)
    {
        if (icon == null) return;
        float alpha = Mathf.PingPong(Time.unscaledTime / transitionTime, 1f);
        icon.color = Color.Lerp(startColor, endColor, alpha);
    }

    protected virtual void ResetMenuUI(int index)
    {
        this.currentIcon = this.menuList[index].Find("Icon").GetComponent<Image>();
        currentIcon.color = startColor;
    }
}
