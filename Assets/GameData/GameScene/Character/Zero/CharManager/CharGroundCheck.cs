using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharGroundCheck : CoreMonoBehaviour
{
    [SerializeField] protected Collider2D _charFeet;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharFeet();
    }

    protected virtual void LoadCharFeet()
    {
        if (this._charFeet != null) return;
        this._charFeet = GetComponent<Collider2D>();
        Debug.LogWarning(transform.name + ": Load Char Feet", gameObject);
        this._charFeet.isTrigger = true;
    }

    //OnTrigger vs OnCollision
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != 11) return; //11: terrain layer
        CharManager.Instance._charController.charMovement.jumpCount = 0;
        CharManager.Instance._charController.charMovement.airDashCountLeft = 1;
    }
    protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer != 11) return; //11: terrain layer
        CharManager.Instance._charController.charMovement.jumpCount = 0;
        CharManager.Instance._charController.isOnGround = true;
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != 11) return; //11: terrain layer
        CharManager.Instance._charController.isOnGround = false;
    }
}
