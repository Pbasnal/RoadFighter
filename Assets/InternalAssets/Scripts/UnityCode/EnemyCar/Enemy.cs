using Assets.Scripts.UnityLogic.BehaviourInterface;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityCode.EnemyCar;
using UnityCode.Managers;
using UnityEngine;
using UnityLogic.BehaviourInterface;

namespace Assets.Scripts.UnityCode
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Enemy : ACarType, IEnemy
    {
        [Header("Enemy Information")]
        public string enemyType;
        [Range(0.5f, 2)]
        public float speedMultiplier;

        [Space]
        [Header("Game's Global State Data")]
        public GameState gameState;
        public FloatValue levelSpeed;
       
        // IEnemy interface implementation **********************
        public override string Id => enemyType;
        public override bool CanCarMove { get; protected set; }
        public float StartTime { get; set; }
        public float MinYTime { get; set; }
        public float MaxYTime { get; set; }
        public override float Speed => levelSpeed.value * speedMultiplier;
        // *****************************************************

        // Components which are resolved in Awake method********
        private Rigidbody2D _rigidBody;
        private AudioManager _audioManager;
        private Damage _damageController;
        // *****************************************************

        private void Awake()
        {
            _audioManager = FindObjectOfType<AudioManager>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _damageController = GetComponent<Damage>();

            _rigidBody.interpolation = RigidbodyInterpolation2D.Interpolate;
            
        }

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);

            // commenting below logic, because destination variable is not getting used

            // this logic works because in enemy controller we are assigning position
            // to the car before we activate it.
            // dependent on: EnemyController.SpawnEnemyCars()
            //if (isActive)
            //{
            //    destination = (Vector2)transform.position + Vector2.down * 50;
            //}
        }

        public override float MoveCarAndGetDistanceMoved()
        {
            var distanceMoved = Speed * Time.fixedDeltaTime;
            var newPosition = new Vector2(transform.position.x, transform.position.y - distanceMoved);

            _rigidBody.MovePosition(newPosition);

            //transform.position = Vector2.MoveTowards(transform.position, destination, distanceMoved); 

            return distanceMoved;
        }

        public override bool IsCollisionPossibleTill(float thisDistance)
        {
            bool CollisionWithOtherCarsAheadIsPossible = true;

            var hit = Physics2D.Raycast(transform.position, Vector3.down, thisDistance, gameObject.layer);
            if (!hit)
            {
                return !CollisionWithOtherCarsAheadIsPossible;
            }

            var carWithWhichCollisionCanHappen = hit.collider.GetComponent<Enemy>();
            if (carWithWhichCollisionCanHappen == null)
            {
                //Debug.Log(name + " can collide with " + hit.collider.name);
                return !CollisionWithOtherCarsAheadIsPossible;
            }
            
            var speedOfOtherCar = carWithWhichCollisionCanHappen.Speed;
            if (speedOfOtherCar >= Speed)
            {
                return !CollisionWithOtherCarsAheadIsPossible;
            }

            return CollisionWithOtherCarsAheadIsPossible;
        }

        public override void ResetCar()
        {
            _damageController.ResetDamageTaken();
        }

        public override void OnPlay()
        {
            base.OnPlay();

            _audioManager.PlayAudio("EnemyCar");
        }

        public override void OnPause()
        {
            base.OnPause();

            if (_audioManager == null)
            {
                return;
            }
            _audioManager.StopAudio("EnemyCar");
        }
    }
}
