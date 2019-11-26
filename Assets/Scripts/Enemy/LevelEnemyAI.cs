using System.Collections;
using UnityEngine;

public class LevelEnemyAI : MonoBehaviour
{
    public BlockEnemyAI[] blockEnemyAIs;
    public FloatValue levelSpeed;
    public Transform distanceLimit;

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

            blockEnemyAIs[0].GenerateEnemySpawnContexts();
            var distance = Mathf.Abs(distanceLimit.position.y - transform.position.y);
            blockEnemyAIs[0].FilterEnemies(distance);
            blockEnemyAIs[0].SpawnEnemies();
        }
    }
}
