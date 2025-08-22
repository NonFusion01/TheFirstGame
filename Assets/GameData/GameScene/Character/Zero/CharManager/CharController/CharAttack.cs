using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CharAttack : CoreMonoBehaviour
{
    [SerializeField] protected CharController charCtrl;
    //attack
    [Header("Attack")]
    public bool isDeclaredAtk = false;
    public bool isAttacking = false;
    //melee atk
    [Header("Melee Attack")]
    public float atkSeqTime = 0;
    public float inputAtkTime = 1f;
    protected int lastAtkSeq = 0;
    protected int currentAtkSeq = 0;
    protected int atkSeqLimit = 3;
    protected bool canEndAtkTrigger = false;
    //ranged atk
    [Header("Ranged Attack")]
    public Transform shootPoint;
    protected float rangedAtkCooldown = 0.5f;
    protected float rangedAtkTime = 0f;
    public float bulletSpeed = 10f;
    public bool isChargedAtk = false;
    public float chargedAtkTimeRequired = 0.5f;
    public float chargedAtkTime;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharCtrl();
        this.LoadShotPoint();
    }

    protected virtual void LoadCharCtrl()
    {
        if (this.charCtrl != null) return;
        this.charCtrl = transform.parent.GetComponentInChildren<CharController>();
        Debug.LogWarning(transform.name + ": Load Char Ctrl", gameObject);
    }

    protected virtual void LoadShotPoint()
    {
        if (this.shootPoint != null) return;
        this.shootPoint = transform.parent.Find("PlayerRangedAtk").Find("ShootPoint");
        Debug.LogWarning("Load Shoot Point");
    }

    protected virtual void Update()
    {
        if (GameManagerScript.isGamePaused) return;
        if (this.charCtrl.isDisableController) return;
        this.CharMeleeAtk();
        this.CharRangedAtk();
    }

    protected virtual void CharMeleeAtk()
    {
        this.atkSeqTime -= Time.deltaTime;
        if (this.atkSeqTime < 0) this.atkSeqTime = 0;
        if (this.charCtrl.charTakeDamage.isTakingDmg) return;
        if (this.isAttacking) return;
        if (Input.GetKeyDown(KeyCode.C))
        {
            this.isDeclaredAtk = true;
            this.charCtrl.isActionOcurr = true;
            CharManager.Instance._charRigidbody2D.velocity = Vector3.zero;
            CharManager.Instance._charRigidbody2D.gravityScale = 0;
            if (this.atkSeqTime <= 0)
            {
                this.currentAtkSeq = 1;
            }
            if (this.atkSeqTime > 0 && this.atkSeqTime < this.inputAtkTime)
            {
                this.currentAtkSeq++;
                if (this.currentAtkSeq > this.atkSeqLimit) this.currentAtkSeq = 1;
            }
            StartCoroutine(DeclareMeleeAttack(this.currentAtkSeq));
        }
    }

    protected IEnumerator DeclareMeleeAttack(int atkSeq)
    {
        this.isAttacking = true;
        if (this.charCtrl.isOnGround)
        {
            if (atkSeq == 1)
            {
                CharManager.Instance._charMeleeAtk.atk1Range._collider2D.enabled = true;
                CharManager.Instance._charMeleeAtk.atk1Range.SetOwner(this.transform.parent);
            }
            if (atkSeq == 2)
            {
                CharManager.Instance._charMeleeAtk.atk2Range._collider2D.enabled = true;
                CharManager.Instance._charMeleeAtk.atk2Range.SetOwner(this.transform.parent);

            }
            if (atkSeq == 3)
            {
                CharManager.Instance._charMeleeAtk.atk3Range._collider2D.enabled = true;
                CharManager.Instance._charMeleeAtk.atk3Range.SetOwner(this.transform.parent);
            }
        }
        if (!this.charCtrl.isOnGround)
        {
            if (atkSeq == 1)
            {
                CharManager.Instance._charMeleeAtk.airAtk1Range._collider2D.enabled = true;
                CharManager.Instance._charMeleeAtk.airAtk1Range.damage = 10;
                CharManager.Instance._charMeleeAtk.airAtk1Range.SetOwner(this.transform.parent);
            }
            if (atkSeq == 2)
            {
                CharManager.Instance._charMeleeAtk.airAtk2Range._collider2D.enabled = true;
                CharManager.Instance._charMeleeAtk.airAtk2Range.damage = 15;
                CharManager.Instance._charMeleeAtk.airAtk2Range.SetOwner(this.transform.parent);

            }
            if (atkSeq == 3)
            {
                CharManager.Instance._charMeleeAtk.airAtk3Range._collider2D.enabled = true;
                CharManager.Instance._charMeleeAtk.airAtk3Range.damage = 20;
                CharManager.Instance._charMeleeAtk.airAtk3Range.SetOwner(this.transform.parent);
            }
        }

        if (this.charCtrl.isOnGround) this.charCtrl.charAniCtrl.NonLoopAnimationPlay("Atk" + atkSeq + "Animation");
        if (!this.charCtrl.isOnGround) this.charCtrl.charAniCtrl.NonLoopAnimationPlay("AirAtk" + atkSeq + "Animation");
        yield return new WaitForSeconds(0.3f);
        CharManager.Instance._charRigidbody2D.gravityScale = this.charCtrl.basicGravityScale;
        if (atkSeq == 1)
        {
            CharManager.Instance._charMeleeAtk.atk1Range._collider2D.enabled = false;
            CharManager.Instance._charMeleeAtk.airAtk1Range._collider2D.enabled = false;
        }
        if (atkSeq == 2)
        {
            CharManager.Instance._charMeleeAtk.atk2Range._collider2D.enabled = false;
            CharManager.Instance._charMeleeAtk.airAtk2Range._collider2D.enabled = false;
        }
        if (atkSeq == 3)
        {
            CharManager.Instance._charMeleeAtk.atk3Range._collider2D.enabled = false;
            CharManager.Instance._charMeleeAtk.airAtk3Range._collider2D.enabled = false;
        }
        this.atkSeqTime = this.inputAtkTime;
        this.isAttacking = false;
        this.charCtrl.isActionOcurr = false;
        if (this.charCtrl.isOnGround)
        {
            this.charCtrl.canReturnToIdleState = false;
            StartCoroutine(WaitForAction(this.inputAtkTime));
        }
    }

    protected virtual void CharRangedAtk()
    {
        this.rangedAtkTime -= Time.deltaTime;
        if (this.rangedAtkTime < 0) this.rangedAtkTime = 0;
        if (this.charCtrl.charTakeDamage.isTakingDmg) return;

        // if skill is chosen, ignore
        if (this.charCtrl.charSkillSelection.index != 0) return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (this.rangedAtkTime > 0) return;
            this.CharGroundShoot();
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (this.isAttacking) return;
            this.chargedAtkTime += Time.deltaTime;
            if (this.chargedAtkTime > this.chargedAtkTimeRequired) this.chargedAtkTime = this.chargedAtkTimeRequired;
            if (this.chargedAtkTime == this.chargedAtkTimeRequired) this.isChargedAtk = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            this.chargedAtkTime = 0;
            if (this.charCtrl.charTakeDamage.isTakingDmg) return;
            if (this.isChargedAtk)
            {
                this.isChargedAtk = false;
            }
        }
    }

    protected virtual void CharGroundShoot()
    {
        if (!this.charCtrl.isOnGround) return;
        if (this.isAttacking) return;
        StartCoroutine(DeclareRangedAttack());
    }

    protected IEnumerator DeclareRangedAttack()
    {
        this.charCtrl.isActionOcurr = true;
        this.isAttacking = true;
        this.charCtrl.charTakeDamage.isAbleToTakeDmg = false;
        this.charCtrl.charAniCtrl.NonLoopAnimationPlay("GroundShotAnimation");
        yield return new WaitForSeconds(0.4f);
        this.SetupBullet(BulletSpawner.normalBullet);
        yield return new WaitUntil(() => !this.charCtrl.charAniCtrl.isNonLoopAnimation);
        this.isAttacking = false;
        this.charCtrl.charTakeDamage.isAbleToTakeDmg = true;
        this.rangedAtkTime = this.rangedAtkCooldown;
        this.charCtrl.isActionOcurr = false;
    }

    protected virtual void SetupBullet(string bulletName)
    {
        Transform newBullet = BulletSpawner.Instance.Spawn(bulletName, this.shootPoint.position, Quaternion.identity);
        newBullet.gameObject.SetActive(true);
        newBullet.localScale = Vector3.one * this.charCtrl.charMovement.moveDirection.x;
        BulletCtrl bulletCtrl = newBullet.GetComponent<BulletCtrl>();
        bulletCtrl.SetOwner(this.transform.parent);
        bulletCtrl.isImpactable = true;
        bulletCtrl.bulletRigidbody2D.velocity = this.charCtrl.charMovement.moveDirection * this.bulletSpeed;
    }

    protected IEnumerator WaitForAction(float waitTime)
    {
        float elapsedTime = 0f;
        while ((elapsedTime < waitTime) && !this.charCtrl.isActionOcurr)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (this.charCtrl.isActionOcurr)
        {
            this.charCtrl.canReturnToIdleState = true;
            yield break;
        }
        if (elapsedTime >= waitTime)
        {
            this.charCtrl.canReturnToIdleState = true;
            if (!this.charCtrl.isOnGround) yield break;
            StartCoroutine(EndAttack());
            yield break;
        }
    }
    public IEnumerator EndAttack()
    {
        if (this.charCtrl.isOnGround)
        {
            this.charCtrl.isActionOcurr = true;
            this.charCtrl.charAniCtrl.NonLoopAnimationPlay("EndAtkAnimation");
            yield return new WaitUntil(() => !this.charCtrl.charAniCtrl.isNonLoopAnimation);
            this.charCtrl.isActionOcurr = false;
        }
        else yield return null;
    }

}
