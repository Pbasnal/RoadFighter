using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityCode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityLogic.ScriptableObjects;

namespace Assets.Scripts.UnityCode
{
    public class GameManager : MonoBehaviour
    {
        public float startingLevelSpeed;

        public FloatValue playerHealth;
        public FloatValue levelSpeed;
        public FloatValue playerPoints;
        public FloatValue pointsMultiplier;
        public FloatValueWithHistory highScore;
        public GamePauseController pauseController;

        public AnimationPlayer animationPlayer;
        public string animationToPlay;

        private bool playerIsAlreadyDead = false;

        private void Awake()
        {
            pauseController.FindAllPauseableObjects();
            pauseController.PauseGame();
            highScore?.Init();
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
            if (playerHealth.value <= 0 && !playerIsAlreadyDead)
            {
                pauseController.PauseGame();
                playerIsAlreadyDead = true;
                highScore?.AddHistoryObject(playerPoints.value);
                // play high score animation
                animationPlayer?.PlayFromBegining(animationToPlay);
                //playerHealth.value = 100;
                //StartCoroutine(RestartLevel());
            }
        }

        public void RestartLevel()
        {
            //yield return new WaitForSecondsRealtime(3);
            levelSpeed.value = startingLevelSpeed;
            playerIsAlreadyDead = false;
            playerPoints.value = 0;
            pointsMultiplier.value = 1;
            playerHealth.value = 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //yield return null;
        }
    }
}
