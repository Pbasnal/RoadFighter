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

    private void Update()
    {
        timeSinceLastBlock += Time.deltaTime;
        //Debug.DrawLine(transform.position, distanceLimit.position, Color.red);
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);

            var t1 = timeSinceLastBlock;

            var enemy = blockEnemyAIs[0].slowestEnemy;
            var d = distanceLimit.localPosition.y < 0 ?
                distanceLimit.localPosition.y * -1 : distanceLimit.localPosition.y;

            Debug.DrawLine(transform.position, new Vector2(1, transform.position.y - (minSpeedOfACar * t1)), Color.blue, 1f);

            var t2a = (d - (minSpeedOfACar * t1)) / minSpeedOfACar + t1;
            var t2b = (d / maxSpeedOfACar) + t1;

            if (t2b >= t2a - 1 && t2b <= t2a + 1)
            {
                continue;
            }

            blockEnemyAIs[0].SpawnEnemies();
            timeSinceLastBlock = 0;
        }
    }
}
