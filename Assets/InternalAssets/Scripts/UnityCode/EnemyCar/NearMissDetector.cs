using Assets.Scripts.UnityLogic.ScriptableObjects;
using SpawnSystem.UnityCode;
using UnityEngine;

namespace UnityCode.EnemyCar
{
    public class NearMissDetector : MonoBehaviour
    {
        public EnemyController enemyController;
        public FloatValue playerPointsMultiplier;

        private float multiplierIncreasedBy = 0 ;

        private void Awake()
        {
            enemyController = FindObjectOfType<EnemyController>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
            {
                return;
            }

            playerPointsMultiplier.value += 0.5f;
            multiplierIncreasedBy = 0.5f;
            enemyController.ReduceMultiplierToNormalAfterDelay(this, 3);
        }

        public void ResetMultiplierValue()
        {
            playerPointsMultiplier.value -= multiplierIncreasedBy;
            multiplierIncreasedBy = 0;
        }
    }
}
