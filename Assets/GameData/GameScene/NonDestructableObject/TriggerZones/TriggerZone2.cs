using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone2 : TriggerZoneCtrl
{
    [SerializeField] protected TriggerZoneToMoveCamera moveCam;
    [SerializeField] protected TriggerZoneToMoveGate moveGates;
    [SerializeField] protected Transform player;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMoveCam();
        this.LoadMoveGates();
        this.LoadPlayer();
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

    protected virtual void LoadPlayer()
    {
        if (this.player != null) return;
        this.player = GameObject.Find("Player").transform;
        Debug.LogWarning(transform.name + ": Load Player", gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (this.isTriggered && !this.canTriggerMultipleTimes) return;
        CharController charController = other.GetComponent<CharController>();
        if (charController == null) return;
        StartCoroutine(TriggerZoneAction());
        this.isTriggered = true;
    }
    protected IEnumerator TriggerZoneAction()
    {
        // Move cam to stop point
        CharManager.Instance._charStats.inInvincibleState = true;
        this.moveCam.cameraMoving.startPoint.position = this.moveCam.cameraMoving.cinemachineCamera.transform.position;
        this.moveCam.cameraMoving.stopPoint.position = this.moveCam.camStopPoint.position;
        this.moveCam.CameraMove(2f);
        yield return new WaitUntil(() => !this.moveCam.cameraMoving.IsChangingLocation);
        Debug.Log("Move Forward");

        // Move gates
        yield return new WaitForSeconds(1f); // Wait for camera to settle
        this.moveGates.GateMove();
        yield return new WaitUntil(() => this.moveGates.isAllGatedMoved);
        Debug.Log("All Gates moved");

        // Move cam back
        this.moveCam.cameraMoving.startPoint.position = this.moveCam.cameraMoving.cinemachineCamera.transform.position;
        this.moveCam.cameraMoving.stopPoint.position = new Vector3(this.player.position.x, this.player.position.y, -10);
        this.moveCam.CameraMove(2f);
        Debug.Log("Move Back");
        yield return new WaitUntil(() => !this.moveCam.cameraMoving.IsChangingLocation);

        yield return new WaitForSeconds(1f); // Wait for camera to settle
        this.moveCam.cameraMoving.Player.GetComponent<CharController>().isDisableController = false;
        CharManager.Instance._charStats.inInvincibleState = false;

    }

}
