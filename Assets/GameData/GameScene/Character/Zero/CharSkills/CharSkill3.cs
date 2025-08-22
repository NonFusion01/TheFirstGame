using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSkill3 : CharBaseSkill
{
    [Header("Skill 3 Description")]
    [SerializeField] protected Transform skillCastLocation;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadInitialValue();
        this.LoadSkillCastLocation();
    }

    protected virtual void LoadSkillCastLocation()
    {
        if (this.skillCastLocation != null) return;
        this.skillCastLocation = transform.Find("SkillCastLocation");
        Debug.LogWarning(transform.name + ": Load Skill Cast Location", gameObject);
    }


    protected virtual void LoadInitialValue()
    {
        this.energyRequired = 10;
        this.currentEnergy = this.maxEnergy;
        this.skillCooldown = 5f;
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
        if (this.currentEnergy < this.energyRequired) return;
        this.currentEnergy -= this.energyRequired;
        StartCoroutine(DeclareSkill3());
    }

    protected IEnumerator DeclareSkill3()
    {
        this.charCtrl.isActionOcurr = true;
        this.charCtrl.charSkillSelection.isUsingSkill = true;
        this.charCtrl.charAttack.isAttacking = true;
        this.charCtrl.charTakeDamage.isAbleToTakeDmg = false;
        CharManager.Instance._charRigidbody2D.velocity = Vector3.zero;
        CharManager.Instance._charRigidbody2D.gravityScale = 0;
        this.charCtrl.charAniCtrl.NonLoopAnimationPlay("Skill3Animation");
        StartCoroutine(SpawnSkillRange());
        yield return new WaitUntil(() => !this.charCtrl.charAniCtrl.isNonLoopAnimation);
        CharManager.Instance._charRigidbody2D.gravityScale = this.charCtrl.basicGravityScale;
        this.charCtrl.charTakeDamage.isAbleToTakeDmg = true;
        this.charCtrl.charAttack.isAttacking = false;
        this.charCtrl.charSkillSelection.isUsingSkill = false;
        this.charCtrl.isActionOcurr = false;
        this.timeRemaining = this.skillCooldown;
    }

    protected IEnumerator SpawnSkillRange()
    {
        yield return new WaitForSeconds(1.2f);
        Transform newSkill = BulletSpawner.Instance.Spawn("Skill3Range", this.skillCastLocation.position, Quaternion.identity);
        newSkill.gameObject.SetActive(true);
        BulletCtrl bulletCtrl = newSkill.GetComponent<BulletCtrl>();
        bulletCtrl.SetOwner(this.transform.parent.parent);
        bulletCtrl.damage = 10;
        bulletCtrl.isImpactable = false;
        bulletCtrl.remainingTime = 5.1f;
    }
}
