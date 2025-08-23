using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone1 : TriggerZoneCtrl
{
    [SerializeField] protected TriggerZoneToMoveCamera moveCam;
    [SerializeField] protected TriggerZoneToMoveGate moveGates;
    [SerializeField] protected TriggerZoneToSpawnBoss spawnBoss;


    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMoveCam();
        this.LoadMoveGates();
        this.LoadSpawnBoss();
    }

    protected virtual void LoadMoveCam()
    {
        if (this.moveCam != null) return;
        this.moveCam = GetComponent<TriggerZoneToMoveCamera>();
        Debug.LogWarning(transform.name + ": Load Move Cam", gameObject);
    }

    protected virtual void LoadMoveGates()
    {
        if (this.moveGates != null) return;
        this.moveGates = GetComponent<TriggerZoneToMoveGate>();
        Debug.LogWarning(transform.name + ": Load Move Gates", gameObject);
    }

    protected virtual void LoadSpawnBoss()
    {
        if (this.spawnBoss != null) return;
        this.spawnBoss = GetComponent<TriggerZoneToSpawnBoss>();
        Debug.LogWarning(transform.name + ": Load Spawn Boss", gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (this.isTriggered && !this.canTriggerMultipleTimes) return;
        CharController charController = other.GetComponent<CharController>();
        if (charController == null) return;
        StartCoroutine(TriggerZoneAction());
        this.isTriggered = true;
    }

    protected virtual void FixedUpdate()
    {
        if (CharManager.Instance._charStats.IsDefeated)
        { 
            this.moveGates.ResetGates(); 
            this.isTriggered = false;
        }
    }

    protected IEnumerator TriggerZoneAction()
    {
        this.moveCam.cameraMoving.startPoint.position = this.moveCam.cameraMoving.cinemachineCamera.transform.position;
        this.moveCam.cameraMoving.stopPoint.position = this.moveCam.camStopPoint.position;
        this.moveCam.CameraMove(2f);
        this.moveGates.GateMove();
        this.spawnBoss.BossSpawn();
        yield return new WaitUntil(() => !this.moveCam.cameraMoving.IsChangingLocation);
        this.moveCam.cameraMoving.ChangeCameraConfiner(1);
        this.moveCam.cameraMoving.Player.GetComponent<CharController>().isDisableController = false;
        this.moveCam.LockCamInXAxis();
    }
}
