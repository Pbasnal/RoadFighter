using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.InternalAssets.Scripts.UnityLogic.BehaviourInterface;
using Assets.InternalAssets.Scripts.UnityLogic.Mechanics;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityCode;
using UnityCode.EnemyCar;
using UnityEngine;
using UnityLogic.BehaviourInterface;
using UnityLogic.Mechanics;
using URandom = UnityEngine.Random;

namespace SpawnSystem.UnityCode
{
    public class EnemyController : APausableBehaviour, IGridSpawnBeaviour
    {
        [Header("Player Collision settings to not block player")]
        public Transform player;
        public FloatValue bufferInYaxis;

        [Space]
        [Header("Car Spawn Settings")]
        public GamePauseController pauseController;
        public MultiGameObjectPool enemyCarsPool;
        public Transform carTravelLimit;
        public FloatValue gameSpeed;
        public FloatValue waitTimeBeforeStarting;
        public FloatValue waitTimeBetweenEachSpawn;
        public float distanceInWhichToAvoidCollision = 15f;

        [Space]
        [Header("Grid Settings")]
        public float colOffset;
        public float rowOffset;
        public GridPattern[] carSpawnPatterns;
        private GridSpawnLocations gridSpawn;

        [Space]
        [Header("Private Variables for internal Processing")]
        private List<ACarType> carsWaitingToMove;
        private Dictionary<string, ACarType> carsWhichCanMove;

        [Header("Internal variables used to sync car spawn")]
        private float minimumYPositionClosestCarHasToMoveBeforeSpawningNew = 3f;
        private float yPositionOfClosestCar = 0f;

        public Vector3 SpawnPosition => transform == null ?
            Vector3.zero : transform.position;

        public float ColOffset => colOffset;

        public float RowOffset => rowOffset;

        private void Awake()
        {
            gridSpawn = new GridSpawnLocations(this);
            carsWaitingToMove = new List<ACarType>();
            carsWhichCanMove = new Dictionary<string, ACarType>();

            minimumYPositionClosestCarHasToMoveBeforeSpawningNew = transform.position.y - rowOffset * 4;
        }

        private void Start()
        {
            foreach (var pooledObject in enemyCarsPool.InitializePools())
            {
                pauseController.AddNewPausableObject(pooledObject);
            }
        }

        private void FixedUpdate()
        {
            MoveAllMoveableCarsAndDeactivateWhichHaveReachedLimit();
        }

        public override void OnPause()
        {
            StopAllCoroutines();
            base.OnPause();
        }

        public override void OnPlay()
        {
            StartCoroutine("SpawnEnemyCars");
            base.OnPlay();
        }

        private IEnumerator SpawnEnemyCars()
        {
            // throw enemies after delay
            yield return new WaitForSecondsRealtime(waitTimeBeforeStarting.value);

            while (true)
            {
                FillCarsCanMoveDictionary();
                yield return new WaitForSecondsRealtime(waitTimeBetweenEachSpawn.value);
                if (!IsCurrentCarPatternMoving())
                {
                    continue;
                }
                if (!HasTheCurrentPatternMovedMinimumDistance())
                {
                    continue;
                }

                var pattern = SelectACarSpawnPattern();
                FillWaitingCarsList(pattern);

                if (carsWaitingToMove.Count >= pattern.NumberOfCellsWhichHaveCar)
                {
                    // this means we have enough cars to make the pattern
                    carsWaitingToMove.ForEach(c => enemyCarsPool.RecycleObject(c.Id));
                }
                else
                {
                    // don't have enough cars in pool to make the pattern
                    // deactivate the cars and wait for next cycle

                    carsWaitingToMove.ForEach(c => enemyCarsPool.UnlockObject(c));
                    carsWaitingToMove.Clear();
                }
            }
        }

        private void FillWaitingCarsList(GridPattern pattern)
        {
            var spawnLocations = gridSpawn.GenerateSpawnLocationsFor(pattern);

            for (int r = 0; r < spawnLocations.GetLength(0); r++)
            {
                for (int c = 0; c < spawnLocations.GetLength(1); c++)
                {
                    if (spawnLocations[r, c] == null)
                    {
                        continue;
                    }

                    var car = (ACarType)enemyCarsPool.LockObjectForRecycling(pattern.rows[r].cells[c].Id);
                    if (car == null)
                    {
                        Debug.Log("No more cars left in the pool");
                        continue;
                    }

                    car.transform.position = new Vector3(
                        spawnLocations[r, c].Value.x,
                        spawnLocations[r, c].Value.y,
                        0);
                    carsWaitingToMove.Add(car);
                }
            }
        }
       
        private void FillCarsCanMoveDictionary()
        {
            List<ACarType> carsWhichCanMoveNow = new List<ACarType>();
            foreach (var car in carsWaitingToMove)
            {
                if (car.IsCollisionPossibleTill(distanceInWhichToAvoidCollision))
                {
                    continue;
                }
                carsWhichCanMoveNow.Add(car);
            }

            if (WillAny3CarsBlockPlayer(carsWhichCanMoveNow))
            {
                return;
            }

            foreach (var car in carsWhichCanMoveNow)
            {
                if (!carsWhichCanMove.ContainsKey(car.name))
                {
                    carsWhichCanMove.Add(car.name, car);
                }
                carsWaitingToMove.Remove(car);
            }
        }

