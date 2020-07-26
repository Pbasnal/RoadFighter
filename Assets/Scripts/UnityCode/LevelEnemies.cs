using Assets.Scripts.UnityLogic.BehaviourInterface;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityCode;
using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    public class LevelEnemies : PausableBehaviour, ILevelEnemies
    {
        public GameState gameState;
        public Transform minBoundary;
        public Transform maxBoundary;
        public Transform[] SpawnPositions;
        public EnemyPool[] enemyPools;
        public LayerMask layerOfEnemies;
        public LevelEnemyAI levelEnemyAI;

        public float Miny => minBoundary.localPosition.y;
        public float Maxy => maxBoundary.localPosition.y;
        public int NumberOfLanes => SpawnPositions.Length;

        // Use this for initialization
        private void Start()
        {
            levelEnemyAI.SetLevelEnemies(this);
            OnPlay();
        }

        private IEnumerator SpawnEnemyCars()
        {
            while (true)
            {
                if (gameState.State != States.Running)
                {
                    yield return new WaitForSecondsRealtime(2);
                }

                var contexts = levelEnemyAI.GenerateEnemySpawnContexts();
                for (int i = 0; i < contexts.Length; i++)
                {
                    if (!contexts[i].spawnCar)
                    {
                        continue;
                    }

                    var enemy = enemyPools[contexts[i].enemyType].RecycleObject();
                    if (enemy == null)
                    {
                        enemy = InstantiateNewEnemy(contexts[i].enemyType, true);
                        enemy.StartTime = Time.time;
                        enemy.MinYTime = contexts[i].MinyTime;
                        enemy.MaxYTime = contexts[i].MaxyTime;
                        enemy.StartTime = contexts[i].startTime;
                        enemyPools[contexts[i].enemyType].AddActiveObject("enemy", enemy);
                    }
                    enemy.transform.position = SpawnPositions[contexts[i].lane].position;
                }

                yield return new WaitForSecondsRealtime(2);
            }
        }

        public int GetARandomEnemyForLane(int lane)
        {
            return Random.Range(0, enemyPools.Length);
        }

        public IEnemy GetEnemyInTheLane(int lane)
        {
            var spawnPosition = (Vector2)SpawnPositions[lane].position;
            var hit = Physics2D.Raycast(spawnPosition, Vector2.down, Mathf.Abs(maxBoundary.localPosition.y), layerOfEnemies);

            if (hit.collider == null || !hit.collider.tag.Equals("Enemy"))
            {
                return null;
            }

            return hit.collider.gameObject.GetComponent<IEnemy>();
        }

        public List<int> SelectRandomLanesToSpawnEnemies()
        {
            //return new List<int> { 0, 1, 2 };
            var numberOfLanesToSelect = Random.Range(1, NumberOfLanes + 1);

            HashSet<int> randomNumbers = new HashSet<int>();

            for (int i = 0; i < numberOfLanesToSelect; i++)
            {
                while (!randomNumbers.Add(Random.Range(0, NumberOfLanes)))
                {
                    ;
                }
            }

            return randomNumbers.ToList();
        }

        public IEnemy GetEnemyOfType(int enemyType)
        {
            return enemyPools[enemyType].prefab.GetComponent<Enemy>();
        }

        private Enemy InstantiateNewEnemy(int enemyType, bool active)
        {
            var enemy = Instantiate(enemyPools[enemyType].prefab);
            enemy.transform.parent = transform;
            enemy.SetActive(active);

            return enemy.GetComponent<Enemy>();
        }

        public float GetTime(int enemyType)
        {
            return Time.time;
        }

        public override void OnPause()
        {
            StopAllCoroutines();
        }

        public override void OnPlay()
        {
            StartCoroutine("SpawnEnemyCars");
        }
    }
}