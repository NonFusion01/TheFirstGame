using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharManager : CoreMonoBehaviour
{
    protected static CharManager instance;
    public static CharManager Instance => instance;

    [SerializeField] public CharController _charController;
    [SerializeField] public Rigidbody2D _charRigidbody2D;
    [SerializeField] public SpriteRenderer _charSpriteRenderer;
    [SerializeField] public CharGroundCheck _charGroundCheck;
    [SerializeField] public Animator _charAnimator;
    [SerializeField] public CharMeleeAttack _charMeleeAtk;
    [SerializeField] public CharStats _charStats;
    [SerializeField] public SpriteRenderer _charIcon;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharAnimation();
        this.LoadCharRigidbody();
        this.LoadCharSpriteRenderer();
        this.LoadCharGroundCheck();
        this.LoadCharAnimator();
        this.LoadCharMeleeAttack();
        this.LoadCharStats();
        this.LoadCharIcon();
    }

    protected override void Awake()
    {
        base.Awake();
        if (CharManager.instance != null) Debug.LogError("Only 1 Char Ctrl is allowed to exist");
        CharManager.instance = this;
    }

    protected virtual void LoadCharAnimation()
    {
        if (this._charController != null) return;
        this._charController = GetComponent<CharController>();
        Debug.LogWarning(transform.name + ": Load CharController", gameObject);
    }

    protected virtual void LoadCharRigidbody()
    {
        if (this._charRigidbody2D != null) return;
        this._charRigidbody2D = GetComponent<Rigidbody2D>();
        Debug.LogWarning(transform.name + ": Load Char Rigidbody", gameObject);
        this._charRigidbody2D.gravityScale = 3f;
        this._charRigidbody2D.freezeRotation = true;
        
    }

    protected virtual void LoadCharSpriteRenderer()
    {
        if (this._charSpriteRenderer != null) return;
        this._charSpriteRenderer = GetComponent<SpriteRenderer>();
        Debug.LogWarning(transform.name + ": Load Char Sprite Renderer", gameObject);
    }
    protected virtual void LoadCharGroundCheck()
    {
        if (this._charGroundCheck != null) return;
        this._charGroundCheck = GetComponentInChildren<CharGroundCheck>();
        Debug.LogWarning(transform.name + ": Load Char Ground Check", gameObject);
    }

    protected virtual void LoadCharAnimator()
    {
        if (this._charAnimator != null) return;
        this._charAnimator = GetComponent<Animator>();
        Debug.LogWarning(transform.name + ": Load Char Animator", gameObject);
    }

    protected virtual void LoadCharMeleeAttack()
    {
        if (this._charMeleeAtk != null) return;
        this._charMeleeAtk = GetComponentInChildren<CharMeleeAttack>();
        Debug.LogWarning(transform.name + ": Load Char Melee Atk", gameObject);
    }

    protected virtual void LoadCharStats()
    {
        if (this._charStats != null) return;
        this._charStats = GetComponentInChildren<CharStats>();
        Debug.LogWarning(transform.name + ": Load Char Stats", gameObject);
    }

    protected virtual void LoadCharIcon()
    {
        if (this._charIcon != null) return;
        this._charIcon = transform.Find("PlayerIcon").GetComponent<SpriteRenderer>();
        Debug.LogWarning(transform.name + ": Load Char Icon", gameObject);
        this._charIcon.gameObject.SetActive(false);
    }
}
