using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMoving : CoreMonoBehaviour
{
    [SerializeField] public Camera mainCamera;
    [SerializeField] public CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] public CinemachineConfiner2D cameraConfiner2D;
    [SerializeField] public Transform startPoint;
    [SerializeField] public Transform stopPoint;
    [SerializeField] public Transform cameraAlternativeFollow;
    [SerializeField] public List<Transform> backgrounds;
    [SerializeField] protected Transform player;
    public Transform Player => player;
    [SerializeField] protected bool isChangingLocation = false;
    public bool IsChangingLocation => isChangingLocation;
    protected float progressTime = 0f;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMainCamera();
        this.LoadCinemachineCamera();
        this.LoadCinemachineConfiner2D();
        this.LoadPoints();
        this.LoadAlternativeFollow();
        this.LoadPlayer();
        this.LoadBackgrounds();
    }

    protected virtual void LoadMainCamera()
    {
        if (this.mainCamera != null) return;
        this.mainCamera = Camera.main;
        Debug.LogWarning(transform.name + ": Load Main Camera", gameObject);
    }

    protected virtual void LoadCinemachineCamera()
    {
        if (this.cinemachineCamera != null) return;
        this.cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
        Debug.LogWarning(transform.name + ": Load Camera", gameObject);
    }

    protected virtual void LoadCinemachineConfiner2D()
    {
        if (this.cameraConfiner2D != null) return;
        this.cameraConfiner2D = this.cinemachineCamera.GetComponent<CinemachineConfiner2D>();
        Debug.LogWarning(transform.name + ": Load Cinemachine Confiner 2D", gameObject);
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

    protected virtual void LoadBackgrounds()
    {
        if (this.backgrounds.Count > 0) return;
        Transform transform = GameObject.Find("Backgrounds").transform;
        foreach (Transform child in transform)
        {
            this.backgrounds.Add(child);
        }
        Debug.LogWarning(base.transform.name + ": Load Backgrounds", gameObject);
    }

    protected override void Start()
    {
        this.SetupInitialCamera();
    }

    protected virtual void FixedUpdate()
    {
        if (!this.isChangingLocation && !CharManager.Instance._charStats.inInvincibleState) this.cameraAlternativeFollow.position = this.player.position;
    }

    public virtual void SetCamFollowPlayer()
    {
        this.cameraAlternativeFollow.position = this.player.position;
    }

    protected virtual void SetupInitialCamera()
    {
        this.cameraAlternativeFollow.position = this.player.position;
        this.cameraConfiner2D.m_BoundingShape2D = this.backgrounds[0].GetComponent<PolygonCollider2D>();
    }

    public virtual void ChangeCameraConfiner(int index)
    {
        if (index < 0 || index >= this.backgrounds.Count) return;
        this.cameraConfiner2D.m_BoundingShape2D = this.backgrounds[index].GetComponent<PolygonCollider2D>();
    }

    public IEnumerator MoveCamera(float time)
    {
        this.isChangingLocation = true;
        this.cameraAlternativeFollow.position = this.startPoint.position;
        this.cinemachineCamera.Follow = this.cameraAlternativeFollow;
        this.cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0;
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
