using DigitalRubyShared;
using UnityEngine;

namespace Assets.Scripts.UnityCode.UnityObjects
{
    [CreateAssetMenu(menuName = "Input/Touch", fileName = "TouchInputManager", order = 51)]
    public class TouchInputManager : ScriptableInputManager
    {
        public GameObject basicGestures;
        private bool touchHold;
        private Vector2 initialPosition;

        private InputCommand inputCommand;

        private void OnEnable()
        {
            //basicGestures = FindObjectOfType<BasicGestures>();
            //basicGestures.swipeGestureCallback += PlayerSwipedOnScreen;
        }

        private void OnDisable()
        {
            //basicGestures.swipeGestureCallback -= PlayerSwipedOnScreen;
        }

        private void PlayerSwipedOnScreen(GestureRecognizer gestureRecognizer, 
            Vector2 startPosition, Vector2 endPosition, Transform transformedThatGotSwiped)
        {
            if (endPosition.x > startPosition.x)
            {
                inputCommand = InputCommand.Right;
            }
            else if (endPosition.x < startPosition.x)
            {
                inputCommand = InputCommand.Left;
            }
        }

        public override InputCommand GetCommand()
        {
            return inputCommand;

            if (Input.touchCount == 0)
            {
                return InputCommand.None;
            }

            var touch = Input.GetTouch(0);
            Vector2 direction = Vector2.zero;

            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                touchHold = false;
                return InputCommand.None;
            }
            else if (!touchHold && touch.phase == UnityEngine.TouchPhase.Moved)
            {
                touchHold = true;
                direction = touch.position - initialPosition;
            }

            if (direction.x < 0)
            {
                return InputCommand.Left;
            }
            else if (direction.x == 0)
            {
                return InputCommand.None;
            }
            else
            {
                return InputCommand.Right;
            }
        }
    }
}
