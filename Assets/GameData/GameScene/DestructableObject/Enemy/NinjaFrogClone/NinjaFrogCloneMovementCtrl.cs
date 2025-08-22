using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaFrogCloneMovementCtrl : CoreMonoBehaviour
{
    [SerializeField] protected NinjaFrogCloneCtrl ninjaFrogCloneCtrl;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float moveSpeed = 2.0f;
    [SerializeField] protected float jumpForce = 5.0f;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadNinjaFrogCtrl();
        this.LoadSpriteRenderer();
    }

    protected virtual void LoadNinjaFrogCtrl()
    {
        if (this.ninjaFrogCloneCtrl != null) return;
        this.ninjaFrogCloneCtrl = transform.parent.GetComponent<NinjaFrogCloneCtrl>();
        Debug.LogWarning(transform.name + ": Load Ninja Frog Clone Ctrl", gameObject);
    }

    protected virtual void LoadSpriteRenderer()
    {
        if (this.spriteRenderer != null) return;
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.LogWarning(transform.name + ": Load Sprite Renderer", gameObject);
    }


    public IEnumerator MoveRight(float time)
    {
        this.ninjaFrogCloneCtrl.rb2D.velocity = this.moveSpeed * this.ninjaFrogCloneCtrl.transform.right;
        yield return new WaitForSeconds(time);
        this.ninjaFrogCloneCtrl.rb2D.velocity = Vector2.zero;

    }

    public IEnumerator MoveLeft(float time)
    {
        this.ninjaFrogCloneCtrl.rb2D.velocity = -this.moveSpeed * this.ninjaFrogCloneCtrl.transform.right;
        yield return new WaitForSeconds(time);
        this.ninjaFrogCloneCtrl.rb2D.velocity = Vector2.zero;
    }

    public IEnumerator Jump()
    {
        yield return new WaitForSeconds(3f);
        this.ninjaFrogCloneCtrl.rb2D.AddForce(Vector2.up * this.jumpForce, ForceMode2D.Impulse);

    }
}
