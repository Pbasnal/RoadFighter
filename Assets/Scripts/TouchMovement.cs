using UnityEngine;

namespace Assets.Scripts
{
    public class TouchMovement : MonoBehaviour
    {
        public int moveUnits = 1;
        public int currentPositionInUnits;
        public int movementRange = 1;
        public int moveSpeed = 1;

        private int moveDirection;
        private bool touchHold;
        private Vector2 initialPosition;

        private void Update()
        {
            var direction = GetMoveDirection();

            if (direction != 0)
            {
                currentPositionInUnits = currentPositionInUnits + moveUnits * direction;
                transform.position = new Vector3(currentPositionInUnits, transform.position.y, 0);
                moveDirection = direction;
            }
        }

        private void FixedUpdate()
        {
            var minPos = currentPositionInUnits - 0.05;
            var maxPos = currentPositionInUnits + 0.05;
            if(transform.position.x < minPos || transform.position.x > maxPos)
            {
                transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime * moveDirection, transform.position.y, 0);                
            }
        }

        private int GetMoveDirection()
        {
            if (Input.touchCount == 0)
            {
                return 0;
            }

            var touch = Input.GetTouch(0);
            Vector2 direction = Vector2.zero;

            if (touch.phase == TouchPhase.Began)
            {
                initialPosition = touch.position;
                touchHold = false;
            }
            else if (!touchHold && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
            {
                touchHold = true;
                direction = touch.position - initialPosition;
            }

            if (direction.x < 0)
            {
                return -1;
            }
            else if (direction.x == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
