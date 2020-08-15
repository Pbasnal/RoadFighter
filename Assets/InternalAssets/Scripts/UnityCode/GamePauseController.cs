using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;
using UnityLogic.BehaviourInterface;

namespace UnityCode
{
    public class GamePauseController : MonoBehaviour
    {
        public GameState gameState;
        public bool playGameOnStart;

        private List<APausableBehaviour> APausableBehaviours;

        private void Awake()
        {
            FindAllPauseableObjects();
        }

        private void Start()
        {
            if (playGameOnStart)
            {
                PlayGame();
            }
        }

        public void FindAllPauseableObjects()
        {
           APausableBehaviours = FindObjectsOfType<APausableBehaviour>().ToList();
        }

        public void AddNewPausableObject(GameObject gObj)
        {
            var pausebehaviour = gObj.GetComponent<APausableBehaviour>();
            if (pausebehaviour == null)
            {
                return;
            }
            APausableBehaviours.Add(pausebehaviour);
        }

        public void PauseGame()
        {
            gameState.State = States.Pause;
            foreach (APausableBehaviour behaviour in APausableBehaviours)
            {
                behaviour.OnPause();
            }
        }

        public void PlayGame()
        {
            gameState.State = States.Running;

            foreach (APausableBehaviour behaviour in APausableBehaviours)
            {
                behaviour.OnPlay();
            }
        }
    }
}
