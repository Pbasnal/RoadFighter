using Assets.Scripts.UnityLogic.BehaviourInterface;
using NSubstitute;
using UnityEngine;

namespace MovementTests
{
    public class ActorBuilder
    {
        private IMoveableActor actor;

        public ActorBuilder()
        {
            actor = Substitute.For<IMoveableActor>();
        }

        public ActorBuilder At(Vector2 position)
        {
            actor.CurrentPosition.Returns(position);
            return this;
        }

        public ActorBuilder WithSpeed(float speed)
        {
            actor.Speed.Returns(speed);
            return this;
        }

        public ActorBuilder WhichCanMoveUnits(int units)
        {
            actor.MoveUnits.Returns(units);
            return this;
        }

        public IMoveableActor Build()
        {
            return actor;
        }
    }
}
