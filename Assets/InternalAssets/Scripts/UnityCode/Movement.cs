using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.Scripts.UnityLogic.BehaviourInterface;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using TMPro;
using UnityCode.Managers;
using UnityEngine;
using UnityLogic.BehaviourInterface;

namespace Assets.Scripts.UnityCode
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Movement : APausableBehaviour, IMoveableActor
    {
        // Inspector fields
        [Space]
        [Header("Game components to refer")]
        public ScriptableInputManager keyboardInputManager;
        public ScriptableInputManager touchInputManager;
        public BehaviourInputManager fingersInput;
        public GameState gameState;

        [Space]
        [Header("GameObject components")]
        public Animator animator;
        public new Rigidbody2D rigidbody;

        [Space]
        [Header("External components")]
        public MovementContoller controller;
        public GameObject unitLocator;

        [Space]
        [Header("Movement Settings")]
        public int leftLimit;
        public int rightLimit;
        public int moveUnits;
        public float speed;

        // IMoveable fields and properties
        public Vector2 CurrentPosition
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public Vector3 Destination { get; set; }

        private IInputManager inputManager;
        private IDictionary<InputCommand, Action> commandMap;
        private AudioManager _audioManager;

        public RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
        {
            return Physics2D.Raycast(origin, direction, distance, layerMask);
        }

        private void Awake()
        {
            //Debug.Log("Application platform " + Application.platform);
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                inputManager = fingersInput;// touchInputManager;
            }
            else
            {
                inputManager = fingersInput;// touchInputManager;// keyboardInputManager;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            controller.SetActor(this);

            commandMap = new Dictionary<InputCommand, Action>
            {
                { InputCommand.None, EmptyCommand },
                { InputCommand.Left, controller.MoveDestinationLeft },
                { InputCommand.Right, controller.MoveDestinationRight}
            };

            _audioManager = FindObjectOfType<AudioManager>();
            Destination = transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            commandMap[inputManager.GetCommand()]();
            Destination = controller.GetDestination();
            transform.position = Vector3.Lerp(transform.position, Destination, speed * Time.deltaTime);
        }


        private void EmptyCommand()
        { }

        public override void OnPlay()
        {
            if (animator != null)
            {
                animator.enabled = false;
            }

            _audioManager.PlayAudio("PlayerCar");
        }

        public override void OnPause()
        {
            if (animator != null)
            {
                animator.enabled = true;
            }

            if (_audioManager == null)
            {
                return;
            }
            _audioManager.StopAudio("PlayerCar");
        }
    }
}