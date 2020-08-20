using UnityEngine;

namespace Assets.Scripts.UnityLogic.BehaviourInterface
{
    public interface IMoveableActor
    {
        int LeftLimit { get; }
        int RightLimit { get; }
        int MoveUnits { get; }
        Vector2 CurrentPosition { get; set; }
        float Speed { get; }

        void MoveTo(Vector2 destination);
    }
}