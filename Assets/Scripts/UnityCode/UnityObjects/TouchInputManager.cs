using UnityEngine;

namespace Assets.Scripts.UnityCode.UnityObjects
{
    [CreateAssetMenu(menuName = "Input/Touch", fileName = "TouchInputManager", order = 51)]
    public class TouchInputManager : InputManager
    {
        private bool touchHold;
        private Vector2 initialPosition;

        public override InputCommand GetCommand()
        {
            if (Input.touchCount == 0)
            {
                return InputCommand.None;
            }

            var touch = Input.GetTouch(0);
            Vector2 direction = Vector2.zero;

            if (touch.phase == TouchPhase.Began)
            {
                touchHold = false;
                return InputCommand.None;
            }
            else if (!touchHold && touch.phase == TouchPhase.Moved)
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
