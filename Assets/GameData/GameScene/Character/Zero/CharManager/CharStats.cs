using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharStats : CoreMonoBehaviour
{
    [SerializeField] protected CharController charController;
    [SerializeField] protected int playTimeLeft = 7;
    public int PlayTimeLeft => playTimeLeft;
    [SerializeField] public int maxHP = 100;
    [SerializeField] public int currentHP;
    // Resurrection
    [SerializeField] protected SavePointsCtrl savePointCtrl;
    [SerializeField] protected CameraMoving cameraMoving;
    [SerializeField] public bool inInvincibleState = false;
    [SerializeField] protected bool isDefeated = false;
    public bool IsDefeated => isDefeated;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharController();
        this.LoadSavePointsCtrl();
        this.LoadCameraMoving();
    }

    protected virtual void LoadCharController()
    {
        if (this.charController != null) return;
        this.charController = transform.parent.GetComponent<CharController>();
        Debug.LogWarning(transform.name + ": Load Char Ctrl", gameObject);
    }

    protected virtual void LoadSavePointsCtrl()
    {
        if (this.savePointCtrl != null) return;
        this.savePointCtrl = FindObjectOfType<SavePointsCtrl>();
        Debug.LogWarning(transform.name + ": Load Save Points Ctrl", gameObject);
    }

    protected virtual void LoadCameraMoving()
    {
        if (this.cameraMoving != null) return;
        this.cameraMoving = FindObjectOfType<CameraMoving>();
        Debug.LogWarning(transform.name + ": Load Camera Moving", gameObject);
    }

    protected override void Start()
    {
        base.Start();
        this.LoadInitialStats();
    }

    protected virtual void LoadInitialStats()
    {
        this.currentHP = this.maxHP;
    }

    protected virtual void Update()
    {
        if (this.currentHP <= 0)
        {
            if (this.isDefeated) return;
            this.playTimeLeft--;
            if (this.playTimeLeft < 0) this.playTimeLeft = 0;
            this.isDefeated = true;
            StartCoroutine(DefeatedState());
        }
    }

    protected IEnumerator DefeatedState()
    {
        //this.charCtrl._charController.isDisableController = true;
        CharManager.Instance._charController.charSkillSelection.SkillList[2].CancelSkill();

        //this.cameraMoving.mainCamera.Follow = this.cameraMoving.cameraAlternativeFollow;
        CharManager.Instance._charRigidbody2D.velocity = Vector2.zero;
        CharManager.Instance._charSpriteRenderer.color = new Color(1, 1, 1, 0);
        Transform newFx = FXSpawner.Instance.Spawn("DesappearingEffect", this.transform.position, Quaternion.identity);
        newFx.gameObject.SetActive(true);
        newFx.transform.localScale = new Vector3(7, 7, 1);
       
        yield return new WaitForSeconds(0.5f);
        if (this.playTimeLeft > 0) StartCoroutine(Resurrection());
        else StartCoroutine(BackToMenuScene());

    }

    protected IEnumerator Resurrection()
    {
        this.currentHP = this.maxHP;
        CharManager.Instance._charSpriteRenderer.color = new Color(1, 1, 1, 1);
        this.transform.parent.position = this.savePointCtrl.CurrentSavePoint.transform.position;

        this.cameraMoving.startPoint.position = new Vector3(this.cameraMoving.mainCamera.transform.position.x, this.cameraMoving.mainCamera.transform.position.y, 0);
        this.cameraMoving.stopPoint.position = this.savePointCtrl.CurrentSavePoint.transform.position;
        StartCoroutine(this.cameraMoving.MoveCamera(2f));
        this.cameraMoving.ChangeCameraConfiner(0);

        yield return new WaitUntil(() => !this.cameraMoving.IsChangingLocation);
        Debug.Log("Resurrect Complete");
        this.cameraMoving.cinemachineCamera.Follow = this.transform.parent;
        this.isDefeated = false;
        this.charController.isDisableController = false;
        yield return new WaitForSeconds(1f);
    }

    protected IEnumerator BackToMenuScene()
    {
        yield return new WaitForSeconds(0.5f);
        Notification.Instance.Show("You Lose");
        yield return new WaitForSeconds(2f);
        this.ReturnToMenuScene();
    }

    protected virtual void ReturnToMenuScene()
    {
        ApplicationVariable.LoadingSceneName = "MenuScene";
        SceneManager.LoadScene("LoadingScene");
    }
}
