using UnityEngine;

namespace Assets.Scripts.UnityCode.UnityObjects
{
    [CreateAssetMenu(menuName = "Input/Touch", fileName = "TouchInputManager", order = 51)]
    public class TouchInputManager : ScriptableInputManager
    {
        public override float TimeWhenCommandCame { get; set; }

        public GameObject basicGestures;

        private bool touchStarted;
        private bool touchEnded;
        private bool isTouchMoving;

        private float touchDeltaPositionFromPrevFrame;
        private Vector2 touchStartPosition;
        private Vector2 touchPositionInPrevFrame;

        private InputCommand prevInputCommand;

        private InputCommand CaptureTouchBegan(Touch touch)
        {
            TimeWhenCommandCame = Time.time;
            touchStarted = true;
            touchEnded = false;
            isTouchMoving = false;
            touchStartPosition = touch.position;
            touchPositionInPrevFrame = touch.position;
            touchDeltaPositionFromPrevFrame = 0;

            return ProcessTouch();
        }

        private InputCommand CaptureTouchMoving(Touch touch)
        {
            touchDeltaPositionFromPrevFrame = touch.deltaPosition.x;
            isTouchMoving = Mathf.Abs(touchDeltaPositionFromPrevFrame) > 1f;

            Debug.Log($"Stage: {touch.phase} ds: {touchDeltaPositionFromPrevFrame} b: {touchStartPosition.x} s: {touchPositionInPrevFrame.x} e: {touch.position.x}");
            touchPositionInPrevFrame = touch.position;

            return ProcessTouch();
        }

        private InputCommand CaptureTouchEnd(Touch touch)
        {
            touchDeltaPositionFromPrevFrame = (touch.position - touchStartPosition).x;
            isTouchMoving = Mathf.Abs(touchDeltaPositionFromPrevFrame) > 1f;

            Debug.Log($"Stage: {touch.phase} ds: {touchDeltaPositionFromPrevFrame} b: {touchStartPosition.x} s: {touchPositionInPrevFrame.x} e: {touch.position.x}");
            touchPositionInPrevFrame = touch.position;

            var cmd = ProcessTouch();

            touchStarted = false;
            touchEnded = true;
            prevInputCommand = InputCommand.None;

            return cmd;
        }

        public override InputCommand GetCommand()
        {
            if (Input.touchCount == 0)
            {
                if (prevInputCommand != InputCommand.None)
                {
                    Debug.Log("No touch detected");
                }
                prevInputCommand = InputCommand.None;
                return InputCommand.None;
            }

            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case UnityEngine.TouchPhase.Began:
                    return CaptureTouchBegan(touch);
                case UnityEngine.TouchPhase.Moved:
                    return CaptureTouchMoving(touch);
                case UnityEngine.TouchPhase.Ended:
                case UnityEngine.TouchPhase.Canceled:
                    return CaptureTouchEnd(touch);
                default:
                    return InputCommand.None;
            }
        }

        private void DebugTouch()
        {
            
        }

        private InputCommand ProcessTouch()
        {
            if (!isTouchMoving)
            {
                prevInputCommand = InputCommand.None;
                return InputCommand.None;
            }
            else if (touchDeltaPositionFromPrevFrame < 0 && prevInputCommand != InputCommand.Left)
            {
                prevInputCommand = InputCommand.Left;
                return prevInputCommand;
            }
            else if (touchDeltaPositionFromPrevFrame > 0 && prevInputCommand != InputCommand.Right)
            {
                prevInputCommand = InputCommand.Right;
                return prevInputCommand;
            }

            return InputCommand.None;
        }
    }
}
