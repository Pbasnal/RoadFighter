using Assets.Scripts.UnityLogic.BehaviourInterface;
using UnityEngine;

namespace Assets.Scripts.UnityLogic.ScriptableObjects
{
    public abstract class MovementContoller : ScriptableObject
    {
        public abstract void SetActor(IMoveableActor actor);
        public abstract void MoveDestinationLeft();
        public abstract void MoveDestinationRight();
        public abstract Vector3 GetDestination();
    }
}
