using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : CoreMonoBehaviour
{
    [SerializeField] public CharMovement charMovement;
    [SerializeField] public CharAttack charAttack;
    [SerializeField] public CharSkillSelection charSkillSelection;
    [SerializeField] public CharAniCtrl charAniCtrl;
    [SerializeField] public CharTakeDmg charTakeDamage;
    public bool isDisableController = false;
    //rigidbody
    [Header("Basic Infomation")]
    public float basicGravityScale = 3f;
    public bool isOnGround;
    public bool isActionOcurr = false;
    public bool canReturnToIdleState = true;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharMovement();
        this.LoadCharAttack();
        this.LoadCharSkillSelection();
        this.LoadCharAniCtrl();
        this.LoadCharTakeDamage();
    }

    protected virtual void LoadCharMovement()
    {
        if (this.charMovement != null) return;
        this.charMovement = GetComponentInChildren<CharMovement>();
        Debug.LogWarning(transform.name + ": Load Char Movement", gameObject);
    }

    protected virtual void LoadCharAttack()
    {
        if (this.charAttack != null) return;
        this.charAttack = GetComponentInChildren<CharAttack>();
        Debug.LogWarning(transform.name + ": Load Char Attack", gameObject);
    }
    protected virtual void LoadCharSkillSelection()
    {
        if (this.charSkillSelection != null) return;
        this.charSkillSelection = GetComponentInChildren<CharSkillSelection>();
        Debug.LogWarning(transform.name + ": Load Char Skills", gameObject);
    }

    protected virtual void LoadCharAniCtrl()
    {
        if (this.charAniCtrl != null) return;
        this.charAniCtrl = GetComponentInChildren<CharAniCtrl>();
        Debug.LogWarning(transform.name + ": Load Char Animation Ctrl", gameObject);
    }

    protected virtual void LoadCharTakeDamage()
    {
        if (this.charTakeDamage != null) return;
        this.charTakeDamage = GetComponentInChildren<CharTakeDmg>();
        Debug.LogWarning(transform.name + ": Load Char Take Damage", gameObject);
    }


    //Touch Controller
    //public bool isLeftBtnHolding;
    //public bool isRightBtnHolding;

    protected override void Start()
    {
        base.Start();
        Physics2D.IgnoreLayerCollision(0, 10); //ignore player layer and Background layer
    }

    protected virtual void Update()
    {
        this.CharFall();
        this.CharIdle();
    }

    protected virtual void CharIdle()
    {
        if (this.charMovement.isDashing) return;
        if (this.charAttack.isAttacking) return;
        if (!this.canReturnToIdleState) return;
        this.charAniCtrl.AnimationStop();
    }

    protected virtual void CharFall()
    {
        if (this.isOnGround) return;
        if (this.charAttack.isAttacking) return;
        if (this.charTakeDamage.isTakingDmg) return;
        if (this.charSkillSelection.isUsingSkill) return;

        if (CharManager.Instance._charRigidbody2D.velocity.y < 0)
        {
            this.isActionOcurr = false;
            this.charAniCtrl.PlayLoopAnimation("FallP1Animation");
            
        }
        //limit fall speed
        if (CharManager.Instance._charRigidbody2D.velocity.y < -10)
        { 
            CharManager.Instance._charRigidbody2D.velocity = new Vector3(CharManager.Instance._charRigidbody2D.velocity.x, -10, 0); 
        }
    }

    public virtual void DisableController()
    {
        this.isDisableController = true;
        this.charMovement.isDashing = false;
        this.charAttack.isAttacking = false;
        this.charSkillSelection.isUsingSkill = false;
        this.isActionOcurr = false;
        this.charAniCtrl.AnimationStop();
        CharManager.Instance._charRigidbody2D.velocity = Vector3.zero;
    }
}
