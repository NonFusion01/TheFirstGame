using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : CoreMonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] public int spawnedCount = 0;
    [SerializeField] public Transform holder;
    [SerializeField] public List<Transform> prefabs;
    [SerializeField] public List<Transform> poolObjs;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPrefab();
        this.LoadHolder();
    }

    protected virtual void LoadHolder()
    {
        if (this.holder != null) return;
        this.holder = transform.Find("Holder");
        Debug.LogWarning(transform.name + ": Load Holder", gameObject);
    }

    protected virtual void LoadPrefab()
    {
        if (this.prefabs.Count > 0) return;
        Transform prefabsObjs = transform.Find("Prefabs");
        foreach(Transform prefab in prefabsObjs)
        {
            this.prefabs.Add(prefab);
        }
        this.HidePrefab();
        Debug.LogWarning(transform.name + ": Load Prefabs", gameObject);
    }

    protected virtual void HidePrefab()
    {
        foreach(Transform prefab in prefabs)
        {
            prefab.gameObject.SetActive(false);
        }
    }


    public virtual Transform Spawn(string prefabName, Vector3 spawnPos, Quaternion spawnRot)
    {
        Transform prefab = GetPrefabByName(prefabName);
        if (prefab == null)
        {
            Debug.Log("Prefab not Found");
            return null;
        }
        return this.Spawn(prefab, spawnPos, spawnRot);
    }

    public virtual Transform Spawn(Transform prefab, Vector3 spawnPos, Quaternion spawnRot)
    {
        Transform newPrefab = this.GetObjectFromPool(prefab);
        newPrefab.SetPositionAndRotation(spawnPos, spawnRot);
        newPrefab.SetParent(this.holder);
        this.spawnedCount++;
        return newPrefab;
    }

    protected virtual Transform GetPrefabByName(string prefabName)
    {
        foreach (Transform prefab in this.prefabs)
        {
            if (prefab.name == prefabName) return prefab;
        }
        return null;
    }


    protected virtual Transform GetObjectFromPool(Transform prefab)
    {
        foreach (Transform poolObj in this.poolObjs)
        {
            if (poolObj == null) continue;
            if (poolObj.name == prefab.name) 
            {
                this.poolObjs.Remove(poolObj);
                return poolObj;
            }
        }
        Transform newPrefab = Instantiate(prefab);
        newPrefab.name = prefab.name;
        return newPrefab; 

    }

    public virtual void ReturnObjectToPool(Transform obj)
    {
        if (this.poolObjs.Contains(obj)) return;
        this.poolObjs.Add(obj);
        obj.gameObject.SetActive(false);
        this.spawnedCount--;
    }

    public virtual Transform RandomPrefabs()
    {
        int rate = (int) Random.Range(0, this.prefabs.Count);
        return this.prefabs[rate];
    }
}
