using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityLogic.CommonInterface;

namespace Assets.InternalAssets.Scripts.UnityLogic.Mechanics
{
    // Scriptable object
    public class GameObjectPool
    {
        public ICanBePooled poolableObject;

        private IDictionary<string, ICanBePooled> allPoolObjects 
            = new Dictionary<string, ICanBePooled>();

        private ConcurrentQueue<ICanBePooled> inactiveObjects 
            = new ConcurrentQueue<ICanBePooled>();

        private ConcurrentQueue<ICanBePooled> waitingObjects 
            = new ConcurrentQueue<ICanBePooled>();

        public IEnumerable<GameObject> AddObjectsToPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                // todo: check if spawning will add all the other scripts to the 
                // gameobject or not
                var poolObject = poolableObject.SpawnANewInstance();
                if (allPoolObjects.ContainsKey(poolObject.Name))
                {
                    continue;
                }
                inactiveObjects.Enqueue(poolObject);
                allPoolObjects.Add(poolObject.Name, poolObject);

                yield return poolableObject.SpawnedObject;
            }
        }

        public void DeactivateObject(ICanBePooled car)
        {
            if (!allPoolObjects.ContainsKey(car.Name) || !car.IsActive)
            {
                //Debug.Log("object doesn't exists");
                return;
            }

            car.SetActive(false);
            inactiveObjects.Enqueue(car);
        }

        public void UnlockObject(ICanBePooled car)
        {
            if (!allPoolObjects.ContainsKey(car.Name))
            {
                //Debug.Log("object doesn't exists");
                return;
            }
            if (!waitingObjects.TryDequeue(out car))
            {
                return;
            }

            inactiveObjects.Enqueue(car);
        }

        public ICanBePooled LockObjectForRecycling()
        {
            inactiveObjects.TryDequeue(out ICanBePooled poolObject);
            if (poolObject == null)
            {
                return null;
            }

            waitingObjects.Enqueue(poolObject);
            return poolObject;
        }

        public ICanBePooled RecycleObject()
        {
            if (waitingObjects.TryDequeue(out ICanBePooled poolObject) 
                || inactiveObjects.TryDequeue(out poolObject))
            {
                poolObject.SetActive(true);
                return poolObject;
            }

            return null;
        }
    }
}
