using UnityEngine;

namespace Assets.Scripts.UnityLogic.BehaviourInterface
{
    public interface IMoveableActor
    {
        Vector2 CurrentPosition { get; set; }
        Vector3 Destination { get; }
    }
}