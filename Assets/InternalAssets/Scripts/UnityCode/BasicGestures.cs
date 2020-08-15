using System;

using DigitalRubyShared;

using UnityEngine;

namespace LockdownGames.Assets._Project.Systems.InputSystem
{
    public class BasicGestures : MonoBehaviour
    {
        public FingersScript fingersScript;
        public new Camera camera;

        public LayerMask layersToDetectObjectsIn;

        //public Transform clickedLocation;

        public Action<GestureRecognizer, Vector2, Transform> tapGestureCallback;
        public Action<GestureRecognizer, Vector2, Transform> doubleTapGestureCallback;
        public Action<GestureRecognizer, Vector2, Vector2, Transform> swipeGestureCallback;

        private TapGestureRecognizer tapGesture;
        private TapGestureRecognizer doubleTapGesture;
        private TapGestureRecognizer tripleTapGesture;
        private SwipeGestureRecognizer swipeGesture;
        private PanGestureRecognizer panGesture;
        private ScaleGestureRecognizer scaleGesture;
        private RotateGestureRecognizer rotateGesture;
        private LongPressGestureRecognizer longPressGesture;

        private void Awake()
        {
            if (fingersScript == null)
            {
                fingersScript = FindObjectOfType<FingersScript>();
                if (fingersScript == null)
                {
                    throw new Exception("Please add FingerScript to the scene");
                }
            }

            if (camera == null)
            {
                camera = Camera.main;
                if (camera == null)
                {
                    throw new Exception("Please add at least one camera to the scene");
                }
            }
        }

        public void Start()
        {
            tapGesture = new TapGestureRecognizer();
            doubleTapGesture = new TapGestureRecognizer();
            swipeGesture = new SwipeGestureRecognizer();
            panGesture = new PanGestureRecognizer();
            scaleGesture = new ScaleGestureRecognizer();
            rotateGesture = new RotateGestureRecognizer();
            longPressGesture = new LongPressGestureRecognizer();
            tripleTapGesture = new TapGestureRecognizer();

            //CreateDoubleTapGesture();
            //CreateTapGesture();
            CreateSwipeGesture();

            //CreatePanGesture();
            //CreateScaleGesture();
            //CreateRotateGesture();
            //CreateLongPressGesture();
            //CreatePlatformSpecificViewTripleTapGesture();
        }

        private void CreateTapGesture()
        {
            tapGesture.StateUpdated += TapGestureCallback;
            //tapGesture.NameOfGesture = "Single Tap gesture";
            //tapGesture.RequireGestureRecognizerToFail = doubleTapGesture;
            fingersScript.AddGesture(tapGesture);
        }

        private void CreateDoubleTapGesture()
        {
            doubleTapGesture.NumberOfTapsRequired = 2;
            //doubleTapGesture.get NameOfGesture = "Double Tap gesture";
            doubleTapGesture.StateUpdated += DoubleTapGestureCallback;
            //doubleTapGesture.RequireGestureRecognizerToFail = tripleTapGesture;
            fingersScript.AddGesture(doubleTapGesture);
        }

        private void CreateSwipeGesture()
        {
            swipeGesture.Direction = SwipeGestureRecognizerDirection.Any;
            swipeGesture.StateUpdated += SwipeGestureCallback;
            swipeGesture.MinimumDistanceUnits = 0.2f;
            swipeGesture.MinimumSpeedUnits = 1.0f;
            swipeGesture.DirectionThreshold = 1.0f; // allow a swipe, regardless of slope
            fingersScript.AddGesture(swipeGesture);
        }

        private void CreatePanGesture()
        {
            panGesture.MinimumNumberOfTouchesToTrack = 2;
            panGesture.StateUpdated += PanGestureCallback;
            fingersScript.AddGesture(panGesture);
        }

        private void CreateScaleGesture()
        {
            scaleGesture.StateUpdated += ScaleGestureCallback;
            fingersScript.AddGesture(scaleGesture);
        }

        private void CreateRotateGesture()
        {
            rotateGesture.StateUpdated += RotateGestureCallback;
            fingersScript.AddGesture(rotateGesture);
        }

