using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIconUI : CoreMonoBehaviour
{
    [SerializeField] protected CharSkillSelection charSkillSelection;
    [SerializeField] protected Image icon;
    protected int index;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharSkillSelection();
        this.LoadIcon();
    }

    protected virtual void LoadCharSkillSelection()
    {
        if (this.charSkillSelection != null) return;
        this.charSkillSelection = GameObject.Find("Player").GetComponentInChildren<CharSkillSelection>();
        Debug.LogWarning(transform.name + ": Load Char Skill Selection", gameObject);
    }
    protected virtual void LoadIcon()
    {
        if (this.icon != null) return;
        this.icon = transform.Find("Icon").GetComponent<Image>();
        Debug.LogWarning(transform.name + ": Load Icon", gameObject);
    }

    protected virtual void FixedUpdate()
    {
        this.UpdateIcons();
    }

    protected virtual void UpdateIcons()
    {
        this.index = this.charSkillSelection.index;
        if (this.index != 0)
        {
            this.icon.sprite = this.charSkillSelection.SkillList[this.index].Icon.sprite;
        }
        if (this.index == 0)
        {
            this.icon.sprite = CharManager.Instance._charIcon.sprite;
        }
    }
}
