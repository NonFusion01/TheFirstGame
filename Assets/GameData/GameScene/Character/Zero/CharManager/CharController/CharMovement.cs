using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : CoreMonoBehaviour
{
    [SerializeField] protected CharController charCtrl;
    //transform according to the character scale
    protected Vector3 vectorScaleR = new Vector3(2, 2, 1);
    protected Vector3 vectorScaleL = new Vector3(-2, 2, 1);
    //move
    [Header("Moving")]
    public float groundVelocity = 0;
    protected Vector3 moveVelocity = Vector3.zero;
    protected Vector3 vectorToRight = new Vector3(1, 0, 0);
    protected Vector3 vectorToLeft = new Vector3(-1, 0, 0);
    public Vector3 moveDirection = new Vector3(1, 0, 0);
    public float moveSpeed = 3f;
    //jump
    [Header("Jumping")]
    public float jumpForce = 10f;
    public int jumpCount = 0;
    protected Vector3 _vectorJump = new Vector3(0, 1, 0);
    //dash
    [Header("Dashing")]
    public bool isDashing = false;
    public float dashForce = 7f;
    public float airDashForce = 5f;
    public int airDashCountLeft = 1;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharCtrl();
    }

    protected virtual void LoadCharCtrl()
    {
        if (this.charCtrl != null) return;
        this.charCtrl = transform.parent.GetComponentInChildren<CharController>();
        Debug.LogWarning(transform.name + ": Load Char Ctrl", gameObject);
    }

    protected virtual void Update()
    {
        if (GameManagerScript.isGamePaused) return;
        if (this.charCtrl.isDisableController) return;
        this.CharMove(); 
    }

    protected virtual void CharMove()
    {
        if (this.charCtrl.charTakeDamage.isTakingDmg) return;
        if (this.charCtrl.charAttack.isAttacking) return;
        if (this.charCtrl.charSkillSelection.isUsingSkill) return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            this.charCtrl.isActionOcurr = true;
            this.CharJump();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            this.CharDash();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.charCtrl.isActionOcurr = true;
            this.CharMoveRight();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            this.charCtrl.isActionOcurr = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.charCtrl.isActionOcurr = true;
            this.CharMoveLeft();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            this.charCtrl.isActionOcurr = false;
        }
    }

    protected virtual void CharMoveLeft()
    {
        if (this.isDashing) return;
        this.transform.parent.localScale = this.vectorScaleL;
        this.moveDirection = this.vectorToLeft;
        this.CharMoveLeftRight(moveDirection);
    }

    protected virtual void CharMoveRight()
    {
        if (this.isDashing) return;
        this.transform.parent.localScale = this.vectorScaleR;
        this.moveDirection = this.vectorToRight;
        this.CharMoveLeftRight(moveDirection);
    }

    protected virtual void CharMoveLeftRight(Vector3 moveVector)
    {
        Vector3 newMoveVector = new Vector3(moveVector.x * this.moveSpeed + this.groundVelocity, CharManager.Instance._charRigidbody2D.velocity.y, 0);
        CharManager.Instance._charRigidbody2D.velocity = newMoveVector;
        if (!this.charCtrl.isOnGround) return;
        this.charCtrl.charAniCtrl.PlayLoopAnimation("WalkP2Animation");
    }

    public virtual void CharJump()
    {
        if (this.jumpCount >= 2) return;
        if (!this.charCtrl.isOnGround) this.jumpCount = 1;
        CharManager.Instance._charRigidbody2D.velocity = new Vector3 (CharManager.Instance._charRigidbody2D.velocity.x, 0, 0);
        CharManager.Instance._charRigidbody2D.gravityScale = this.charCtrl.basicGravityScale;
        CharManager.Instance._charRigidbody2D.AddForce(_vectorJump * this.jumpForce, ForceMode2D.Impulse);
        this.jumpCount++;
        if (this.jumpCount == 1) this.charCtrl.charAniCtrl.NonLoopAnimationPlay("Jump1Animation");
        if (this.jumpCount == 2) this.charCtrl.charAniCtrl.NonLoopAnimationPlay("Jump2Animation");
    }

    protected virtual void CharDash()
    {
        if (this.isDashing) return;
        if (this.airDashCountLeft == 0) return;
        StartCoroutine(Dashing());
    }

    protected IEnumerator Dashing()
    {
        this.charCtrl.isActionOcurr = true;
        this.isDashing = true;
        CharManager.Instance._charRigidbody2D.velocity = Vector3.zero;
        if (this.charCtrl.isOnGround)
        {
            CharManager.Instance._charRigidbody2D.AddForce(this.moveDirection * this.dashForce, ForceMode2D.Impulse);
        }
        if (!this.charCtrl.isOnGround)
        {
            CharManager.Instance._charRigidbody2D.AddForce(this.moveDirection * this.airDashForce, ForceMode2D.Impulse);
            CharManager.Instance._charRigidbody2D.gravityScale = 0;
            this.airDashCountLeft--;
        }
        this.charCtrl.charAniCtrl.NonLoopAnimationPlay("DashP1Animation");
        yield return null;
        StartCoroutine(WaitForAction(0.5f));
    }

    protected IEnumerator WaitForAction(float waitTime)
    {
        float elapsedTime = 0f;
        while ((elapsedTime < waitTime) && !this.charCtrl.charSkillSelection.isUsingSkill)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (elapsedTime >= waitTime)
        {
            CharManager.Instance._charRigidbody2D.gravityScale = this.charCtrl.basicGravityScale;
            this.EndAction();
            yield break;
        }
        if (this.charCtrl.charSkillSelection.isUsingSkill)
        {
            yield return new WaitUntil(() => !this.charCtrl.charSkillSelection.isUsingSkill);
            this.EndAction();
            yield break;
        }   
    }

    protected virtual void EndAction()
    {
        this.isDashing = false;
        this.charCtrl.isActionOcurr = false;

    }
}
