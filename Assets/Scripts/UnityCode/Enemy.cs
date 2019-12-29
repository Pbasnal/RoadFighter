using Assets.Scripts.UnityLogic.BehaviourInterface;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Enemy : MonoBehaviour, IEnemy
    {
        public GameState gameState;
        public FloatValue levelSpeed;

        [Range(0.5f, 2)]
        public float speedMultiplier;

        public string Name => name;
        public float StartTime { get; set; }
        public float MinYTime { get; set; }
        public float MaxYTime { get; set; }

        public float Speed => levelSpeed.value * speedMultiplier;

        private void FixedUpdate()
        {
            if (gameState.State != States.Running)
            {
                return;
            }

            transform.position = new Vector2(transform.position.x, transform.position.y - Speed * Time.deltaTime);
        }
    }
}
