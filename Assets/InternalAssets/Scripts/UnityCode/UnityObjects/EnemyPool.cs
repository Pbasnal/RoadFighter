using Assets.Scripts.UnityLogic.CommonInterface;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    [CreateAssetMenu(menuName = "AI/EnemyPool", fileName = "EnemyPool", order = 51)]
    public class EnemyPool : ScriptableObject, IObjectPool<Enemy>
    {
        public GameObject prefab;

        private IDictionary<string, Enemy> activeObjectMap = new Dictionary<string, Enemy>();
        private ConcurrentQueue<Enemy> inactiveObjects = new ConcurrentQueue<Enemy>();

        public void AddInactiveObject(Enemy enemy)
        {
            inactiveObjects.Enqueue(enemy);
        }

        public void AddActiveObject(Enemy enemy)
        {
            if (!activeObjectMap.ContainsKey(enemy.Name))
            {
                activeObjectMap.Add(enemy.Name, enemy);
            }
        }

        public void DeactivateObject(Enemy enemy)
        {
            if (!activeObjectMap.ContainsKey(enemy.Name))
            {
                //Debug.Log("object doesn't exists");
                return;
            }

            var poolObject = activeObjectMap[enemy.Name];
            inactiveObjects.Enqueue(poolObject);
        }

        public Enemy RecycleObject()
        {
            Enemy poolObject;
            inactiveObjects.TryDequeue(out poolObject);
            if (poolObject == null)
            {
                return null;
            }

            if (!activeObjectMap.ContainsKey(poolObject.Name))
            {
                activeObjectMap.Add(poolObject.Name, poolObject);
            }

            return poolObject;
        }
    }
}
