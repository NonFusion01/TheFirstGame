using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSkillSelection: CoreMonoBehaviour
{
    [SerializeField] protected CharController charCtrl;
    [SerializeField] public int index = 0;
    [SerializeField] protected List<CharBaseSkill> skillList;
    public List<CharBaseSkill> SkillList => skillList;
    [SerializeField] public bool isUsingSkill = false;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharCtrl();
        this.LoadSkills();
    }

    protected virtual void LoadCharCtrl()
    {
        if (this.charCtrl != null) return;
        this.charCtrl = transform.parent.GetComponent<CharController>();
        Debug.LogWarning(transform.name + ": Load Char Ctrl", gameObject);
    }

    protected virtual void LoadSkills()
    {
        if (this.skillList.Count > 0) return;
        foreach (Transform transform in this.transform)
        {
            CharBaseSkill skill = transform.GetComponent<CharBaseSkill>();
            this.skillList.Add(skill);
            Debug.LogWarning(transform.name + ": Load Skill", gameObject);
        }
    }

    protected virtual void Update()
    {
        if (GameManagerScript.isGamePaused) return;
        if (this.charCtrl.isDisableController) return;

        this.SkillSelection();
        this.DeclareSkill();
    }

    protected virtual void SkillSelection()
    {
        if (this.isUsingSkill) return;
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (this.index == 0) this.index = this.skillList.Count - 1;
            else this.index--;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (this.index == this.skillList.Count - 1) this.index = 0;
            else this.index++;
        }
    }

    protected virtual void DeclareSkill()
    {
        if (this.index == 0) return;
        if (this.isUsingSkill) return;
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (this.charCtrl.charAttack.isAttacking) return;
            if (this.charCtrl.charTakeDamage.isTakingDmg) return;
            if (this.skillList[2].isInSkillDuration) this.skillList[2].CancelSkill();
            else this.skillList[index].CastSkill();
        }
    }
}
