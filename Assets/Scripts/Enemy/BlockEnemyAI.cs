using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnContext
{
    public int lane;
    public int enemyType;
    public bool isActive;

    public List<string> calculationLog;
}

public class BlockEnemyAI : MonoBehaviour
{
    public GameObjectPool2[] enemyPools;

    private Vector2[] spawnPoints;
    private EnemySpawnContext[] enemySpawnContexts;

    public Enemy fastestEnemy;
    public Enemy slowestEnemy;

    // Start is called before the first frame update
    private void Start()
    {
        spawnPoints = new Vector2[transform.childCount];
        enemySpawnContexts = new EnemySpawnContext[transform.childCount];

        int i = 0;
        foreach (Transform child in transform)
        {
            enemySpawnContexts[i] = new EnemySpawnContext { isActive = false };
            spawnPoints[i++] = child.position;
        }
    }

    // Update is called once per frame
    public EnemySpawnContext[] GenerateEnemySpawnContexts()
    {
        var lanes = spawnPoints.ToList();
        var numberOfLanesToUse = Random.Range(1, lanes.Count);

        for (int j = 0; j < enemySpawnContexts.Length; j++)
        {
            enemySpawnContexts[j].isActive = false;
        }

        var selectedx = new List<float>();
        for (int i = 0; i < numberOfLanesToUse; i++)
        {
            enemySpawnContexts[i].isActive = true;
            enemySpawnContexts[i].lane = Random.Range(0, lanes.Count);
            enemySpawnContexts[i].enemyType = Random.Range(0, enemyPools.Length);

            if (selectedx.Contains(spawnPoints[enemySpawnContexts[i].lane].x))
            {
                i--;
                continue;
            }
            selectedx.Add(spawnPoints[enemySpawnContexts[i].lane].x);

            lanes.RemoveAt(enemySpawnContexts[i].lane);
        }

        return enemySpawnContexts;
    }

    public void FilterEnemies(float collisionDistance)
    {
        for (int i = 0; i < enemySpawnContexts.Length; i++)
        {
            if (!enemySpawnContexts[i].isActive)
            {
                continue;
            }

            var enemyPool = enemyPools[enemySpawnContexts[i].enemyType];
            var spawnPoint = spawnPoints[enemySpawnContexts[i].lane];

            var hit = Physics2D.Raycast(spawnPoint, Vector2.down, 10, gameObject.layer);
            if (hit.collider == null || hit.collider.tag.Equals("Player"))
            {
                continue;
            }

            Debug.Log("Hit");


            float distance = Mathf.Abs(hit.point.y - spawnPoint.y);
            if (distance < collisionDistance)
            {
                enemySpawnContexts[i].isActive = false;
            }

            enemySpawnContexts[i].calculationLog.Add("h: " + hit.point.y);
            enemySpawnContexts[i].calculationLog.Add("s: " + spawnPoint.y);
            enemySpawnContexts[i].calculationLog.Add("d: " + distance);
            enemySpawnContexts[i].calculationLog.Add("cd: " + collisionDistance);
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawnContexts.Length; i++)
        {
            if (!enemySpawnContexts[i].isActive)
            {
                continue;
            }

            var enemyType = enemySpawnContexts[i].enemyType;
            var spawnPoint = spawnPoints[enemySpawnContexts[i].lane];

            var spawnedObject = enemyPools[enemyType].RecycleObjectAt(spawnPoint);
            spawnedObject.transform.parent = transform;

            var enemy = spawnedObject.GetComponent<Enemy>();
            enemy.pool = enemyPools[enemyType];

            if (enemySpawnContexts[i].calculationLog == null)
            {
                return;
            }

            enemy.calculationLog = new List<string>(enemySpawnContexts[i].calculationLog);
        }
    }
}
