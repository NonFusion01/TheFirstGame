using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxUICtrl : CoreMonoBehaviour
{
    [SerializeField] protected List<BoxUI> boxUIList;
    [SerializeField] public bool isAnyBoxOpen = false;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBoxUIs();
    }

    protected virtual void LoadBoxUIs()
    {
        if (this.boxUIList.Count > 0) return;
        foreach (Transform transform in this.transform)
        {
            BoxUI boxUI = transform.GetComponent<BoxUI>();
            if (boxUI == null) continue;
            this.boxUIList.Add(boxUI);
            Debug.LogWarning(transform.name + ": Load Box UI", gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        this.CheckBoxOpen();
    }

    protected virtual void CheckBoxOpen()
    {
        foreach (BoxUI boxUI in this.boxUIList)
        {
            if (boxUI.isBoxOpen)
            {
                this.isAnyBoxOpen = true;
                return;
            }
        }
        this.isAnyBoxOpen = false;
    }
}
