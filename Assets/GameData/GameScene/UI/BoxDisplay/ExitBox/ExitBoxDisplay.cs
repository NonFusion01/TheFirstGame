using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBoxDisplay : BoxUI
{
    [SerializeField] protected BoxUICtrl boxUICtrl;
    [SerializeField] protected Transform exitBox;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBoxUICtrl();
        this.LoadExitBox();
    }

    protected virtual void LoadBoxUICtrl()
    {
        if (this.boxUICtrl != null) return;
        this.boxUICtrl = transform.parent.GetComponent<BoxUICtrl>();
        Debug.LogWarning(transform.name + ": Load Box UI Ctrl", gameObject);
    }

    protected virtual void LoadExitBox()
    {
        if (this.exitBox != null) return;
        this.exitBox = transform.Find("ExitBox");
        Debug.LogWarning(transform.name + ": Load Exit Box", gameObject);
        this.exitBox.gameObject.SetActive(false);
        this.isBoxOpen = false;
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!this.exitBox.gameObject.activeSelf)
            {
                if (this.boxUICtrl.isAnyBoxOpen) return;
                this.OpenBox();
            }
            else
            {
                this.EscapeBox();
            }
        }
    }

    protected virtual void OpenBox()
    {
        this.exitBox.gameObject.SetActive(true);
        this.isBoxOpen = true;
        this.boxUICtrl.isAnyBoxOpen = true;
        GameManagerScript.isGamePaused = true;
    }

    public virtual void EscapeBox()
    {
        this.exitBox.gameObject.SetActive(false);
        this.isBoxOpen = false;
        GameManagerScript.isGamePaused = false;
    }
}
