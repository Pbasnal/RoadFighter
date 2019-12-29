using Assets.Scripts.UnityLogic.BehaviourInterface;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UnityLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "AI/Enemy", fileName = "EnemyAI", order = 51)]
    public class LevelEnemyAI : ScriptableObject
    {
        private ILevelEnemies levelEnemies;
        private EnemySpawnContext[] enemySpawnContexts;

        public void SetLevelEnemies(ILevelEnemies levelEnemies)
        {
            this.levelEnemies = levelEnemies;
            enemySpawnContexts = new EnemySpawnContext[levelEnemies.NumberOfLanes];

            for (int i = 0; i < levelEnemies.NumberOfLanes; i++)
            {
                enemySpawnContexts[i] = new EnemySpawnContext();
            }
        }

        public bool SpawnEnemies()
        {
            return false;
        }

        public EnemySpawnContext[] GenerateEnemySpawnContexts()
        {
            for (int j = 0; j < enemySpawnContexts.Length; j++)
            {
                enemySpawnContexts[j].Clear();
            }

            var selectedLanes = levelEnemies.SelectRandomLanesToSpawnEnemies();
            var existingEnemies = GetEnemiesInAllLanes();//.ToList();

            for (int i = 0; i < selectedLanes.Count; i++)
            {
                var lane = selectedLanes[i];
                var enemyType = levelEnemies.GetARandomEnemyForLane(lane);

                var e2 = levelEnemies.GetEnemyOfType(enemyType);
                var startTime = levelEnemies.GetTime(enemyType);

                var mirrorE2 = new MirrorEnemy
                {
                    Name = e2.Name,
                    MaxYTime = Mathf.Abs(levelEnemies.Maxy) / e2.Speed + startTime,
                    MinYTime = Mathf.Abs(levelEnemies.Miny) / e2.Speed + startTime,
                    Speed = e2.Speed,
                    StartTime = startTime
                };

                //Debug.Log(string.Format("lane: {0}  e2- min: {1} -   max: {2}", selectedLanes[i], mirrorE2.MinYTime, mirrorE2.MaxYTime));

                // Lane check
                if (existingEnemies[lane] != null)
                {
                    continue;
                }

                // Check if new enemy will block the path for the player
                bool spawn = false;
                for (int j = 0; j < existingEnemies.Length; j++)
                {
                    if (lane == j)
                    {
                        continue;
                    }
                    if (existingEnemies[j] == null || !CheckOverlap(existingEnemies[j], mirrorE2.MinYTime, mirrorE2.MaxYTime))
                    {
                        spawn = true;
                        break;
                    }
                }

                if (!spawn)
                {
                    continue;
                }

                existingEnemies[lane] = mirrorE2;

                enemySpawnContexts[i].spawnCar = true;
                enemySpawnContexts[i].lane = lane;
                enemySpawnContexts[i].enemyType = enemyType;
                enemySpawnContexts[i].MinyTime = mirrorE2.MinYTime;
                enemySpawnContexts[i].MaxyTime = mirrorE2.MaxYTime;
                enemySpawnContexts[i].startTime = startTime;
            }

            return enemySpawnContexts;
        }

        private bool CheckOverlap(IEnemy e1, float minyTime, float maxyTime)
        {
            if (e1.MaxYTime <= maxyTime && e1.MaxYTime > minyTime)
            {
                return true;
            }

            if (e1.MinYTime >= minyTime && e1.MinYTime < maxyTime)
            {
                return true;
            }

            if (maxyTime <= e1.MaxYTime && maxyTime > e1.MinYTime)
            {
                return true;
            }

            if (minyTime >= e1.MinYTime && minyTime < e1.MaxYTime)
            {
                return true;
            }

            return false;
        }

        public IEnemy[] GetEnemiesInAllLanes()
        {
            var enemies = new IEnemy[levelEnemies.NumberOfLanes];

            for (int i = 0; i < levelEnemies.NumberOfLanes; i++)
            {
                var enemy = levelEnemies.GetEnemyInTheLane(i);
                if (enemy == null)
                {
                    continue;
                }
                //Debug.Log(string.Format("lane: {0}  e1- min: {1} -   max: {2}", i, enemy.MinYTime, enemy.MaxYTime));
                enemies[i] = enemy;
            }

            return enemies;
        }
    }

    public class EnemySpawnContext
    {
        public int lane;
        public int enemyType;
        public bool spawnCar;
        public float startTime;
        public float MinyTime;
        public float MaxyTime;

        public List<string> calculationLog = new List<string>();

        public void Clear()
        {
            lane = 0;
            enemyType = 0;
            spawnCar = false;
            //enemy = null;
            calculationLog.Clear();
        }
    }

    public class MirrorEnemy : IEnemy
    {
        public string Name { get; set; }

        public float StartTime { get; set; }

        public float MinYTime { get; set; }
        public float MaxYTime { get; set; }

        public float Speed { get; set; }
    }
}
