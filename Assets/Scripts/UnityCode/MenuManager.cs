using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityCode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UnityCode
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject pauseMenu;
        public GameObject mainMenu;
        public GameObject inGameStats;
        public GamePauseController pauseController;
        public GameState gameState;

        private Scene gameScene;

        public void Play()
        {
            pauseMenu.SetActive(false);
            mainMenu.SetActive(false);
            inGameStats.SetActive(true);
            pauseController.PlayGame();
            gameState.State = States.Running;
        }
    }
}
