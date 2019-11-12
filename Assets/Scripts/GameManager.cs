using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float startingLevelSpeed;
    public float basePointValue;

    public FloatValue playerHealth;
    public FloatValue levelSpeed;
    public FloatValue playerPoints;
    public FloatValue pointsMultiplier;

    public Text pointsText;
    public Text healthText;
    public Text multiplierText;

    // Use this for initialization
    private void Start()
    {
        levelSpeed.value = startingLevelSpeed;
        playerPoints.value = 0;
        StartCoroutine(IncreasePoints());
        StartCoroutine(IncreaseLevelSpeed());

        Time.timeScale = 0;
    }

    private IEnumerator IncreasePoints()
    {
        while (playerHealth.value > 0)
        {
            playerPoints.value += basePointValue * pointsMultiplier.value;
            yield return new WaitForSecondsRealtime(1 / levelSpeed.value * 10);
        }

        yield return null;
    }

    private IEnumerator IncreaseLevelSpeed()
    {
        while (playerHealth.value > 0)
        {
            levelSpeed.value += 0.1f;
            yield return new WaitForSecondsRealtime(2);
        }

        yield return null;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        pointsText.text = playerPoints.value.ToString();
        healthText.text = playerHealth.value.ToString();
        multiplierText.text = pointsMultiplier.value.ToString();

        if (playerHealth.value <= 0)
        {
            Time.timeScale = 0;
            StartCoroutine(RestartLevel());
        }
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSecondsRealtime(3);
        levelSpeed.value = startingLevelSpeed;
        playerPoints.value = 0;
        playerHealth.value = 100;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartCoroutine(IncreasePoints());
        StartCoroutine(IncreaseLevelSpeed());
        Time.timeScale = 1;
        yield return null;
    }
}
