using System.Collections.Generic;
using DigitalRubyShared;
using LockdownGames.Assets._Project.Systems.InputSystem;
using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    public class FingersInput : BehaviourInputManager
    {
        public BasicGestures basicGestures;
        public override float TimeWhenCommandCame { get; set; }


        private Queue<InputCommand> inputCommands;

        private bool swipeEneded;

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
            if (gestureRecognizer.State == GestureRecognizerState.Ended)
            {
                swipeEneded = true;
                //Debug.Log("Swiping ended");
            }

            var newCommand = InputCommand.None;
            if (endPosition.x > startPosition.x)
            {
                newCommand = InputCommand.Right;
            }
            else if (endPosition.x < startPosition.x)
            {
                newCommand = InputCommand.Left;
            }

            // this check means that previous swipe ended.
            // so we can process the incoming swipe.
            if (swipeEneded)
            {
                // incoming swip will be processed. So, marking swipeEnded
                // as false. This will potect this function from recording same 
                // swipe multiple times.
                swipeEneded = false;
                //Debug.Log("moving ");
                inputCommands.Enqueue(newCommand);

                TimeWhenCommandCame = basicGestures.TouchStartTime;
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
