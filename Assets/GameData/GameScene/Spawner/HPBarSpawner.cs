using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarSpawner : Spawner
{
    private static HPBarSpawner instance;
    public static HPBarSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (HPBarSpawner.instance != null) Debug.LogError("Only 1 HP Bar Spawner is allowed to exist");
        HPBarSpawner.instance = this;
    }
}
