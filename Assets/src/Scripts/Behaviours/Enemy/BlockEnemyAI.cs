using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnContext
{
    public int lane;
    public int enemyType;
    public float collisionDistance;
    public bool spawnCar;
    public Enemy enemy;

    public List<string> calculationLog = new List<string>();
}

public class BlockEnemyAI : MonoBehaviour
{
    public GameObjectPool2[] enemyPools;
    public LayerMask layerOfEnemyCars;

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
            enemySpawnContexts[i] = new EnemySpawnContext { spawnCar = false };
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
            enemySpawnContexts[j].spawnCar = false;
            enemySpawnContexts[j].enemy = null;
            enemySpawnContexts[j].collisionDistance = 0;
        }

        var selectedx = new List<float>();
        for (int i = 0; i < numberOfLanesToUse; i++)
        {
            enemySpawnContexts[i].spawnCar = true;
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

    public void FilterEnemies(Vector3 collisionPosition)
    {        
        for (int i = 0; i < enemySpawnContexts.Length; i++)
        {
            //if (!enemySpawnContexts[i].spawnCar)
            //{
            //    continue;
            //}

            var enemyPool = enemyPools[enemySpawnContexts[i].enemyType];
            var spawnPoint = spawnPoints[enemySpawnContexts[i].lane];

            var collisionDistance = Mathf.Abs(collisionPosition.y - spawnPoint.y);
            var hit = Physics2D.Raycast(spawnPoint, Vector2.down, collisionDistance, layerOfEnemyCars);
            Debug.DrawLine(spawnPoint, spawnPoint + Vector2.down * collisionDistance, Color.red, 1);
            if (hit.collider == null || !hit.collider.tag.Equals("Enemy"))
            {
                Debug.Log("Enemy not hit: " + hit.collider?.tag);
                continue;
            }

            Debug.Log("Hit - " + hit.collider.tag);

            float distance = Mathf.Abs(hit.point.y - spawnPoint.y);
            Debug.DrawLine(spawnPoint + new Vector2(0.1f, 0), spawnPoint + new Vector2(0.1f, -1 * distance), Color.blue, 1);

            if (distance < collisionDistance)
            {
                enemySpawnContexts[i].spawnCar = false;
            }

            enemySpawnContexts[i].collisionDistance = distance;
            enemySpawnContexts[i].enemy = hit.collider.GetComponent<Enemy>();
            enemySpawnContexts[i].calculationLog.Add("h: " + hit.point.y);
            enemySpawnContexts[i].calculationLog.Add("s: " + spawnPoint.y);
            enemySpawnContexts[i].calculationLog.Add("d: " + distance);
            enemySpawnContexts[i].calculationLog.Add("cd: " + collisionDistance);
        }
    }

    public bool WillEnemiesBlockPlayer(Vector3 collisionPosition)
    {
        bool blocking = true;

        var timeToReach = new float[enemySpawnContexts.Length];
        for (int i = 0; i < enemySpawnContexts.Length; i++)
        {
            var collisionDistance = Mathf.Abs(spawnPoints[i].y - collisionPosition.y);
            if (enemySpawnContexts[i].spawnCar)
            {
                var carToSpawn = enemyPools[enemySpawnContexts[i].enemyType].prefab.GetComponent<Enemy>();
                timeToReach[i] = collisionDistance / carToSpawn.carSpeed;
            }
            else if (enemySpawnContexts[i].enemy != null)
            {
                timeToReach[i] = Mathf.Abs(collisionDistance - enemySpawnContexts[i].collisionDistance) / enemySpawnContexts[i].enemy.carSpeed;
            }
        }

        var prevTime = timeToReach[0];
        for (int i = 1; i < timeToReach.Length; i++)
        {
            if (Mathf.Abs(timeToReach[i] - prevTime) > 0.1)
            {
                blocking = false;
                break;
            }
        }

        return blocking;
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawnContexts.Length; i++)
        {
            if (!enemySpawnContexts[i].spawnCar)
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
