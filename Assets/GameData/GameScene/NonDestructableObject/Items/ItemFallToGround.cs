using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFallToGround : CoreMonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadRigidbody();
    }

    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponent<Rigidbody2D>();
        Debug.LogWarning(transform.name + ": Load Rigidbody", gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.FallToGround();
        Physics.IgnoreLayerCollision(14, 15);
        Physics2D.IgnoreLayerCollision(14, 15);
    }

    protected virtual void FallToGround()
    {
        this.rb.velocity = new Vector3(0, -5, 0);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        // check if the object is falling and collides with a non-destructible object
        if (other.gameObject.layer == 11)
        {
            this.rb.velocity = Vector3.zero;
        }

    }
}
