using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaFrogSkills : CoreMonoBehaviour
{
    [SerializeField] protected NinjaFrogSkill1 skill1;
    public NinjaFrogSkill1 Skill1 => skill1;
    [SerializeField] protected NinjaFrogSkill2 skill2;
    public NinjaFrogSkill2 Skill2 => skill2;
    [SerializeField] protected NinjaFrogSkill3 skill3;
    public NinjaFrogSkill3 Skill3 => skill3;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadSkill1();
        this.LoadSkill2();
        this.LoadSkill3();
    }

    protected virtual void LoadSkill1()
    {
        if (this.skill1 != null) return;
        this.skill1 = transform.parent.GetComponentInChildren<NinjaFrogSkill1>();
        Debug.LogWarning(transform.name + ": Load Skill 1", gameObject);
    }

    protected virtual void LoadSkill2()
    {
        if (this.skill2 != null) return;
        this.skill2 = transform.parent.GetComponentInChildren<NinjaFrogSkill2>();
        Debug.LogWarning(transform.name + ": Load Skill 2", gameObject);
    }

    protected virtual void LoadSkill3()
    {
        if (this.skill3 != null) return;
        this.skill3 = transform.parent.GetComponentInChildren<NinjaFrogSkill3>();  
        Debug.LogWarning(transform.name + ": Load Skill 3", gameObject);
    }
}
