using System.Collections.Generic;
using UnityEngine;

namespace UnityCode
{
    public class GamePauseController : MonoBehaviour
    {
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
            foreach (PausableBehaviour behaviour in pausableBehaviours)
            {
                behaviour.OnPause();
                behaviour.enabled = false;
            }
        }

        public void PlayGame()
        {
            foreach (PausableBehaviour behaviour in pausableBehaviours)
            {
                behaviour.enabled = true;
                behaviour.OnPlay();
            }
        }
    }
}
