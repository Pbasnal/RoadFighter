using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObject Pool", menuName = "Level/Pool", order = 52)]
public class GameObjectPool : ScriptableObject
{
    private IDictionary<string, Queue<GameObject>> activeCarsMap;
    private IDictionary<string, Queue<GameObject>> inactiveCarsMap;

    public GameObject[] objectGameObjectypes { get; private set; }
    private GameObject mostRecentActiveObject { get; set; }

    public void Init(GameObject[] gameObjects)
    {
        activeCarsMap = new Dictionary<string, Queue<GameObject>>();
        inactiveCarsMap = new Dictionary<string, Queue<GameObject>>();
        objectGameObjectypes = new GameObject[gameObjects.Length];

        int i = 0;
        for (int j = 0; j < gameObjects.Length; j++)
        {
            if (!inactiveCarsMap.ContainsKey(gameObjects[j].name))
            {
                inactiveCarsMap.Add(gameObjects[j].name, new Queue<GameObject>());
                activeCarsMap.Add(gameObjects[j].name, new Queue<GameObject>());
                objectGameObjectypes[i++] = gameObjects[j];
            }
            inactiveCarsMap[gameObjects[j].name].Enqueue(gameObjects[j]);
        }
    }

    public void AddInactiveObject(string objectType, GameObject gameObject)
    {
        if (!inactiveCarsMap.ContainsKey(objectType))
        {
            inactiveCarsMap.Add(objectType, new Queue<GameObject>());
        }
        inactiveCarsMap[objectType].Enqueue(gameObject);
    }

    public void AddActiveObject(string objectype, GameObject gameObject)
    {
        if (!activeCarsMap.ContainsKey(objectype))
        {
            activeCarsMap.Add(objectype, new Queue<GameObject>());
        }
        activeCarsMap[objectype].Enqueue(gameObject);

        if (activeCarsMap[objectype].Count == 0)
        {
            mostRecentActiveObject = null;
        }
        else
        {
            mostRecentActiveObject = activeCarsMap[objectype].Peek();
        }
    }

    public GameObject DeactivateTopObject(string objectType)
    {
        if (activeCarsMap[objectType].Count == 0)
        {
            mostRecentActiveObject = null;
            return null;
        }
        var poolObject = activeCarsMap[objectType].Dequeue();
        inactiveCarsMap[objectType].Enqueue(poolObject);

        if (activeCarsMap[objectType].Count == 0)
        {
            mostRecentActiveObject = null;
        }
        else
        {
            mostRecentActiveObject = activeCarsMap[objectType].Peek();
        }

        return poolObject;
    }

    public GameObject RecycleObject(string objectType)
    {
        if (inactiveCarsMap[objectType].Count == 0)
        {
            return null;
        }
        var poolObject = inactiveCarsMap[objectType].Dequeue();
        activeCarsMap[objectType].Enqueue(poolObject);

        if (activeCarsMap[objectType].Count == 0)
        {
            mostRecentActiveObject = null;
        }
        else
        {
            mostRecentActiveObject = activeCarsMap[objectType].Peek();
        }
        return poolObject;
    }
}
