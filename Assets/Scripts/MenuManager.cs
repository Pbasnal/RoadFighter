using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject inGameStats;

    public GameStateObject gameState;

    private void Start()
    {
        Time.timeScale = 0;
        gameState.gameState = GameState.Pause;
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
        inGameStats.SetActive(false);
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
        gameState.gameState = GameState.Pause;
        pauseMenu.SetActive(true);
        mainMenu.SetActive(false);
        inGameStats.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        gameState.gameState = GameState.Running;
        pauseMenu.SetActive(false);
        mainMenu.SetActive(false);
        inGameStats.SetActive(true);
    }

    public void Play()
    {
        Time.timeScale = 1;
        gameState.gameState = GameState.Running;
    }

    public void Quit()
    {
        
    }

    public void MainMenu()
    {
        Time.timeScale = 0;
        gameState.gameState = GameState.Pause;
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
        inGameStats.SetActive(false);
    }
}
