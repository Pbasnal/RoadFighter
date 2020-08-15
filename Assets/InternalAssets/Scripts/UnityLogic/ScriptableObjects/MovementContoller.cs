using Assets.Scripts.UnityLogic.BehaviourInterface;
using UnityEngine;
using static Assets.Scripts.UnityLogic.ScriptableObjects.TransformMovementController;

namespace Assets.Scripts.UnityLogic.ScriptableObjects
{
    public abstract class MovementContoller : ScriptableObject
    {
        public abstract Vector2 UnitLocation { get; }
        public abstract void SetActor(IMoveableActor actor);
        public abstract void MoveDestinationLeft();
        public abstract void MoveDestinationRight();
        public abstract void Move();
        public abstract void SetDirectionBlocked(Direction direction, bool blocked);
    }
}
