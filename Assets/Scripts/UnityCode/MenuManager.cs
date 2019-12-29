using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UnityCode
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject pauseMenu;
        public GameObject mainMenu;
        public GameObject inGameStats;

        public GameState gameState;

        private Scene gameScene;

        private void Start()
        {
            Time.timeScale = 0;
            gameState.State = States.Pause;
            pauseMenu.SetActive(false);
            mainMenu.SetActive(true);
            inGameStats.SetActive(false);
            gameScene = SceneManager.GetActiveScene();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale > 0)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            }
        }

        public void Pause()
        {
            Time.timeScale = 0;
            gameState.State = States.Pause;
            pauseMenu.SetActive(true);
            mainMenu.SetActive(false);
            inGameStats.SetActive(false);
        }

        public void Resume()
        {
            Time.timeScale = 1;
            gameState.State = States.Running;
            pauseMenu.SetActive(false);
            mainMenu.SetActive(false);
            inGameStats.SetActive(true);
        }

        public void Play()
        {
            pauseMenu.SetActive(false);
            mainMenu.SetActive(false);
            inGameStats.SetActive(true);
            Time.timeScale = 1;
            gameState.State = States.Running;
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void MainMenu()
        {
            Time.timeScale = 0;
            gameState.State = States.NotStarted;
            SceneManager.LoadScene(gameScene.name);


            pauseMenu.SetActive(false);
            mainMenu.SetActive(true);
            inGameStats.SetActive(false);
        }

    }
}
