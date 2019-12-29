using Assets.Scripts.UnityLogic.BehaviourInterface;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;

namespace MovementTests
{
    public class ControllerScenario
    {
        private TransformMovementController controller;

        public ControllerScenario()
        {
            controller = ScriptableObject.CreateInstance<TransformMovementController>();
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

        public ControllerScenario MoveTillDetination(out float deltaTime)
        {
            deltaTime = 0.0f;
            while (controller.prevDirection != TransformMovementController.Direction.None)
            {
                controller.Move();
                deltaTime += Time.deltaTime;
            }

            return this;
        }

        public ControllerScenario MoveFor(float time)
        {
            var deltaTime = 0.0f;
            while (controller.prevDirection != TransformMovementController.Direction.None 
                && deltaTime < time)
            {
                controller.Move();
                deltaTime += Time.deltaTime;
            }

            return this;
        }

        public static implicit operator TransformMovementController(ControllerScenario builder)
        {
            return builder.controller;
        }
    }
}
