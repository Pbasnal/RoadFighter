using System.Collections.Generic;
using DigitalRubyShared;
using LockdownGames.Assets._Project.Systems.InputSystem;
using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    public class FingersInput : BehaviourInputManager
    {
        public BasicGestures basicGestures;
        private InputCommand inputCommand = InputCommand.None;

        private Queue<InputCommand> inputCommands;

        private void OnEnable()
        {
            if (basicGestures == null)
            {
                basicGestures = FindObjectOfType<BasicGestures>();
            }
            basicGestures.swipeGestureCallback += PlayerSwipedOnScreen;

            inputCommands = new Queue<InputCommand>();
        }

        private void OnDisable()
        {
            basicGestures.swipeGestureCallback -= PlayerSwipedOnScreen;
        }

        private void PlayerSwipedOnScreen(GestureRecognizer gestureRecognizer, 
            Vector2 startPosition, Vector2 endPosition, Transform transformedThatGotSwiped)
        {
            if (endPosition.x > startPosition.x)
            {
                inputCommands.Enqueue(InputCommand.Right);
            }
            else if (endPosition.x < startPosition.x)
            {
                inputCommands.Enqueue(InputCommand.Left);
            }
        }

        public override InputCommand GetCommand()
        {
            if (inputCommands.Count == 0)
            {
                return InputCommand.None;
            }

            return inputCommands.Dequeue();
        }
    }
}
