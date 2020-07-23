using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    public class Damage : MonoBehaviour
    {
        public FloatValue playerHealth;

        public float msTimePerDamage = 500;

        private float msTimeSinceLastDamage = 0;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                playerHealth.value -= 2f;
                msTimeSinceLastDamage = 0;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.name == "Player" && msTimeSinceLastDamage >= msTimePerDamage)
            {
                playerHealth.value -= 0.5f;
                msTimeSinceLastDamage = 0;
            }

            msTimeSinceLastDamage += Time.deltaTime * 1000;
        }
    }
}
