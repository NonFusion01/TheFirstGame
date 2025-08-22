using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSkill1 : CharBaseSkill
{
    [Header("Skill 1 Description")]
    
    [SerializeField] protected Transform skillCastLocation;
    [SerializeField] protected float bulletSpeed = 10f;
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
        this.energyRequired = 20;
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
        if (!this.charCtrl.isOnGround) return;
        if (this.currentEnergy < this.energyRequired) return;
        this.currentEnergy -= this.energyRequired;
        StartCoroutine(DeclareSkill1());
    }

    protected IEnumerator DeclareSkill1()
    {
        this.charCtrl.isActionOcurr = true;
        this.charCtrl.charSkillSelection.isUsingSkill = true;
        this.charCtrl.charAttack.isAttacking = true;
        this.charCtrl.charTakeDamage.isAbleToTakeDmg = false;
        CharManager.Instance._charSpriteRenderer.color = Color.red;
        this.charCtrl.charAniCtrl.NonLoopAnimationPlay("Skill1Animation");
        StartCoroutine(SpawnSkillRange());
        yield return new WaitUntil(() => !this.charCtrl.charAniCtrl.isNonLoopAnimation);
        CharManager.Instance._charSpriteRenderer.color = Color.white;
        this.charCtrl.charTakeDamage.isAbleToTakeDmg = true;
        this.charCtrl.charAttack.isAttacking = false;
        this.charCtrl.charSkillSelection.isUsingSkill = false;
        this.charCtrl.isActionOcurr = false;
        this.timeRemaining = this.skillCooldown;
    }

    protected IEnumerator SpawnSkillRange()
    {
        yield return new WaitForSeconds(0.9f);
        Transform newBullet = BulletSpawner.Instance.Spawn("Skill1Range", this.skillCastLocation.position, Quaternion.identity);
        newBullet.gameObject.SetActive(true);
        newBullet.localScale = Vector3.one * this.charCtrl.charMovement.moveDirection.x;
        BulletCtrl bulletCtrl = newBullet.GetComponent<BulletCtrl>();
        bulletCtrl.SetOwner(this.transform.parent.parent);
        bulletCtrl.isImpactable = false;
        bulletCtrl.damage = 50;
        bulletCtrl.bulletRigidbody2D.velocity = this.charCtrl.charMovement.moveDirection * this.bulletSpeed;
    }
}
