using UnityEngine;

namespace TestScripts
{
    public class RigidBodySimple : MonoBehaviour
    {
        public float speed;
        public Transform startPosition;
        public Transform endPosition;

        private Rigidbody2D rigidBody;
        private float currentSpeed = 0;
        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (transform.position.y < endPosition.position.y)
            {
                transform.position = new Vector3(startPosition.position.x, startPosition.position.y, 0);
            }

            //transform.position = new Vector3(transform.position.x,
            //    transform.position.y - speed * Time.deltaTime, 0);
        }

        private void FixedUpdate()
        {
            if (speed != currentSpeed)
            {
                rigidBody.velocity = new Vector2(0, -speed);
                currentSpeed = speed;
            }
        }
    }
}
