using System.Collections;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityCode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UnityCode
{
    public class GameManager : MonoBehaviour
    {
        public float startingLevelSpeed;

        public FloatValue playerHealth;
        public FloatValue levelSpeed;
        public FloatValue playerPoints;
        public FloatValue pointsMultiplier;
        public GamePauseController pauseController;

        private void Awake()
        {
            pauseController.FindAllPauseableObjects();
            pauseController.PauseGame();
        }

        // Use this for initialization
        private void Start()
        {
            playerHealth.value = 100;
            pointsMultiplier.value = 1;
            levelSpeed.value = startingLevelSpeed;
            playerPoints.value = 0;
        }

        public void StartGame()
        {
            pauseController.PlayGame();
        }

        // Update is called once per frame
        private void Update()
        {
            if (playerHealth.value <= 0)
            {
                pauseController.PauseGame();
                playerHealth.value = 100;
                StartCoroutine(RestartLevel());
            }
        }

        private IEnumerator RestartLevel()
        {
            yield return new WaitForSecondsRealtime(3);
            levelSpeed.value = startingLevelSpeed;
            playerPoints.value = 0;
            pointsMultiplier.value = 1;
            playerHealth.value = 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            yield return null;
        }
    }
}
