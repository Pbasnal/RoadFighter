using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityCode.EnemyCar;
using UnityCode.Managers;
using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    public class Damage : MonoBehaviour
    {
        [Header("Player health and damage settings")]
        public FloatValue playerHealth;
        public float initialImpactDamage = 10f;
        public float continousCollisionDamage = 3f;

        [Space]
        [Header("Damge to self")]
        public float maxhealth = 100f;
        public float initialImpactDamageToTake = 30f;
        public float continousCollisionDamageToTake = 20f;

        [Space]
        [Header("Common settings to damage both player and self")]
        public float msTimePerDamage = 500;

        [Space]
        [Header("Child Components")]
        public NearMissDetector nearMissDetector;

        // private fields needed
        private float msTimeSinceLastDamage = 0;
        private float currentHealth;
        private AudioManager _audioManager;

        private void Start()
        {
            _audioManager = FindObjectOfType<AudioManager>();
            ResetDamageTaken();
        }

        public void ResetDamageTaken()
        {
            currentHealth = maxhealth;
            msTimeSinceLastDamage = 0;
            var collider = gameObject.GetComponent<BoxCollider2D>();
            collider.enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _audioManager.PlayAudio("Collision");
            if (!collision.gameObject.CompareTag("Player"))
            {
                return;
            }            
            playerHealth.value -= initialImpactDamage;
            currentHealth -= initialImpactDamageToTake;
            msTimeSinceLastDamage = 0;

            nearMissDetector.ResetMultiplierValue();
            //Handheld.Vibrate();

            if (currentHealth < 0)
            {
                Died();
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                return;
            }
            else if (msTimeSinceLastDamage < msTimePerDamage)
            {
                msTimeSinceLastDamage += Time.deltaTime * 1000;
                return;
            }

            playerHealth.value -= continousCollisionDamage;
            currentHealth -= continousCollisionDamageToTake;
            msTimeSinceLastDamage = 0;
            //Handheld.Vibrate();

            if (currentHealth < 0)
            {
                Died();
            }
        }

        private void Died()
        {
            // trigger destroyed animation

            // to disable all further collisions
            var collider = gameObject.GetComponent<BoxCollider2D>();
            collider.enabled = false;

            transform.position = new Vector2(transform.position.x + 2, transform.position.y);
        }
    }
}
