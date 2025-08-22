using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSkill2 : CharBaseSkill
{
    [Header("Skill 2 Description")]
    [SerializeField] protected int energyConsumedPerSecond = 10;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadInitialValue();
    }

    protected virtual void LoadInitialValue()
    {
        this.energyRequired = 50;
        this.currentEnergy = this.maxEnergy;
        this.skillCooldown = 1f;
    }

    protected virtual void FixedUpdate()
    {
        this.timeRemaining -= Time.fixedDeltaTime;
        if (this.timeRemaining < 0)
        {
            this.timeRemaining = 0;
            this.isOnCooldown = false;
        }
        else this.isOnCooldown = true;
    }

    public override void CastSkill()
    {
        if (this.isOnCooldown) return;
        if (this.isInSkillDuration) return;
        if (this.currentEnergy < this.energyRequired) return;
        this.charCtrl.charSkillSelection.isUsingSkill = true;
        StartCoroutine(UsingSkill2());
        StartCoroutine(DecreaseEnergy());
    }

    protected IEnumerator UsingSkill2()
    {
        yield return new WaitForSeconds(0.1f); // wait for the skill to be ready
        this.isInSkillDuration = true;
        this.charCtrl.charTakeDamage.isAbleToTakeDmg = false;
        StartCoroutine(TransitionColor(Color.white, Color.black));
    }

    protected IEnumerator TransitionColor(Color start, Color target)
    {
        float durationTime = 0f;
        float elapsedTime = 0.25f;
        while (durationTime < elapsedTime)
        {
            durationTime += Time.deltaTime;
            CharManager.Instance._charSpriteRenderer.color = Color.Lerp(start, target, durationTime / elapsedTime);
            yield return null;
        }
        CharManager.Instance._charSpriteRenderer.color = target;
        this.charCtrl.charSkillSelection.isUsingSkill = false;

    }

    protected IEnumerator DecreaseEnergy()
    {
        yield return new WaitForSeconds(1f);
        this.currentEnergy -= this.energyConsumedPerSecond;
        if (this.currentEnergy < 0) this.currentEnergy = 0;
        if (this.currentEnergy == 0) this.CancelSkill();
        if (this.isInSkillDuration && this.currentEnergy > 0) StartCoroutine(DecreaseEnergy());
    }

    public override void CancelSkill()
    {
        if (!this.isInSkillDuration) return;
        Debug.Log("Cancel Skill 2");
        this.isInSkillDuration = false;
        this.charCtrl.charTakeDamage.isAbleToTakeDmg = true;
        StartCoroutine(TransitionColor(Color.black, Color.white));
    }
}
