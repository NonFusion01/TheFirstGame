using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSpawner : Spawner
{
    protected static FXSpawner instance;
    public static FXSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (FXSpawner.instance != null) Debug.LogError("Only 1 FX Spawner is allowed to exist");
        FXSpawner.instance = this;
    }
}
