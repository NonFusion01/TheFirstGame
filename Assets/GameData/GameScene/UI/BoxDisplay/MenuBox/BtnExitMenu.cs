using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnExitMenu : CoreMonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected MenuCtrl menu;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMenu();
    }

    protected virtual void LoadMenu()
    {
        if (this.menu != null) return;
        this.menu = this.transform.parent.GetComponent<MenuCtrl>();
        Debug.LogWarning (transform.name + ": Load Menu", gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.menu.gameObject.SetActive(false);
    }
}
