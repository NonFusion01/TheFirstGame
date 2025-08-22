using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaFrogSkill3 : CoreMonoBehaviour
{
    [SerializeField] protected NinjaFrogCtrl ninjaFrogCtrl;
    [SerializeField] protected Transform skillLocation1;
    [SerializeField] protected Transform skillLocation2;
    protected Vector3 directionRight = new Vector3(1, 0, 0);
    protected Vector3 directionLeft = new Vector3(-1, 0, 0);
    public Vector2 direction;


    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadNinjaFrogCtrl();
        this.LoadSkillLocation();
    }

    protected virtual void LoadNinjaFrogCtrl()
    {
        if (this.ninjaFrogCtrl != null) return;
        this.ninjaFrogCtrl = transform.parent.GetComponentInChildren<NinjaFrogCtrl>();
        Debug.LogWarning(transform.name + ": Load Ninja Frog Ctrl", gameObject);
    }

    protected virtual void LoadSkillLocation()
    {
        if (this.skillLocation1 != null && this.skillLocation2 != null) return;
        this.skillLocation1 = transform.parent.Find("NinjaFrogCtrl").Find("SkillLocation_1");
        this.skillLocation2 = transform.parent.Find("NinjaFrogCtrl").Find("SkillLocation_2");
        Debug.LogWarning(transform.name + ": Load Skill Location", gameObject);
    }

    protected override void Start()
    {
        base.Start();
    }

    public IEnumerator CastSkill3()
    {
        yield return new WaitForSeconds(1f);
        Transform newFX1 = FXSpawner.Instance.Spawn("Smoke_1", skillLocation1.position, Quaternion.identity);
        newFX1.transform.localScale = Vector3.one;
        newFX1.gameObject.SetActive(true);

        Transform clone1 = BulletSpawner.Instance.Spawn("Saw", skillLocation1.position, Quaternion.identity);
        clone1.gameObject.SetActive(true);
        clone1.localScale = Vector3.one * this.ninjaFrogCtrl.movement.localScaleL.x;
        BulletCtrl sawCtrl1 = clone1.GetComponent<BulletCtrl>();
        this.SetupBulletProperties(sawCtrl1, this.ninjaFrogCtrl.movement.moveDirection);

        Transform newFX2 = FXSpawner.Instance.Spawn("Smoke_1", skillLocation2.position, Quaternion.identity);
        newFX2.transform.localScale = Vector3.one;
        newFX2.gameObject.SetActive(true);

        Transform clone2 = BulletSpawner.Instance.Spawn("Saw", skillLocation2.position, Quaternion.identity);
        clone2.gameObject.SetActive(true);
        clone2.localScale = Vector3.one * this.ninjaFrogCtrl.movement.localScaleR.x;
        BulletCtrl sawCtrl2 = clone2.GetComponent<BulletCtrl>();
        this.SetupBulletProperties(sawCtrl2, -this.ninjaFrogCtrl.movement.moveDirection);
    }

    protected virtual void SetupBulletProperties(BulletCtrl bulletCtrl, Vector3 direction)
    {
        bulletCtrl.SetOwner(this.ninjaFrogCtrl.transform);
        bulletCtrl.remainingTime = 5f; // Set the bullet's lifetime
        bulletCtrl.damage = 5;
        bulletCtrl.isImpactable = true;
        bulletCtrl.bulletRigidbody2D.velocity = direction * 1f;
    }
}
