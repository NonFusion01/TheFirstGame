using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NinjaFrogCtrl : EnemyBossCtrl
{
    [Header("Ninja Frog")]
    [SerializeField] public NinjaFrogMovement movement;
    [SerializeField] public NinjaFrogSkills skills;
    [SerializeField] public List<Transform> appearPositionList;
    public Vector3 appearPosition;
    public bool useBehaviour = false;

    [SerializeField] protected CharController player;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMovement();
        this.LoadSkills();
        this.LoadAppearPositions();
        this.LoadPlayer();
        this.LoadInitialStats();
    }

    protected override void LoadCollider()
    {
        base.LoadCollider();
        Physics.IgnoreLayerCollision(12, 14);
        Physics2D.IgnoreLayerCollision(12, 14);
        Physics.IgnoreLayerCollision(14, 14);
        Physics2D.IgnoreLayerCollision(14, 14);
    }

    protected virtual void LoadMovement()
    {
        if (this.movement != null) return;
        this.movement = GetComponentInChildren<NinjaFrogMovement>();
        Debug.LogWarning(transform.name + ": Load Movement", gameObject);
    }

    protected virtual void LoadSkills()
    {
        if (this.skills != null) return;
        this.skills = GetComponent<NinjaFrogSkills>();
        Debug.LogWarning(transform.name + ": Load Skills", gameObject);
    }

    protected virtual void LoadAppearPositions()
    {
        if (this.appearPositionList.Count > 0) return;
        Transform appearPositions = transform.parent.Find("AppearPositions");
        foreach (Transform position in appearPositions)
        {
            this.appearPositionList.Add(position);
        }
        Debug.LogWarning(transform.name + ": Load Appear Positions", gameObject);
        this.appearPosition = this.appearPositionList[0].position;

    }

    protected virtual void LoadPlayer()
    {
        if (this.player != null) return;
        this.player = GameObject.Find("Player").GetComponent<CharController>();
        Debug.LogWarning(transform.name + ": Load Player", gameObject);
    }

    protected virtual void LoadInitialStats()
    {
        this.maxHp = 300;
        this.hp = this.maxHp;

    }

    protected override void Start()
    {
        base.Start();
        this.appearPosition = this.appearPositionList[0].position;
        this.transform.position = this.appearPosition;
        this.transform.parent.gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(BossAppear());
    }

    protected IEnumerator BossAppear()
    {
        this.isAbleToTakeDamage = false;
        this.model.color = new Color(currentColor.x, currentColor.y, currentColor.z, 0); //hide boss on scene
        yield return new WaitForSeconds(2.5f);
        Notification.Instance.Show("WARNING");
        yield return new WaitForSeconds(3f);
        Transform newFX = FXSpawner.Instance.Spawn("Smoke_1", this.transform.position, Quaternion.identity);
        newFX.gameObject.SetActive(true);
        this.model.color = new Color(currentColor.x, currentColor.y, currentColor.z, 1);
        yield return new WaitForSeconds(1f);
        this.SetHPBar();
        this.useBehaviour = true;
        this.isAbleToTakeDamage = true;
        StartCoroutine(NinjaFrogBehaviour());
    }

    protected virtual void SetHPBar()
    {
        this.bossHPBarCtrl.gameObject.SetActive(true);
        this.bossHPBarCtrl.enemyBossCtrl = this;
        this.bossHPBarCtrl.hp = this.hp;
        this.bossHPBarCtrl.maxHp = this.maxHp;
    }

    // reset boss appearance and stats when player is defeated
    public virtual void ResetBossStatus()
    {
        this.transform.localScale = this.movement.localScaleL;
        this.rb.velocity = Vector2.zero;
        this.appearPosition = this.appearPositionList[0].position;
        this.transform.position = this.appearPosition;
        this.hp = this.maxHp;
        this.rb.gravityScale = 1;
        this.movement.isAction = false;
        this.isAbleToTakeDamage = true;
        this.skills.Skill1.ResetSkill();
        this.skills.Skill2.ResetSkill();
        this.bossHPBarCtrl.gameObject.SetActive(false);
        this.transform.parent.gameObject.SetActive(false);
    }

    protected virtual void FixedUpdate()
    {
        if (CharManager.Instance._charStats.currentHP <= 0) this.ResetBossStatus();
        if (this.hp < 0) this.hp = 0;
        if (this.hp == 0 && this.useBehaviour)
        {
            this.useBehaviour = false;
            StopAllCoroutines();
            StartCoroutine(DefeatedState());
            return;
        }
    }

    protected IEnumerator NinjaFrogBehaviour()
    {
        if (!this.useBehaviour)
        {
            yield break;
        }
        if (this.appearPosition == this.appearPositionList[0].position)
        {
            StartCoroutine(this.movement.MoveLeft(2f));
            yield return new WaitForSeconds(1f);
            StartCoroutine(this.movement.MoveRight(1f));
            yield return new WaitForSeconds(1f);
            StartCoroutine(this.movement.MoveLeft(3f));
        }
        if (this.appearPosition == this.appearPositionList[1].position)
        {
            StartCoroutine(this.movement.MoveRight(2f));
            yield return new WaitForSeconds(1f);
            StartCoroutine(this.movement.MoveLeft(1f));
            yield return new WaitForSeconds(1f);
            StartCoroutine(this.movement.MoveRight(3f));
        }
        StartCoroutine(this.skills.Skill3.CastSkill3());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(this.skills.Skill3.CastSkill3());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(this.movement.Jump());
        yield return new WaitForSeconds(2f);
        StartCoroutine(this.skills.Skill3.CastSkill3());
        yield return new WaitForSeconds(3f);
        StartCoroutine(this.skills.Skill1.CastSkill1());
        yield return new WaitForSeconds(9f);
        StartCoroutine(this.skills.Skill2.CastSkill2());
        yield return new WaitForSeconds(9f);
        StartCoroutine(NinjaFrogBehaviour());
        yield break;
    }

    protected IEnumerator DefeatedState()
    {
        this.isAbleToTakeDamage = false;
        this.rb.velocity = Vector2.zero;
        this.model.color = new Color(currentColor.x, currentColor.y, currentColor.z, 0);
        this.player.charTakeDamage.isAbleToTakeDmg = false;
        yield return new WaitForSeconds(1f);
        this.player.isActionOcurr = false;
        this.player.isDisableController = true;
        yield return new WaitForSeconds(2f);
        Notification.Instance.Show("You Win");
        yield return new WaitForSeconds(2f);
        this.ReturnToMenuScene();
    }

    protected virtual void ReturnToMenuScene()
    {
        ApplicationVariable.LoadingSceneName = "MenuScene";
        SceneManager.LoadScene("LoadingScene");
    }
}
