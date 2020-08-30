using System;
using System.Collections;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;
using UnityLogic.BehaviourInterface;

namespace UnityCode
{
    public class PointsManager : APausableBehaviour
    {
        public float basePointValue;
        public float waitTimeBeforeIncreasingPointsInSeconds = 0.5f;
        public float waitTimeBeforeIncreasingLevelSpeedInSeconds = 2;
        public float maxLevelSpeed = 10;

        public FloatValue playerHealth;
        public FloatValue levelSpeed;
        public FloatValue playerPoints;
        public FloatValue pointsMultiplier;

        public override void OnPlay()
        {
            StartCoroutine(IncreasePoints());
            StartCoroutine(IncreaseLevelSpeed());
        }

        public override void OnPause()
        {
            StopAllCoroutines();
        }

        private IEnumerator IncreasePoints()
        {
            while (playerHealth.value > 0)
            {
                playerPoints.value += basePointValue * pointsMultiplier.value;
                levelSpeed.value += 0.05f;
                yield return new WaitForSecondsRealtime(waitTimeBeforeIncreasingPointsInSeconds);
            }

            yield break;
        }

        private IEnumerator IncreaseLevelSpeed()
        {
            while (playerHealth.value > 0)
            {
                if (levelSpeed.value >= maxLevelSpeed)
                {
                    break;
                }
                levelSpeed.value += 0.05f;
                yield return new WaitForSecondsRealtime(waitTimeBeforeIncreasingLevelSpeedInSeconds);
            }

            yield break;
        }
    }
}