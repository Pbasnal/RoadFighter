using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;

namespace UnityCode
{
    public class GamePauseController : MonoBehaviour
    {
        public GameState gameState;

        private PausableBehaviour[] pausableBehaviours;

        private void Awake()
        {
            FindAllPauseableObjects();
        }

        public void FindAllPauseableObjects()
        {
            pausableBehaviours = FindObjectsOfType<PausableBehaviour>();
        }

        public void PauseGame()
        {
            gameState.State = States.Pause;
            foreach (PausableBehaviour behaviour in pausableBehaviours)
            {
                behaviour.OnPause();
                behaviour.enabled = false;
            }
        }

        public void PlayGame()
        {
            gameState.State = States.Running;

            foreach (PausableBehaviour behaviour in pausableBehaviours)
            {
                behaviour.enabled = true;
                behaviour.OnPlay();
            }
        }
    }
}
