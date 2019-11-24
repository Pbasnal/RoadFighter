﻿using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObject Pool2", menuName = "Level/Pool2", order = 52)]
public class GameObjectPool2 : ScriptableObject
{
    public GameObject prefab;
    public int startingPoolCount;

    private IDictionary<string, GameObject> activeObjectMap = new Dictionary<string, GameObject>();
    private Queue<GameObject> inactiveObjects = new Queue<GameObject>();

    public void Init()
    {
        for (int i = 0; i < startingPoolCount; i++)
        {
            inactiveObjects.Enqueue(GetNewInActiveObject());
        }
    }

    public void AddInactiveObject(string objectType, GameObject gameObject)
    {
        inactiveObjects.Enqueue(gameObject);
    }

    public void AddActiveObject(string objectype, GameObject gameObject)
    {
        if (!activeObjectMap.ContainsKey(gameObject.name))
        {
            activeObjectMap.Add(gameObject.name, gameObject);
        }
    }

    public void DeactivateObject(GameObject gameObject)
    {
        if (!activeObjectMap.ContainsKey(gameObject.name))
        {
            Debug.Log("object doesn't exists");
            return;
        }

        var poolObject = activeObjectMap[gameObject.name];
        poolObject.SetActive(false);
        inactiveObjects.Enqueue(poolObject);
    }

    public GameObject RecycleObject()
    {
        GameObject poolObject;
        if (inactiveObjects.Count == 0)
        {
            inactiveObjects.Enqueue(GetNewInActiveObject());
        }

        poolObject = inactiveObjects.Dequeue();
        if (!activeObjectMap.ContainsKey(poolObject.name))
        {
            activeObjectMap.Add(poolObject.name, poolObject);
        }
        poolObject.SetActive(true);

        return poolObject;
    }

    public GameObject RecycleObjectAt(Vector3 position)
    {
        GameObject poolObject;
        if (inactiveObjects.Count == 0)
        {
            inactiveObjects.Enqueue(GetNewInActiveObject());
        }

        poolObject = inactiveObjects.Dequeue();
        if (!activeObjectMap.ContainsKey(poolObject.name))
        {
            activeObjectMap.Add(poolObject.name, poolObject);
        }
        poolObject.transform.position = position;
        poolObject.SetActive(true);

        return poolObject;
    }

    private GameObject GetNewInActiveObject()
    {
        var poolObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        poolObject.name = Guid.NewGuid().ToString();
        poolObject.SetActive(false);

        return poolObject;
    }
}
