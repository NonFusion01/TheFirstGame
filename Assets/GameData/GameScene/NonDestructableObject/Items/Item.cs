using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item : CoreMonoBehaviour
{
    [SerializeField] protected DropItemSO itemSO;
    public DropItemSO ItemSO => itemSO;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] public int itemCount;
    [SerializeField] protected float dropRate;
    public float DropRate => dropRate;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadItemSO();
        this.LoadSprite();
    }

    protected virtual void LoadItemSO()
    {
        string itemName = this.gameObject.name;
        if (this.itemSO != null) return; 
        this.itemSO = Resources.Load<DropItemSO>("ScriptableObject/DropItems/" + itemName);
        Debug.LogWarning(transform.name + ": Load Item SO", gameObject);
        this.dropRate = this.itemSO.DropRate;
    }

    protected virtual void LoadSprite()
    {
        if (this.sprite != null) return;
        this.sprite = GetComponent<SpriteRenderer>();
        Debug.LogWarning(transform.name + ": Load Sprite", gameObject);
        this.sprite.sprite = this.itemSO.ItemSprite;
    }
}