        private bool WillAny3CarsBlockPlayer(List<ACarType> carsWhichCanMoveNow)
        {
            // box in which 3 cars shouldn't be present simultaneously
            var miny = player.position.y - bufferInYaxis.value;
            var maxy = player.position.y + bufferInYaxis.value;

            // in this dictionary, for any key, value shouldn't be 3 or more
            Dictionary<string, int> KcarName_VnumberOfOverlaps = new Dictionary<string, int>();

            List<CarOverlapData> timeWindows = new List<CarOverlapData>();
            foreach (var car in carsWhichCanMoveNow.Union(carsWhichCanMove.Values))
            {
                // 1. in game cars are moving downwards
                // 2. miny is calculated by subtracting from player's position. 
                // this means that miny is actually the maximum distance for car.
                // and vice-versa
                var minDistanceTheCarNeedsToTravel = Math.Abs(maxy - car.transform.position.y);
                var maxDistanceTheCarNeedsToTravel = Math.Abs(miny - car.transform.position.y);

                // this is the time window for which car would be in the box
                // 3 of these windows shouldn't overlap.
                var minTime = minDistanceTheCarNeedsToTravel / car.Speed;
                var maxTime = maxDistanceTheCarNeedsToTravel / car.Speed;
                var newTimeWindow = new CarOverlapData
                {
                    id = car.Name,
                    xposition = car.transform.position.x,
                    max = maxTime,
                    min = minTime
                };

                foreach (var timeWindow in timeWindows)
                {
                    if (!newTimeWindow.CollidesWith(timeWindow))
                    {
                        continue;
                    }
                    if (!KcarName_VnumberOfOverlaps.ContainsKey(timeWindow.id))
                    {
                        KcarName_VnumberOfOverlaps.Add(timeWindow.id, 0);
                    }
                    KcarName_VnumberOfOverlaps[timeWindow.id]++;
                    if (KcarName_VnumberOfOverlaps[timeWindow.id] >= 3)
                    {
                        return true;
                    }
                }

                timeWindows.Add(newTimeWindow);
            }

            return false;
        }

        private GridPattern SelectACarSpawnPattern()
        {
            if (carSpawnPatterns == null || carSpawnPatterns.Length == 0)
            {
                return null;
            }

            var selectedPatternIndex = URandom.Range(0, carSpawnPatterns.Length);
            return carSpawnPatterns[selectedPatternIndex];
        }

        private bool IsCurrentCarPatternMoving()
        {
            if (carsWaitingToMove.Count == 0)
            {
                return true;
            }

            return false;
        }

        private bool HasTheCurrentPatternMovedMinimumDistance()
        {
            if (carsWhichCanMove.Count == 0)
            {
                return true;
            }    
            return yPositionOfClosestCar < minimumYPositionClosestCarHasToMoveBeforeSpawningNew;
        }

        private void MoveAllMoveableCarsAndDeactivateWhichHaveReachedLimit()
        {
            var carsWhichHaveReachedLimit = new List<ACarType>();
            yPositionOfClosestCar = -9999f;
            foreach (var car in carsWhichCanMove.Values)
            {
                car.MoveCarAndGetDistanceMoved();

                if (car.transform.position.y > yPositionOfClosestCar)
                {
                    yPositionOfClosestCar = car.transform.position.y;
                }

                if (car.transform.position.y < carTravelLimit.position.y)
                {
                    car.ResetCar();
                    enemyCarsPool.DeactivateObject(car);
                    carsWhichHaveReachedLimit.Add(car);
                }
            }

            carsWhichHaveReachedLimit.ForEach(c => carsWhichCanMove.Remove(c.name));
        }

        public void ReduceMultiplierToNormalAfterDelay(NearMissDetector nearMissDetector, float delay)
        {
            StartCoroutine(ExecuteAfterDelay(nearMissDetector, delay));
        }

        private IEnumerator ExecuteAfterDelay(NearMissDetector nearMissDetector, float timeInSeconds)
        {
            yield return new WaitForSecondsRealtime(timeInSeconds);

            nearMissDetector.ResetMultiplierValue();
        }

        private struct CarOverlapData
        {
            public string id;
            public float xposition;
            public float min;
            public float max;

            public bool CollidesWith(CarOverlapData timeWindow)
            {
                if (xposition == timeWindow.xposition)
                {
                    return false;
                }
                return IsSameAs(timeWindow) ?
                    true : OverlapsWith(timeWindow);
            }

            public bool OverlapsWith(CarOverlapData timeWindow)
            {
                if (max > timeWindow.min && max <= timeWindow.max)
                {
                    return true;
                }

                if (min >= timeWindow.min && min < timeWindow.max)
                {
                    return true;
                }

                return false;
            }

            public bool IsSameAs(CarOverlapData timeWindow)
            {
                return (min == timeWindow.min && max == timeWindow.max);
            }
        }
    }
}