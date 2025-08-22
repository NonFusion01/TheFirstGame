using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : CoreMonoBehaviour
{
    [SerializeField] protected SavePointsCtrl SavePointsCtrl;
    [SerializeField] public int index;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadSavePointsCtrl();
    }

    protected virtual void LoadSavePointsCtrl()
    {
        if (this.SavePointsCtrl != null) return;
         this.SavePointsCtrl = transform.parent.GetComponent<SavePointsCtrl>();
        Debug.LogWarning(transform.name + ": Load Save Point Ctrl", gameObject);
        this.index = this.transform.GetSiblingIndex();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name != "Player") return;
        this.SavePointsCtrl.SetCurrentSavePoint(this);
    }
}
