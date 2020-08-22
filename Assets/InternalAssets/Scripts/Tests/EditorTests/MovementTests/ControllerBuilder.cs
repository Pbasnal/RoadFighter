using Assets.Scripts.UnityLogic.BehaviourInterface;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;

namespace MovementTests
{
    public class ControllerScenario
    {
        private PlayerMovementController controller;

        public ControllerScenario()
        {
            controller = ScriptableObject.CreateInstance<PlayerMovementController>();
        }

        public ControllerScenario ForActor(IMoveableActor actor)
        {
            controller.SetActor(actor);
            return this;
        }

        public ControllerScenario MoveDestinationLeft()
        {
            controller.MoveDestinationLeft();
            return this;
        }

        public ControllerScenario MoveDestinationRight()
        {
            controller.MoveDestinationRight();
            return this;
        }

        public ControllerScenario MoveTillDestination(out float deltaTime)
        {
            deltaTime = 0.0f;
            
            //while(controller.)

            return this;
        }

        public ControllerScenario MoveFor(float time)
        {
            var deltaTime = 0.0f;
            
            return this;
        }

        public static implicit operator PlayerMovementController(ControllerScenario builder)
        {
            return builder.controller;
        }
    }
}
