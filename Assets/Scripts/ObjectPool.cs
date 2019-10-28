using System;
using System.Collections.Generic;
using UnityEngine;

public enum Pools { m249Bullet, bulletIcon, robotBullet, explodeA, bulletCartridge };

public class ObjectPool : MonoBehaviour
{
    public GameObject[] prefabs;
    private Dictionary<string, Queue<GameObject>> dictOfPools;
    private Dictionary<string, GameObject> dictOfPrefabs;

    public static ObjectPool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        dictOfPools = new Dictionary<string, Queue<GameObject>>();
        dictOfPrefabs = new Dictionary<string, GameObject>();

        foreach (GameObject prefab in prefabs)
        {
            dictOfPools.Add(prefab.name, new Queue<GameObject>());
            dictOfPrefabs.Add(prefab.name, prefab);
        }

        foreach (Pools item in Enum.GetValues(typeof(Pools)))
        {
            GrowPool(item);
        }
    }

    private void GrowPool(Pools poolName)
    {
        for (int i = 0; i < 20; i++)
        {
            var instance = Instantiate(dictOfPrefabs[poolName.ToString()]);
            instance.name = poolName.ToString();
            instance.transform.SetParent(transform);
            DeactivateAndAddToPool(instance);
        }
    }

    public GameObject GetFromPoolInactive(Pools poolName)
    {
        Queue<GameObject> pool = dictOfPools[poolName.ToString()];
        if (pool.Count == 0)
            GrowPool(poolName);
        var instance = pool.Dequeue();
        return instance;
    }

    public void DeactivateAndAddToPool(GameObject instance)
    {
        instance.SetActive(false);
        Queue<GameObject> pool = dictOfPools[instance.name];
        pool.Enqueue(instance);
    }
}
