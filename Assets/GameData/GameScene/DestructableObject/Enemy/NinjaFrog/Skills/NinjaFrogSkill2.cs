using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NinjaFrogSkill2 : CoreMonoBehaviour
{
    [SerializeField] protected NinjaFrogCtrl ninjaFrogCtrl;
    [SerializeField] protected Transform skillRangeLocation;

    [SerializeField] protected Vector2 direction;
    protected float durationTime = 1f; // Duration time for the skill
    protected int castNumber = 3;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadNinjaFrogCtrl();
        this.LoadSkillRangeLocation();
    }

    protected virtual void LoadNinjaFrogCtrl()
    {
        if (this.ninjaFrogCtrl != null) return;
        this.ninjaFrogCtrl = transform.parent.GetComponentInChildren<NinjaFrogCtrl>();
        Debug.LogWarning(transform.name + ": Load Ninja Frog Ctrl", gameObject);
    }

    protected virtual void LoadSkillRangeLocation()
    {
        if (this.skillRangeLocation != null) return;
        this.skillRangeLocation = transform.parent.Find("NinjaFrogCtrl").Find("SkillLocation_3");
        Debug.LogWarning(transform.name + ": Load Skill Range Location", gameObject);
    }

    protected override void Start()
    {
        base.Start();
    }

    public IEnumerator CastSkill2() 
    {
        this.ninjaFrogCtrl.movement.isAction = true;
        this.ninjaFrogCtrl.isAbleToTakeDamage = false;
        // Dissapear on scene
        this.SpawnSmokeFX();
        this.ninjaFrogCtrl.rb.velocity = Vector2.zero;
        this.ninjaFrogCtrl.currentColor = new Vector4(this.ninjaFrogCtrl.currentColor.x, this.ninjaFrogCtrl.currentColor.y, this.ninjaFrogCtrl.currentColor.z, 0);
        this.ninjaFrogCtrl.model.color = this.ninjaFrogCtrl.currentColor;
        yield return new WaitForSeconds(1f);
        // Follow target position
        this.ninjaFrogCtrl.transform.position = new Vector2(this.ninjaFrogCtrl.target.position.x, this.ninjaFrogCtrl.target.position.y + 3);
        // Appear on scene
        this.SpawnSmokeFX();
        yield return new WaitForSeconds(0.05f);
        this.direction = this.ninjaFrogCtrl.target.position - this.ninjaFrogCtrl.transform.position;
        this.ninjaFrogCtrl.rb.velocity = new Vector2(this.direction.x, 0).normalized * 0.05f;
        this.ninjaFrogCtrl.currentColor = new Vector4(this.ninjaFrogCtrl.currentColor.x, this.ninjaFrogCtrl.currentColor.y, this.ninjaFrogCtrl.currentColor.z, 1);
        this.ninjaFrogCtrl.model.color = this.ninjaFrogCtrl.currentColor;
        this.SpawnSkillRange();
        this.castNumber--;
        yield return new WaitForSeconds(this.durationTime);
        if (this.castNumber > 0)
        {
            StartCoroutine(CastSkill2());
        }
        else
        {
            StartCoroutine(ReturnToAppearPosition());
            yield break;
        }
    }

    protected virtual void SpawnSmokeFX()
    {
        Transform newFX = FXSpawner.Instance.Spawn("Smoke_1", this.ninjaFrogCtrl.transform.position, Quaternion.identity);
        newFX.transform.localScale = Vector3.one;
        newFX.gameObject.SetActive(true);
    }

    protected virtual void SpawnSkillRange()
    {
        Transform skillRange = BulletSpawner.Instance.Spawn("BossSlashRange", this.skillRangeLocation.position, Quaternion.identity);
        skillRange.rotation = Quaternion.Euler(0, 0, -90);
        skillRange.localScale = Vector3.one;
        skillRange.gameObject.SetActive(true);

        BulletCtrl skillRangeCtrl = skillRange.GetComponent<BulletCtrl>();
        skillRangeCtrl.SetOwner(this.ninjaFrogCtrl.transform);
        skillRangeCtrl.remainingTime = this.durationTime;
        skillRangeCtrl.damage = 20;
        skillRangeCtrl.isImpactable = false;
        skillRangeCtrl.bulletRigidbody2D.gravityScale = this.ninjaFrogCtrl.rb.gravityScale;
    }

    protected IEnumerator ReturnToAppearPosition()
    {
        this.ninjaFrogCtrl.movement.isAction = true;
        this.ninjaFrogCtrl.isAbleToTakeDamage = false;
        //dissapear on scene
        this.SpawnSmokeFX();
        this.ninjaFrogCtrl.model.color = new Color(255, 255, 255, 0);
        yield return new WaitForSeconds(1f);
        //return to appear position
        int k = Random.Range(0, this.ninjaFrogCtrl.appearPositionList.Count);
        this.ninjaFrogCtrl.appearPosition = this.ninjaFrogCtrl.appearPositionList[k].position;
        this.ninjaFrogCtrl.transform.position = this.ninjaFrogCtrl.appearPosition;
        //appear on scene
        this.SpawnSmokeFX();
        this.ninjaFrogCtrl.model.color = new Color(255, 255, 255, 255);
        this.direction = this.ninjaFrogCtrl.target.position - this.ninjaFrogCtrl.transform.position;
        this.ninjaFrogCtrl.rb.velocity = new Vector2(this.direction.x, 0).normalized * 0.01f;
        //return to original state
        this.castNumber = 3;
        this.ninjaFrogCtrl.isAbleToTakeDamage = true;
        this.ninjaFrogCtrl.movement.isAction = false;
    }

    public virtual void ResetSkill()
    {
        this.castNumber = 3;
    }
}
