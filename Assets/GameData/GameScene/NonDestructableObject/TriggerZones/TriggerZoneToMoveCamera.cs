using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneToMoveCamera : CoreMonoBehaviour
{
    [SerializeField] protected TriggerZoneCtrl triggerZoneCtrl;
    [SerializeField] public CameraMoving cameraMoving;
    [SerializeField] public Transform camStopPoint;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadTriggerZoneCtrl();
        this.LoadCameraMoving();
        this.LoadStopPoint();
    }

    protected virtual void LoadTriggerZoneCtrl()
    {
        if (this.triggerZoneCtrl != null) return;
        this.triggerZoneCtrl = GetComponent<TriggerZoneCtrl>();
        Debug.LogWarning(transform.name + ": Load Trigger Zone Ctrl", gameObject);
    }

    protected virtual void LoadCameraMoving()
    {
        if (this.cameraMoving != null) return;
        this.cameraMoving = GameObject.Find("CameraMoving").GetComponent<CameraMoving>();
        Debug.LogWarning(transform.name + ": Load Camera Moving", gameObject);
    }

    protected virtual void LoadStopPoint()
    {
        if (this.camStopPoint != null) return;
        this.camStopPoint = transform.parent.Find("CamStopPoint");
        Debug.LogWarning(transform.name + ": Load Stop Point", gameObject);
    }

    public virtual void CameraMove(float time)
    {
        StartCoroutine(this.cameraMoving.MoveCamera(time));
    }

    public virtual void LockCamInXAxis()
    {
        if (this.cameraMoving.IsChangingLocation) return;
        this.cameraMoving.cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 2;
        //this.cameraMoving.mainCamera.Follow = CharManager.Instance.transform;
    }
}
