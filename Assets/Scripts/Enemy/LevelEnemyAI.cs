using System.Collections;
using UnityEngine;

public class LevelEnemyAI : MonoBehaviour
{
    public BlockEnemyAI[] blockEnemyAIs;
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

            blockEnemyAIs[0].GenerateEnemySpawnContexts();

            blockEnemyAIs[0].FilterEnemies(distanceLimit.position);
            var blocking = blockEnemyAIs[0].WillEnemiesBlockPlayer(distanceLimit.position);

            if (!blocking)
            {
                blockEnemyAIs[0].SpawnEnemies();
            }
        }
    }
}
