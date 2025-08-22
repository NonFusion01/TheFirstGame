using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSkillIndexText : CoreMonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected CharSkillSelection charSkillSelection;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadText();
        this.LoadCharSkillSelection();
    }

    protected virtual void LoadText()
    {
        if (this.text != null) return;
        this.text = this.GetComponent<TextMeshProUGUI>();
        Debug.LogWarning(transform.name + ": Load Text", gameObject);
    }

    protected virtual void LoadCharSkillSelection()
    {
        if (this.charSkillSelection != null) return;
        this.charSkillSelection = GameObject.Find("Player").GetComponentInChildren<CharSkillSelection>();
        Debug.LogWarning(transform.name + ": Load charSkillSelection", gameObject);
    }

    protected virtual void Update()
    {
        this.UpdateText();
    }
    
    protected virtual void UpdateText()
    {
        this.text.text = this.charSkillSelection.index.ToString();
    }
}
