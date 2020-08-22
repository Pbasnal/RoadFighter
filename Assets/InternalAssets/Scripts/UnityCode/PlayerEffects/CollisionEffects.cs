using UnityCode.CameraScripts;
using UnityCode.Managers;
using UnityEngine;
using static UnityCode.CameraScripts.CameraShaker;

namespace UnityCode.PlayerEffects
{
    public class CollisionEffects : MonoBehaviour
    {
        public ParticleSystem sideCollision;
        public ParticleSystem headOnCollisionParticles;
        public CameraShaker cameraShaker;
        public ShakeParameters shakeParameters;

        private AudioManager _audioManager;
        private Vector3 rightRotationForSideCollision = new Vector3(0, 0, 270);
        private Vector3 leftRotationForSideCollision = new Vector3(0, 0, 170);

        private void Awake()
        {
            _audioManager = FindObjectOfType<AudioManager>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _audioManager.PlayAudio("Collision");
            if (!collision.gameObject.CompareTag("Enemy"))
            {
                return;
            }

            var collisionSide = collision.transform.position.x - transform.position.x;
            if (collisionSide > 0.35f)
            {
                var shape = sideCollision.shape;
                shape.rotation = rightRotationForSideCollision;

                sideCollision.Play();
            }
            else if (collisionSide < -0.35f)
            {
                var shape = sideCollision.shape;
                shape.rotation = leftRotationForSideCollision;

                sideCollision.Play();
            }
            else
            {
                headOnCollisionParticles.Play();
            }
            //Handheld.Vibrate();
            cameraShaker.ShakeCamera(shakeParameters);
            cameraShaker.CameraDragTo(0.95f, 0.5f);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Enemy"))
            {
                return;
            }
            //Handheld.Vibrate();
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Enemy"))
            {
                return;
            }

            cameraShaker.ResetCameraScreenPosition(duration: 1);
        }
    }
}
