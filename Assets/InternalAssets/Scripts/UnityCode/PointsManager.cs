using System.Collections;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;
using UnityLogic.BehaviourInterface;

namespace UnityCode
{
    public class PointsManager : APausableBehaviour
    {
        public float basePointValue;
        public float waitTimeBeforeIncreasingPointsInSeconds = 2;

        public FloatValue playerHealth;
        public FloatValue levelSpeed;
        public FloatValue playerPoints;
        public FloatValue pointsMultiplier;

        public override void OnPlay()
        {
            StartCoroutine(IncreasePoints());
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
    }
}
