using System.Collections.Generic;
using Assets.InternalAssets.Scripts.UnityLogic.BehaviourInterface;
using Assets.InternalAssets.Scripts.UnityLogic.Mechanics;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;
using UnityLogic.BehaviourInterface;
using UnityLogic.Mechanics;

namespace UnityCode
{
    public class PatternSpawningTest : MonoBehaviour, IGridSpawnBeaviour
    {
        public MultiGameObjectPool enemyCarsPool;
        public float colOffset;
        public float rowOffset;

        public GridPattern[] carSpawnPatterns;
        private GridSpawnLocations gridSpawn;

        public Vector3 SpawnPosition => transform.position;

        public float ColOffset => colOffset;

        public float RowOffset => rowOffset;

        private List<ACarType> spawnedCars = new List<ACarType>();

        private float prevColOffset = -1;
        private float prevRowOffset = -1;
        private Vector3 prevPosition;

        private void Awake()
        {
            gridSpawn = new GridSpawnLocations(this);
            enemyCarsPool.InitializePools();
        }

        private void Update()
        {
            if (colOffset != prevColOffset 
                || rowOffset != prevRowOffset
                || Vector3.Distance(transform.position, prevPosition) != 0)
            {
                foreach (var car in spawnedCars)
                {
                    enemyCarsPool.DeactivateObject(car);
                }
                SpawnEnemyCars();

                prevColOffset = colOffset;
                prevRowOffset = rowOffset;
                prevPosition = new Vector3(
                    transform.position.x,
                    transform.position.y, 0);
            }
        }

        private void SpawnEnemyCars()
        {
            var pattern = SelectACarSpawnPattern();
            var spawnLocations = gridSpawn.GenerateSpawnLocationsFor(pattern);

            for (int r = 0; r < spawnLocations.GetLength(0); r++)
            {
                for (int c = 0; c < spawnLocations.GetLength(1); c++)
                {
                    if (spawnLocations[r, c] == null)
                    {
                        continue;
                    }

                    var car = (ACarType)enemyCarsPool.RecycleObject(pattern.rows[r].cells[c].Id);
                    if (car == null)
                    {
                        Debug.Log("No more cars left in the pool");
                        continue;
                    }

                    car.transform.position = new Vector3(
                        spawnLocations[r, c].Value.x,
                        spawnLocations[r, c].Value.y,
                        0);
                    spawnedCars.Add(car);
                }
            }
        }

        private GridPattern SelectACarSpawnPattern()
        {
            if (carSpawnPatterns == null || carSpawnPatterns.Length == 0)
            {
                return null;
            }

            var selectedPatternIndex = Random.Range(0, carSpawnPatterns.Length);
            return carSpawnPatterns[selectedPatternIndex];
        }
    }
}
