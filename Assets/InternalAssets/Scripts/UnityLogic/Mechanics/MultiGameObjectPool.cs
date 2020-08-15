using System.Collections.Generic;
using UnityEngine;
using UnityLogic.CommonInterface;

namespace Assets.InternalAssets.Scripts.UnityLogic.Mechanics
{
    [CreateAssetMenu(menuName = "Pool/MultiGameObjectPool", fileName = "MultiGameObjectPool", order = 51)]
    public class MultiGameObjectPool : ScriptableObject
    {
        public int countToBeKeptForAllObjects;
        public GameObject[] objectsToPool; // needs to be gameObject because it has to hold prefabs

        private IDictionary<string, GameObjectPool> gameObjectPools;

        public IEnumerable<GameObject> InitializePools()
        {
            gameObjectPools = new Dictionary<string, GameObjectPool>();

            foreach (var objectToPool in objectsToPool)
            {
                var poolBehaviour = objectToPool.GetComponent<ICanBePooled>();
                if (poolBehaviour == null)
                {
                    continue;
                }

                if (gameObjectPools.ContainsKey(poolBehaviour.Id))
                {
                    continue;
                }

                var pool = new GameObjectPool
                {
                    poolableObject = poolBehaviour
                };
                foreach (var pooledObject in pool.AddObjectsToPool(countToBeKeptForAllObjects))
                {
                    yield return pooledObject;
                }
                
                gameObjectPools.Add(poolBehaviour.Id, pool);
            }
        }

        public void DeactivateObject(ICanBePooled car)
        {
            if (gameObjectPools.ContainsKey(car.Id))
            {
                gameObjectPools[car.Id].DeactivateObject(car);
            }
        }

        public void UnlockObject(ICanBePooled car)
        {
            if (!gameObjectPools.ContainsKey(car.Id))
            {
                return;
            }

            gameObjectPools[car.Id].UnlockObject(car);
        }

        public ICanBePooled LockObjectForRecycling(string poolObjectId)
        {
            if (!gameObjectPools.ContainsKey(poolObjectId))
            {
                return null;
            }

            return gameObjectPools[poolObjectId].LockObjectForRecycling();
        }

        public ICanBePooled RecycleObject(string poolObjectId)
        {
            if (gameObjectPools.ContainsKey(poolObjectId))
            {
                return gameObjectPools[poolObjectId].RecycleObject();
            }

            return null;
        }
    }
}
