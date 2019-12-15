using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Road Section Pool", menuName = "Pool/RoadSectionPool", order = 52)]
public class RoadSectionPool : ScriptableObject, IObjectPool<IRoadSection>
{
    private IDictionary<string, IRoadSection> activeObjectMap = new Dictionary<string, IRoadSection>();
    private ConcurrentQueue<IRoadSection> inactiveObjects = new ConcurrentQueue<IRoadSection>();

    public void AddInactiveObject(IRoadSection poolObject)
    {
        inactiveObjects.Enqueue(poolObject);
    }
    public void AddInactiveObjects(IRoadSection[] poolObjects)
    {
        foreach (var poolObject in poolObjects)
        {
            AddInactiveObject(poolObject);
        }
    }

    public bool AddActiveObject(IRoadSection poolObject)
    {
        if (activeObjectMap.ContainsKey(poolObject.name))
        {
            return false;
        }

        activeObjectMap.Add(poolObject.name, poolObject);
        return true;
    }

    public bool DeactivateObject(IRoadSection poolObject)
    {
        if (!activeObjectMap.ContainsKey(poolObject.name))
        {
            Debug.Log("object doesn't exists");
            return false;
        }

        poolObject = activeObjectMap[poolObject.name];
        inactiveObjects.Enqueue(poolObject);

        return true;
    }

    public bool RecycleObject(out IRoadSection recycledObject)
    {
        if (!inactiveObjects.TryDequeue(out recycledObject))
        {
            return false;
        }

        if (!activeObjectMap.ContainsKey(recycledObject.name))
        {
            activeObjectMap.Add(recycledObject.name, recycledObject);
        }

        return true;
    }
}