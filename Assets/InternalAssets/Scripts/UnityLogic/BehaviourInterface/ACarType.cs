using System;
using UnityEngine;
using UnityLogic.CommonInterface;

namespace UnityLogic.BehaviourInterface
{
    [Serializable]
    public abstract class ACarType : APausableBehaviour, ICanBePooled
    {
        public abstract string Id { get; }
        public abstract bool CanCarMove { get; protected set; }

        public bool IsActive => gameObject.activeInHierarchy;

        public string Name => gameObject.name;

        public GameObject SpawnedObject { get; private set; }

        public abstract float Speed { get; }

        public virtual void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public ICanBePooled SpawnANewInstance()
        {
            var gObj = Instantiate(this.gameObject);
            gObj.name = Id + Guid.NewGuid().ToString();
            gObj.SetActive(false);

            SpawnedObject = gObj;

            return gObj.GetComponent<ICanBePooled>();
        }

        public abstract bool IsCollisionPossibleTill(float thisDistance);
        public abstract float MoveCarAndGetDistanceMoved();

        public abstract void ResetCar();
    }
}
