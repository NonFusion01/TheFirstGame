using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMoving : CoreMonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera mainCamera;
    [SerializeField] public Transform startPoint;
    [SerializeField] public Transform stopPoint;
    [SerializeField] public Transform cameraAlternativeFollow;
    [SerializeField] protected Transform player;
    public Transform Player => player;
    [SerializeField] protected bool isChangingLocation = false;
    public bool IsChangingLocation => isChangingLocation;
    protected float progressTime = 0f;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCamera();
        this.LoadPoints();
        this.LoadAlternativeFollow();
        this.LoadPlayer();
    }

    protected virtual void LoadCamera()
    {
        if (this.mainCamera != null) return;
        this.mainCamera = FindObjectOfType<CinemachineVirtualCamera>();
        Debug.LogWarning(transform.name + ": Load Camera", gameObject);
    }

    protected virtual void LoadPoints()
    {
        if (this.startPoint != null || this.stopPoint != null) return;
        this.startPoint = transform.Find("StartPoint");
        Debug.LogWarning(transform.name + ": Load Start Point", gameObject);
        this.stopPoint = transform.Find("StopPoint");   
        Debug.LogWarning(transform.name + ": Load Stop Point", gameObject);
    }

    protected virtual void LoadAlternativeFollow()
    {
        if (this.cameraAlternativeFollow != null) return;
        this.cameraAlternativeFollow = transform.Find("AlternativeFollow");
        Debug.LogWarning(transform.name + "Load Alternative Follow", gameObject);
        this.cameraAlternativeFollow.position = this.startPoint.position;
    }

    protected virtual void LoadPlayer()
    {
        if (this.player != null) return;
        this.player = GameObject.Find("Player").transform;
        Debug.LogWarning(transform.name + ": Load Player", gameObject);
    }

    protected override void Start()
    {
        this.cameraAlternativeFollow.position = this.player.position;
    }

    protected virtual void FixedUpdate()
    {
        if (!this.isChangingLocation && !CharManager.Instance._charStats.inInvincibleState) this.cameraAlternativeFollow.position = this.player.position;
    }

    public IEnumerator MoveCamera(float time)
    {
        this.isChangingLocation = true;
        this.mainCamera.Follow = this.cameraAlternativeFollow;
        this.mainCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0;
        this.player.GetComponent<CharController>().DisableController();
        float distance = Vector3.Distance(this.cameraAlternativeFollow.position, stopPoint.position);
        while (distance > 0.05f)
        {
            yield return null;
            this.progressTime += Time.deltaTime;
            if (this.progressTime > time) this.progressTime = time;
            float completionPercentage = this.progressTime / time;
            this.cameraAlternativeFollow.position = Vector3.Lerp(startPoint.position, stopPoint.position, completionPercentage);
            distance = Vector3.Distance(this.cameraAlternativeFollow.position, stopPoint.position);
        }
        if (distance <= 0.05f)
        {
            Debug.Log("Camera Move Complete!");
            this.cameraAlternativeFollow.position = stopPoint.position;
            this.progressTime = 0f;
            this.isChangingLocation = false;
            yield break;
        }
    }
}
