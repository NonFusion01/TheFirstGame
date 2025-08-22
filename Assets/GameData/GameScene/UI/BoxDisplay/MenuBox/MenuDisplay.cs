using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplay : BoxUI
{
    [SerializeField] protected BoxUICtrl boxUICtrl;
    [SerializeField] Transform menu;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBoxUICtrl();
        this.LoadMenu();
    }

    protected virtual void LoadBoxUICtrl()
    {
        if (this.boxUICtrl != null) return;
        this.boxUICtrl = transform.parent.GetComponent<BoxUICtrl>();
        Debug.LogWarning(transform.name + ": Load Box UI Ctrl", gameObject);
    }

    protected virtual void LoadMenu()
    {
        if (this.menu != null) return;
        this.menu = transform.Find("Menu");
        Debug.LogWarning(transform.name + ": Load Menu", gameObject);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (this.boxUICtrl.isAnyBoxOpen) return;
            if (this.menu.gameObject.activeSelf) return;
            this.menu.gameObject.SetActive(true);
            this.isBoxOpen = true;
            this.boxUICtrl.isAnyBoxOpen = true;
            GameManagerScript.isGamePaused = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!this.menu.gameObject.activeSelf) return;
            this.menu.gameObject.SetActive(false);
            this.isBoxOpen = false;
            GameManagerScript.isGamePaused = false;
        }
    }
}
