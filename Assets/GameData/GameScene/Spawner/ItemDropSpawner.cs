using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropSpawner : Spawner
{
    private static ItemDropSpawner instance;
    public static ItemDropSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (ItemDropSpawner.instance != null) Debug.LogError("Only 1 ItemDropSpawner is allowed to exist");
        ItemDropSpawner.instance = this;
    }

    public virtual Transform RandomPrefabByRate()
    {
        float rateValue = 0;
        foreach (Transform prefab in this.prefabs)
        {
            Item item = prefab.GetComponent<Item>();
            rateValue += item.DropRate;
        }

        float rate = Random.Range(0, rateValue);
        float maxRate = 0;
        foreach (Transform prefab in this.prefabs)
        {
            Item item = prefab.GetComponent<Item>();
            maxRate += item.DropRate;
            if (rate < maxRate) return prefab;
        }
        return null;
    }
}
