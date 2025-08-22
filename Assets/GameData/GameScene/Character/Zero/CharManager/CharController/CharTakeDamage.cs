using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharTakeDmg : CoreMonoBehaviour
{
    [Header("Take Damage")]
    [SerializeField] protected CharController charCtrl;
    public bool isAbleToTakeDmg = true;
    public bool isTakingDmg = false;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharCtrl();
    }

    protected virtual void LoadCharCtrl()
    {
        if (this.charCtrl != null) return;
        this.charCtrl = transform.parent.GetComponentInChildren<CharController>();
        Debug.LogWarning(transform.name + ": Load Char Ctrl", gameObject);
    }

    public virtual void CharTakingDamage(int damage)
    {
        if (GameManagerScript.isGamePaused) return;
        if (this.charCtrl.isDisableController) return;

        if (!this.isAbleToTakeDmg) return;
        if (charCtrl.charSkillSelection.isUsingSkill) return;
        this.isAbleToTakeDmg = false;
        this.isTakingDmg = true;
        CharManager.Instance._charRigidbody2D.velocity = Vector3.zero;
        CharManager.Instance._charRigidbody2D.gravityScale = 0;
        CharManager.Instance._charStats.currentHP -= damage;
        StartCoroutine(TakeDamage());
    }

    protected IEnumerator TakeDamage()
    {
        this.charCtrl.isActionOcurr = true;
        this.charCtrl.charAniCtrl.NonLoopAnimationPlay("TakeDmgAnimation");
        yield return new WaitForSeconds(0.5f);
        this.charCtrl.isActionOcurr = false;
        this.isTakingDmg = false;
        CharManager.Instance._charRigidbody2D.gravityScale = this.charCtrl.basicGravityScale;
        StartCoroutine(WaitForEnableTakeDmg(1f));
    }

    protected IEnumerator WaitForEnableTakeDmg(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            this.isAbleToTakeDmg = false;
            elapsedTime += Time.deltaTime;
            if (elapsedTime > time) elapsedTime = time;
            yield return null;
        }
        if (elapsedTime == time)
        {
            if (this.charCtrl.charSkillSelection.SkillList[2].isInSkillDuration) yield break;
            this.isAbleToTakeDmg = true;
            yield break;
        }
    }
}
