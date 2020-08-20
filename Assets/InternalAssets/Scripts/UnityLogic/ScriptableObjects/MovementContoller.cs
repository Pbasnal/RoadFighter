using Assets.Scripts.UnityLogic.BehaviourInterface;
using UnityEngine;

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

    //public class Direction
    //{
    //    public const int Left = -1;
    //    public const int None = 0;
    //    public const int Right = 1;
    //}
}
