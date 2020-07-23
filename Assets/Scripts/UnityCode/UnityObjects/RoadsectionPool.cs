using Assets.Scripts.UnityLogic.CommonInterface;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    [CreateAssetMenu(menuName = "Road/RoadsectionPool", fileName = "RoadsectionPool", order = 51)]
    public class RoadsectionPool : ScriptableObject, IObjectPool<Roadsection>
    {
        public GameObject prefab;

        private IDictionary<string, Roadsection> activeObjectMap = new Dictionary<string, Roadsection>();
        private ConcurrentQueue<Roadsection> inactiveObjects = new ConcurrentQueue<Roadsection>();

        public void AddInactiveObject(string objectType, Roadsection enemy)
        {
            inactiveObjects.Enqueue(enemy);
        }

        public void AddActiveObject(string objectype, Roadsection enemy)
        {
            if (!activeObjectMap.ContainsKey(enemy.name))
            {
                activeObjectMap.Add(enemy.name, enemy);
            }
        }

        public void DeactivateObject(Roadsection enemy)
        {
            if (!activeObjectMap.ContainsKey(enemy.name))
            {
                //Debug.Log("object doesn't exists");
                return;
            }

            var poolObject = activeObjectMap[enemy.name];
            inactiveObjects.Enqueue(poolObject);
        }

        public Roadsection RecycleObject()
        {
            Roadsection poolObject;
            inactiveObjects.TryDequeue(out poolObject);
            if (poolObject == null)
            {
                return null;
            }

            if (!activeObjectMap.ContainsKey(poolObject.name))
            {
                activeObjectMap.Add(poolObject.name, poolObject);
            }

            return poolObject;
        }
    }
}
