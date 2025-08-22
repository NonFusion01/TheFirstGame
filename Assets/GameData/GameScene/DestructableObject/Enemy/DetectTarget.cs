using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTarget : CoreMonoBehaviour
{
    [SerializeField] protected Enemy enemy;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadEnemy();
    }

    protected virtual void LoadEnemy()
    {
        if (this.enemy != null) return;
        this.enemy = transform.parent.GetComponentInChildren<Enemy>();
        Debug.LogWarning(transform.name + ": Load Enemy", gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        CharController character = other.GetComponent<CharController>();
        if (character != null)
        {
            this.enemy.target = character.transform.Find("PlayerBody").Find("PlayerCenter");
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        CharController character = other.GetComponent<CharController>();
        if (character != null)
        {
            this.enemy.target = null;
        }
    }
}
