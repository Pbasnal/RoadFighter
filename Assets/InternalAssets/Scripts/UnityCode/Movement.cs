using System;
using System.Collections.Generic;
using Assets.Scripts.UnityLogic.BehaviourInterface;
using Assets.Scripts.UnityLogic.ScriptableObjects;
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
        
        [Space]
        [Header("Ray hit check Settings")]
        public float rayLength;
        public Transform top;
        public Transform bottom;
        public LayerMask layerMask;

        // IMoveable fields and properties
        public Vector2 CurrentPosition
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
        public float Speed => speed;
        public int MoveUnits => moveUnits;
        public int LeftLimit => leftLimit;
        public int RightLimit => rightLimit;

        private IInputManager inputManager;
        private IDictionary<InputCommand, Action> commandMap;
        private AudioManager _audioManager;

        private Vector3 destination;

        public RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
        {
            return Physics2D.Raycast(origin, direction, distance, layerMask);
        }

        private void Awake()
        {
            Debug.Log("Application platform " + Application.platform);
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                inputManager = fingersInput;
            }
            else
            {
                inputManager = keyboardInputManager;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            controller.SetActor(this);
            controller.SetDirectionBlocked(Direction.Left, false);
            controller.SetDirectionBlocked(Direction.Right, false);

            commandMap = new Dictionary<InputCommand, Action>
            {
                { InputCommand.None, EmptyCommand },
                { InputCommand.Left, controller.MoveDestinationLeft },
                { InputCommand.Right, controller.MoveDestinationRight}
            };

            _audioManager = FindObjectOfType<AudioManager>();
        }

        // Update is called once per frame
        private void Update()
        {
            commandMap[inputManager.GetCommand()]();
            controller.Move();

            if (Mathf.Pow(destination.x - transform.position.x, 2) < 0.001f)
            {
                destination = transform.position;
            }
        }

        private void FixedUpdate()
        {
            var newPos= Vector3.Lerp(transform.position, destination, Time.fixedDeltaTime * speed);
            rigidbody.MovePosition(newPos);

            //controller.SetDirectionBlocked(Direction.Left, CheckLeftHit());
            //controller.SetDirectionBlocked(Direction.Right, CheckRightHit());
        }

        private bool CheckLeftHit()
        {
            Debug.DrawLine(top.position, new Vector2(top.position.x - rayLength, top.position.y), Color.red, 1);
            var lhit = Physics2D.Raycast(top.position, Vector2.left, rayLength, layerMask);
            if (lhit.collider != null && lhit.collider.tag.Equals("Enemy"))
            {
                return true;
            }

            lhit = Physics2D.Raycast(bottom.position, Vector2.left, rayLength, layerMask);
            if (lhit.collider != null && lhit.collider.tag.Equals("Enemy"))
            {
                return true;
            }

            return false;
        }

        private bool CheckRightHit()
        {
            Debug.DrawLine(top.position, new Vector2(top.position.x + rayLength, top.position.y), Color.red, 1);
            // top hit detection
            var rhit = Physics2D.Raycast(top.position, Vector2.right, rayLength, layerMask);
            if (rhit.collider != null && rhit.collider.tag.Equals("Enemy"))
            {
                return true;
            }

            // bottom hit detection
            rhit = Physics2D.Raycast(bottom.position, Vector2.right, rayLength, layerMask);
            if (rhit.collider != null && rhit.collider.tag.Equals("Enemy"))
            {
                return true;
            }

            return false;
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

        public void MoveTo(Vector2 destination)
        {
            this.destination = destination;
        }
    }
}