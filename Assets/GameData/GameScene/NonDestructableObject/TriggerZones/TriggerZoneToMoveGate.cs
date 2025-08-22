using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneToMoveGate : CoreMonoBehaviour
{
    [SerializeField] protected TriggerZoneCtrl triggerZoneCtrl;
    [SerializeField] protected List<Gate> gateList;
    public bool isAllGatedMoved = false;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadTriggerZoneCtrl();
        this.LoadGate();
    }

    protected virtual void LoadTriggerZoneCtrl()
    {
        if (this.triggerZoneCtrl != null) return;
        this.triggerZoneCtrl = GetComponent<TriggerZoneCtrl>();
        Debug.LogWarning(transform.name + ": Load Trigger Zone Ctrl", gameObject);
    }

    protected virtual void LoadGate()
    {
        if (this.gateList.Count > 0) return;
        Transform gates = this.transform.parent.Find("Gates");
        foreach (Transform transform in gates)
        {
            Gate gate = transform.GetComponentInChildren<Gate>();
            if (gate != null) this.gateList.Add(gate);
        }
        Debug.LogWarning(transform.name + ": Load Gate", gameObject);
    }

    public virtual void GateMove()
    {
        foreach (Gate gate in this.gateList)
        {
            StartCoroutine(gate.MoveGate());
        }
        StartCoroutine(CheckAllGatesMoved());
    }

    protected IEnumerator CheckAllGatesMoved()
    {
        int gateMoved = 0;
        yield return new WaitForSeconds(1f);
        foreach (Gate gate in this.gateList)
        {
            if (!gate.isMoved) continue; 
            gateMoved++;
        }
        if (gateMoved == this.gateList.Count)
        { 
            this.isAllGatedMoved = true;
            Debug.Log("All Gates Moved");
            yield break;
        }
        else
        {
            StartCoroutine(CheckAllGatesMoved());
        }
    }

    public virtual void ResetGates()
    {
        foreach (Gate gate in this.gateList)
        {
            gate.ResetGatePosition();
        }
        this.isAllGatedMoved = false;
    }
}
