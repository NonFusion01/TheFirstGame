using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : Spawner
{
    protected static BulletSpawner instance;
    public static BulletSpawner Instance => instance;

    public static string normalBullet = "Bullet_1";
    protected override void Awake()
    {
        base.Awake();
        if (BulletSpawner.instance != null) Debug.LogError("Only 1 Bullet Spawner is allowed to exist");
        BulletSpawner.instance = this;
    }
}
