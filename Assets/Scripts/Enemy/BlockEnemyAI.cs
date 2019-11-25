using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockEnemyAI : MonoBehaviour
{
    public GameObjectPool2 normalEnemies;
    public GameObjectPool2 fastEnemies;
    public GameObjectPool2 slowEnemies;

    private Vector2[] spawnPoints;

    public Enemy fastestEnemy;
    public Enemy slowestEnemy;

    // Start is called before the first frame update
    private void Start()
    {
        spawnPoints = new Vector2[transform.childCount];

        int i = 0;
        foreach (Transform child in transform)
        {
            spawnPoints[i++] = child.position;
        }
    }

    // Update is called once per frame
    public void SpawnEnemies()
    {
        var lanes = spawnPoints.ToList();
        var numberOfLanesToUse = Random.Range(1, lanes.Count);

        var selectedx = new List<float>();
        for (int i = 0; i < numberOfLanesToUse; i++)
        {
            var laneToUse = Random.Range(0, lanes.Count);
            SpawnAnEnemyAt(spawnPoints[laneToUse]);

            if (selectedx.Contains(spawnPoints[laneToUse].x))
            {
                i--;
                continue;
            }
            selectedx.Add(spawnPoints[laneToUse].x);

            lanes.RemoveAt(laneToUse);
        }

        var str = "";
        selectedx.ForEach(x => str += x + " - ");

        Debug.Log(str);
    }

    private void SpawnAnEnemyAt(Vector2 position)
    {
        var enemyToUse = Random.Range(0, 3);

        GameObject spawnedEnemy = null;
        switch (enemyToUse)
        {
            case 0:
                {
                    spawnedEnemy = normalEnemies.RecycleObjectAt(position);
                    var enemy = spawnedEnemy.GetComponent<Enemy>();
                    enemy.pool = normalEnemies;

                    if (fastestEnemy == null || fastestEnemy.speedMultipler < enemy.speedMultipler)
                    {
                        fastestEnemy = enemy;
                    }

                    if (slowestEnemy == null || slowestEnemy.speedMultipler > enemy.speedMultipler)
                    {
                        slowestEnemy = enemy;
                    }

                    break;
                }
            case 1:
                {
                    spawnedEnemy = fastEnemies.RecycleObjectAt(position);
                    var enemy = spawnedEnemy.GetComponent<Enemy>();
                    enemy.pool = fastEnemies;
                    if (fastestEnemy == null || fastestEnemy.speedMultipler < enemy.speedMultipler)
                    {
                        fastestEnemy = enemy;
                    }

                    break;
                }
            case 2:
                {
                    spawnedEnemy = slowEnemies.RecycleObjectAt(position);
                    var enemy = spawnedEnemy.GetComponent<Enemy>();
                    enemy.pool = slowEnemies;
                    if (slowestEnemy == null || slowestEnemy.speedMultipler > enemy.speedMultipler)
                    {
                        slowestEnemy = enemy;
                    }
                    break;
                }
        }

        if (spawnedEnemy != null)
        {
            spawnedEnemy.transform.parent = transform;
        }
    }
}
