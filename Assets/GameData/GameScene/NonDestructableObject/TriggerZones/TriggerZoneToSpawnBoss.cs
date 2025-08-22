using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneToSpawnBoss : CoreMonoBehaviour
{
    [SerializeField] protected TriggerZoneCtrl triggerZoneCtrl;
    [SerializeField] protected EnemyBossCtrl enemyBossCtrl;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadTriggerZoneCtrl();
        this.LoadEnemyBossCtrl();
    }

    protected virtual void LoadTriggerZoneCtrl()
    {
        if (this.triggerZoneCtrl != null) return;
        this.triggerZoneCtrl = GetComponent<TriggerZoneCtrl>();
        Debug.LogWarning(transform.name + ": Load Trigger Zone Ctrl", gameObject);
    }

    protected virtual void LoadEnemyBossCtrl()
    {
        if (this.enemyBossCtrl != null) return;
        this.enemyBossCtrl = GameObject.Find("EnemyBoss").GetComponentInChildren<EnemyBossCtrl>();
        Debug.LogWarning(transform.name + ": Load Enemy Boss Ctrl", gameObject);
    }

    public virtual void BossSpawn()
    {
        this.enemyBossCtrl.transform.parent.gameObject.SetActive(true);
    }
}