        private void CreateLongPressGesture()
        {
            longPressGesture.MaximumNumberOfTouchesToTrack = 1;
            longPressGesture.StateUpdated += LongPressGestureCallback;
            fingersScript.AddGesture(longPressGesture);
        }

        private void CreatePlatformSpecificViewTripleTapGesture()
        {
            tripleTapGesture.StateUpdated += PlatformSpecificViewTapUpdated;
            tripleTapGesture.NumberOfTapsRequired = 3;
            //tripleTapGesture.PlatformSpecificView = bottomLabel.gameObject;
            fingersScript.AddGesture(tripleTapGesture);
        }

        private void TapGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State != GestureRecognizerState.Ended)
            {
                return;
            }

            var worldFocusPoint = camera.ScreenToWorldPoint(new Vector3(gesture.FocusX, gesture.FocusY, camera.orthographicSize));
            Debug.Log("Tapped");
            //clickedLocation.position = worldFocusPoint;
            tapGestureCallback?.Invoke(gesture, worldFocusPoint, DetectObject(worldFocusPoint));
        }

        private void DoubleTapGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State != GestureRecognizerState.Ended)
            {
                return;
            }

            var worldFocusPoint = camera.ScreenToWorldPoint(new Vector3(gesture.FocusX, gesture.FocusY, camera.orthographicSize));
            doubleTapGestureCallback?.Invoke(gesture, worldFocusPoint, DetectObject(worldFocusPoint));
        }        

        private void SwipeGestureCallback(GestureRecognizer gesture)
        {
            Debug.Log(string.Format("Swiping s:{0}  dx:{1}  dy:{2}", gesture.Speed, gesture.DistanceX, gesture.DistanceY));
            if (gesture.State != GestureRecognizerState.Ended)
            {
                return;
            }

            Debug.Log(string.Format("Swiped s:{0}  dx:{1}  dy:{2}", gesture.Speed, gesture.DistanceX, gesture.DistanceY));
            var startVec = new Vector3(gesture.StartFocusX, gesture.StartFocusX, camera.nearClipPlane);
            var endVec = new Vector3(gesture.StartFocusX + gesture.DeltaX, gesture.StartFocusY + gesture.DeltaY, camera.nearClipPlane);
            var startPosition = camera.ScreenToWorldPoint(startVec);
            var endPosition = camera.ScreenToWorldPoint(endVec);
            swipeGestureCallback?.Invoke(gesture, startPosition, endPosition, null);
        }

        private void PanGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State != GestureRecognizerState.Executing)
            {
                return;
            }
        }       

        private void ScaleGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State != GestureRecognizerState.Executing)
            {
                return;
            }
        }       

        private void RotateGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State != GestureRecognizerState.Executing)
            {
                return;
            }
        }

        private void LongPressGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Began)
            {
                // perform begin
            }
            else if (gesture.State == GestureRecognizerState.Executing)
            {
                // perform continue
            }
            else if (gesture.State == GestureRecognizerState.Ended)
            {
                // perform end
            }
        }

        private void PlatformSpecificViewTapUpdated(GestureRecognizer gesture)
        {
            if (gesture.State != GestureRecognizerState.Ended)
            {
                return;
            }
        }

        private static bool? CaptureGestureHandler(GameObject obj)
        {
            // I've named objects PassThrough* if the gesture should pass through and NoPass* if the gesture should be gobbled up, everything else gets default behavior
            if (obj.name.StartsWith("PassThrough"))
            {
                // allow the pass through for any element named "PassThrough*"
                return false;
            }
            else if (obj.name.StartsWith("NoPass"))
            {
                // prevent the gesture from passing through, this is done on some of the buttons and the bottom text so that only
                // the triple tap gesture can tap on it
                return true;
            }

            // fall-back to default behavior for anything else
            return null;
        }

        private Transform DetectObject(Vector2 worldPoint)
        {
            var collider = Physics2D.OverlapCircle(worldPoint, 0.2f, layersToDetectObjectsIn);

            if (collider == null)
            {
                return null;
            };

            return collider.transform;
        }
    }
}
