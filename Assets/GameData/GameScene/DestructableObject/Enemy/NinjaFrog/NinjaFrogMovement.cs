using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaFrogMovement : CoreMonoBehaviour
{
    [SerializeField] protected NinjaFrogCtrl ninjaFrogCtrl;
    [SerializeField] protected float moveSpeed = 2.0f;
    [SerializeField] protected float jumpForce = 5.0f;
    public Vector3 localScaleR = new Vector3(1, 1, 1);
    public Vector3 localScaleL = new Vector3(-1, 1, 1);
    public Vector2 moveDirection;

    public bool isAction = false;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadNinjaFrogCtrl();
    }

    protected virtual void LoadNinjaFrogCtrl()
    {
        if (this.ninjaFrogCtrl != null) return;
        this.ninjaFrogCtrl = transform.parent.GetComponent<NinjaFrogCtrl>();
        Debug.LogWarning(transform.name + ": Load Ninja Frog Ctrl", gameObject);
    }

    protected virtual void Update()
    {
        this.UpdateSprite();
        if (!this.isAction)
        {
            this.Rest();
        }
    }

    protected virtual void UpdateSprite()
    {
        if (this.ninjaFrogCtrl.rb.velocity.x > 0)
        {
            this.ninjaFrogCtrl.transform.localScale = this.localScaleR;
            this.moveDirection = new Vector2(this.localScaleR.x, 0);
        }
        else if (this.ninjaFrogCtrl.rb.velocity.x < 0)
        {
            this.ninjaFrogCtrl.transform.localScale = this.localScaleL;
            this.moveDirection = new Vector2(this.localScaleL.x, 0);
        }
    }

    public IEnumerator MoveRight(float time)
    {
        Debug.Log("Boss Move Right");
        this.isAction = true;
        this.ninjaFrogCtrl.rb.velocity = this.moveSpeed * this.ninjaFrogCtrl.transform.right;
        this.ninjaFrogCtrl.PlayLoopAnimation("NinjaFrogRunAnimation");
        yield return new WaitForSeconds(time);
        this.ninjaFrogCtrl.rb.velocity = Vector2.zero;
        this.isAction = false;
    }

    public IEnumerator MoveLeft(float time)
    {
        Debug.Log("Boss Move Left");
        this.isAction = true;
        this.ninjaFrogCtrl.rb.velocity = -this.moveSpeed * this.ninjaFrogCtrl.transform.right;
        this.ninjaFrogCtrl.PlayLoopAnimation("NinjaFrogRunAnimation");
        yield return new WaitForSeconds(time);
        this.ninjaFrogCtrl.rb.velocity = Vector2.zero;
        this.isAction = false;
    }

    public IEnumerator Jump()
    {
        Debug.Log("Boss Jump");
        this.isAction = true;
        yield return null;
        this.ninjaFrogCtrl.rb.AddForce(Vector2.up * this.jumpForce, ForceMode2D.Impulse);
        this.isAction = false;
        this.ninjaFrogCtrl.NonLoopAnimationPlay("NinjaFrogJumpAnimation");
    }

    protected virtual void Rest()
    {
        this.ninjaFrogCtrl.PlayLoopAnimation("NinjaFrogIdleAnimation");
    }
}
