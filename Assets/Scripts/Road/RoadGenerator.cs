﻿using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObjectPool2 roadSectionPool;
    public int startingNumberOfSections = 3;

    private RoadSectionBehaviour latestRoadSection;

    // Use this for initialization
    private void Awake()
    {
        roadSectionPool.Init();
    }

    private void Start()
    {
        GameObject latest = null;
        Vector3 spawnPoint = transform.position;
        for (int i = 0; i < startingNumberOfSections; i++)
        {
            latest = roadSectionPool.RecycleObjectAt(spawnPoint);
            latest.transform.parent = transform;
            latestRoadSection = latest.GetComponent<RoadSectionBehaviour>();
            spawnPoint = latestRoadSection.newSpawnPosition[1].position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var roadSection = collision.gameObject.GetComponent<RoadSectionBehaviour>();

        if (roadSection == null)
        {
            return;
        }
        var newSection = roadSectionPool.RecycleObjectAt(latestRoadSection.newSpawnPosition[1].position);
        newSection.transform.parent = transform;
        latestRoadSection = newSection.GetComponent<RoadSectionBehaviour>();
    }
}
