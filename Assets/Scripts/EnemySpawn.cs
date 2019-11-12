using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoadLane
{
    [HideInInspector] public int laneNumber;
    [HideInInspector] public Enemy slowestEnemy;

    public Transform spawnPoint;
    public GameObjectPool gameObjectPool;    
    public Vector3 position => spawnPoint.position;
    public GameObject GetCar(string name) => gameObjectPool.RecycleObject(name);

    public Enemy GetCarInFront(LayerMask layer)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector3.down, 20, layer);
        slowestEnemy = null;
        if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            slowestEnemy = hit.collider.gameObject.GetComponent<Enemy>() ;
        }

        return slowestEnemy;
    }
}

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    public float minWaitTimeToSpawn = 1;
    public RoadLane[] roadLanes;
    public CollisionTester collisionTester;
    public LayerMask layer;

    private GameObject[] enemyTypes;
    private RoadLane fastestCar;
    private new Collider2D collider;
    private GameObject slowCar;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        GameObject[] instantiatedObjects = new GameObject[enemyPrefabs.Length];

        for (int i = 0; i < roadLanes.Length; i++)
        {
            for (int j = 0; j < enemyPrefabs.Length; j++)
            {
                instantiatedObjects[j] = Instantiate(enemyPrefabs[j], transform.position, Quaternion.identity, transform);
                instantiatedObjects[j].name = enemyPrefabs[j].name;
                instantiatedObjects[j].SetActive(false);
            }
            roadLanes[i].gameObjectPool.Init(instantiatedObjects);
            roadLanes[i].laneNumber = i;
        }
        collisionTester.Init(collider, transform);

        StartCoroutine(SpawnEnemy());
    }

    private void Update()
    {
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag != "Enemy")
        {
            return;
        }

        collider.gameObject.SetActive(false);

        for (int i = 0; i < roadLanes.Length; i++)
        {
            if (roadLanes[i].laneNumber == collider.gameObject.GetComponent<Enemy>().laneNumber)
            {
                var deactivatedObject = roadLanes[i].gameObjectPool.DeactivateTopObject(collider.gameObject.name);
                break;
            }
        }
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(minWaitTimeToSpawn);

            var numberOfLanesToUse = UnityEngine.Random.Range(1, roadLanes.Length);
            var randomLanes = GetRandomValuesFrom<RoadLane>(roadLanes, numberOfLanesToUse);
            var selectedCars = GetRandomValuesFrom<GameObject>(enemyPrefabs, numberOfLanesToUse);

            for (int i = 0; i < randomLanes.Length; i++)
            {
                var prevCar = randomLanes[i].GetCarInFront(layer);
                if (prevCar == null)
                {
                    SpawnCarOnRoad(randomLanes[i], numberOfLanesToUse, selectedCars[i]);
                    continue;
                }

                var car = selectedCars[i];
                if (prevCar != null
                    && collisionTester.WillCarsCollideOnScreen(car.GetComponent<Enemy>(), prevCar.GetComponent<Enemy>()))
                {
                    continue;
                }
                var carEnemy = SpawnCarOnRoad(randomLanes[i], numberOfLanesToUse, selectedCars[i]);
            }
        }
    }

    private Enemy GetFastestCar(GameObject[] gameObjects)
    {
        var slowestCar = gameObjects[0].GetComponent<Enemy>();
        for (int i = 1; i < gameObjects.Length; i++)
        {
            var car = gameObjects[i].GetComponent<Enemy>();
            if (slowestCar.carSpeed < car.carSpeed)
            {
                slowestCar = car;
            }
        }

        return slowestCar;
    }

    private Enemy GetSlowestCar(GameObject[] gameObjects)
    {
        var slowestCar = gameObjects[0].GetComponent<Enemy>();
        for (int i = 1; i < gameObjects.Length; i++)
        {
            var car = gameObjects[i].GetComponent<Enemy>();
            if (slowestCar.carSpeed > car.carSpeed)
            {
                slowestCar = car;
            }
        }

        return slowestCar;
    }

    private Enemy SpawnCarOnRoad(RoadLane randomLane, int numberOfLanesToUse, GameObject selectedCar)
    {
        var car = randomLane.GetCar(selectedCar.name);
        if (car == null)
        {
            car = Instantiate(selectedCar, randomLane.position, Quaternion.identity, transform);
            car.name = selectedCar.name;
            randomLane.gameObjectPool.AddActiveObject(car.name, car);
        }
        car.transform.position = randomLane.position;
        car.GetComponent<Enemy>().laneNumber = randomLane.laneNumber;
        car.SetActive(true);

        return car.GetComponent<Enemy>();
    }


    private T[] GetRandomValuesFrom<T>(T[] set, int numberOfValues)
    {
        if (numberOfValues > set.Length || numberOfValues < 0)
        {
            return set;
        }

        var chosenValues = new List<T>();

        for (int i = 0; i < numberOfValues; i++)
        {
            var selectedIndex = UnityEngine.Random.Range(0, set.Length);

            while (chosenValues.Contains(set[selectedIndex]))
            {
                selectedIndex = (selectedIndex + 1) % set.Length;
            }
            chosenValues.Add(set[selectedIndex]);
        }

        return chosenValues.ToArray();
    }
}
