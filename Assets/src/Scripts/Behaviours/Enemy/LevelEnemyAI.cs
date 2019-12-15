using System.Collections;
using UnityEngine;

public class LevelEnemyAI : MonoBehaviour
{
    public EnemyAI[] blockEnemyAis;
    public FloatValue levelSpeed;
    public Transform distanceLimit;
    public GameStateObject gameState;

    public float minSpeedOfACar = 5 * 0.5f;
    public float maxSpeedOfACar = 5 * 1.5f;

    //public float distanceLimit = 5.0f;

    public float timeSinceLastBlock = 0;

    // Use this for initialization
    private void Start()
    {
        minSpeedOfACar = levelSpeed.value * 0.5f;
        maxSpeedOfACar = levelSpeed.value * 1.5f;

        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);

            if (gameState.gameState == GameState.Pause)
            {
                continue;
            }

            blockEnemyAis[0].SpawnEnemies(distanceLimit.position);
        }
    }
}
