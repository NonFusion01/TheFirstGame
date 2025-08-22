using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DistanceFromPlayer : CoreMonoBehaviour
{
    [SerializeField] protected Transform player;
    [SerializeField] protected bool isPlayerInCamera = false;
    [SerializeField] protected bool isPlayerInDesignatedZone = true;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayer();
       
    }

    protected virtual void LoadPlayer()
    {
        if (this.player != null) return;
        this.player = GameObject.Find("PlayerCenter").transform;
        Debug.LogWarning(transform.name + ": Load Player", gameObject);
    }

    protected virtual void Update()
    {
        this.CheckPlayerInCamera();
    }


    protected virtual void CheckPlayerInCamera()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(player.position);
        if (viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1)
        {
            this.isPlayerInCamera = true;
        }
        else
        {
            this.isPlayerInCamera = false;
        }
        if (viewportPos.x > -0.1f && viewportPos.x < 1.1f && viewportPos.y > -0.1f && viewportPos.y < 1.1f)
        {
            this.isPlayerInDesignatedZone = true;
        }
        else
        {
            this.isPlayerInDesignatedZone = false;
            if (CharManager.Instance._charStats.inInvincibleState) return;
            if (!CharManager.Instance._charStats.IsDefeated) CharManager.Instance._charStats.currentHP = 0;
        }
    }
}
