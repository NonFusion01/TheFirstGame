using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaFrogSkill1 : CoreMonoBehaviour
{
    [SerializeField] protected NinjaFrogCtrl ninjaFrogCtrl;
    [SerializeField] protected Transform skillRangeLocation;
    [SerializeField] protected List<Transform> bossSpawnLocations;
    protected Vector2 direction;
    protected float durationTime = 1f; // Duration time for the skill
    protected int castNumber = 3;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadNinjaFrogCtrl();
        this.LoadSkillRangeLocation();
        this.LoadBossSpawnLocation();
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
        this.skillRangeLocation = transform.parent.Find("NinjaFrogCtrl").Find("SkillLocation_1");
        Debug.LogWarning(transform.name + ": Load Skill Range Location", gameObject);
    }

    protected virtual void LoadBossSpawnLocation()
    {
        if (this.bossSpawnLocations.Count > 0) return;
        foreach (Transform transform in this.transform)
        {
            this.bossSpawnLocations.Add(transform);
        }
        Debug.LogWarning(transform.name + ": Load Skill Location", gameObject);
    }

    public IEnumerator CastSkill1()
    {
        this.ninjaFrogCtrl.movement.isAction = true;
        this.ninjaFrogCtrl.isAbleToTakeDamage = false;
        //dissapear on scene
        this.SpawnSmokeFX();
        this.ninjaFrogCtrl.rb.velocity = Vector2.zero;
        this.ninjaFrogCtrl.currentColor = new Vector4(this.ninjaFrogCtrl.currentColor.x, this.ninjaFrogCtrl.currentColor.y, this.ninjaFrogCtrl.currentColor.z, 0);
        this.ninjaFrogCtrl.model.color = this.ninjaFrogCtrl.currentColor;
        this.ninjaFrogCtrl.PlayLoopAnimation("NinjaFrogRunAnimation");
        //choose random spawn location
        yield return new WaitForSeconds(1f);
        int k = Random.Range(0, this.bossSpawnLocations.Count);
        Vector2 skillLocation = new Vector2(this.bossSpawnLocations[k].position.x, this.ninjaFrogCtrl.target.position.y);
        this.ninjaFrogCtrl.transform.position = skillLocation;
        //appear on scene
        this.SpawnSmokeFX();
        this.ninjaFrogCtrl.currentColor = new Vector4(this.ninjaFrogCtrl.currentColor.x, this.ninjaFrogCtrl.currentColor.y, this.ninjaFrogCtrl.currentColor.z, 1);
        this.ninjaFrogCtrl.model.color = this.ninjaFrogCtrl.currentColor;
        this.ninjaFrogCtrl.isAbleToTakeDamage = true;
        //cast skill
        this.ninjaFrogCtrl.rb.gravityScale = 0;
        this.direction = this.ninjaFrogCtrl.target.position - this.ninjaFrogCtrl.transform.position;
        this.ninjaFrogCtrl.rb.velocity = new Vector2(this.direction.x, 0).normalized * 7f;
        yield return new WaitForSeconds(0.1f);
        this.SpawnSkillRange(this.direction);

        this.castNumber--;
        yield return new WaitForSeconds(this.durationTime);
        if (this.castNumber > 0)
        {
            StartCoroutine(CastSkill1());
        }
        else
        {
            this.ninjaFrogCtrl.rb.gravityScale = 1;
            this.castNumber = 3;
            this.ninjaFrogCtrl.rb.velocity = Vector2.zero;
            StartCoroutine(ReturnToAppearPosition());
            yield break;
        }
    }

    protected IEnumerator ReturnToAppearPosition()
    {
        this.ninjaFrogCtrl.movement.isAction = true;
        this.ninjaFrogCtrl.isAbleToTakeDamage = false;
        //dissapear on scene
        this.SpawnSmokeFX();
        this.ninjaFrogCtrl.currentColor = new Vector4(this.ninjaFrogCtrl.currentColor.x, this.ninjaFrogCtrl.currentColor.y, this.ninjaFrogCtrl.currentColor.z, 0);
        this.ninjaFrogCtrl.model.color = this.ninjaFrogCtrl.currentColor;
        yield return new WaitForSeconds(1f);
        //return to appear position
        int k = Random.Range(0, this.ninjaFrogCtrl.appearPositionList.Count);
        this.ninjaFrogCtrl.appearPosition = this.ninjaFrogCtrl.appearPositionList[k].position;
        this.ninjaFrogCtrl.transform.position = this.ninjaFrogCtrl.appearPosition;
        //appear on scene
        this.SpawnSmokeFX();
        this.ninjaFrogCtrl.currentColor = new Vector4(this.ninjaFrogCtrl.currentColor.x, this.ninjaFrogCtrl.currentColor.y, this.ninjaFrogCtrl.currentColor.z, 1);
        this.ninjaFrogCtrl.model.color = this.ninjaFrogCtrl.currentColor;
        //return to original state
        this.ninjaFrogCtrl.isAbleToTakeDamage = true;
        this.ninjaFrogCtrl.movement.isAction = false;
        this.direction = this.ninjaFrogCtrl.target.position - this.ninjaFrogCtrl.transform.position;
        this.ninjaFrogCtrl.rb.velocity = new Vector2(this.direction.x, 0).normalized * 0.01f;
    }

    protected virtual void SpawnSmokeFX()
    {
        Transform newFX = FXSpawner.Instance.Spawn("Smoke_1", this.ninjaFrogCtrl.transform.position, Quaternion.identity);
        newFX.transform.localScale = Vector3.one;
        newFX.gameObject.SetActive(true);
    }

    protected virtual void SpawnSkillRange(Vector2 direction)
    {
        Transform skillRange = BulletSpawner.Instance.Spawn("BossSlashRange", this.skillRangeLocation.position, Quaternion.identity);
        skillRange.rotation = Quaternion.Euler(0, 0, 0);
        Vector3 newVector = new Vector3(direction.x, 0, 0).normalized;
        skillRange.localScale = new Vector3(newVector.x, 1, 1);
        skillRange.gameObject.SetActive(true);

        BulletCtrl skillRangeCtrl = skillRange.GetComponent<BulletCtrl>();
        skillRangeCtrl.SetOwner(this.ninjaFrogCtrl.transform);
        skillRangeCtrl.remainingTime = this.durationTime;
        skillRangeCtrl.damage = 20;
        skillRangeCtrl.isImpactable = false;
        skillRangeCtrl.bulletRigidbody2D.gravityScale = this.ninjaFrogCtrl.rb.gravityScale;
        skillRangeCtrl.bulletRigidbody2D.velocity = this.ninjaFrogCtrl.rb.velocity;
    }

    public virtual void ResetSkill()
    {
        this.castNumber = 3;
    }
}
