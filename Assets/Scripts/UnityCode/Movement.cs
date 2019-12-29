using Assets.Scripts.UnityLogic.BehaviourInterface;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;
using static Assets.Scripts.UnityLogic.ScriptableObjects.TransformMovementController;

namespace Assets.Scripts.UnityCode
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Movement : MonoBehaviour, IMoveableActor
    {
        // Inspector fields
        public GameState gameState;
        public int leftLimit;
        public int rightLimit;

        public float rayLength;

        public float speed;
        public int moveUnits;

        public Transform top;
        public Transform bottom;

        public MovementContoller controller;
        public GameObject unitLocator;

        public LayerMask layerMask;

        // IMoveable fields
        public Vector2 CurrentPosition
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
        public float Speed => speed;
        public int MoveUnits => moveUnits;
        public int LeftLimit => leftLimit;
        public int RightLimit => rightLimit;

        public RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
        {
            return Physics2D.Raycast(origin, direction, distance, layerMask);
        }

        // Start is called before the first frame update
        private void Start()
        {
            controller.SetActor(this);
            controller.SetDirectionBlocked(Direction.Left, false);
            controller.SetDirectionBlocked(Direction.Right, false);
        }

        // Update is called once per frame
        private void Update()
        {
            if (gameState.State != States.Running)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                controller.MoveDestinationLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                controller.MoveDestinationRight();
            }
        }

        private void FixedUpdate()
        {
            if (gameState.State != States.Running)
            {
                return;
            }

            controller.SetDirectionBlocked(Direction.Left, CheckLeftHit());
            controller.SetDirectionBlocked(Direction.Right, CheckRightHit());
            controller.Move();
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
            var rhit = Physics2D.Raycast(top.position, Vector2.right, rayLength, layerMask);
            if (rhit.collider != null && rhit.collider.tag.Equals("Enemy"))
            {
                return true;
            }

            rhit = Physics2D.Raycast(bottom.position, Vector2.right, rayLength, layerMask);
            if (rhit.collider != null && rhit.collider.tag.Equals("Enemy"))
            {
                return true;
            }

            return false;
        }
    }
}