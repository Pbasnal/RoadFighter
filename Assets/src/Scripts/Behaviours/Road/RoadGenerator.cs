using System.Collections.Concurrent;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObjectPool2 roadSectionPool;
    public int startingNumberOfSections = 3;
    public FloatValue levelSpeed;

    private RoadSectionBehaviour latestRoadSection;
    private ConcurrentQueue<bool> sectionSpawnchannel;
    // Use this for initialization
    private void Awake()
    {
        sectionSpawnchannel = new ConcurrentQueue<bool>();
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

    private void FixedUpdate()
    {
        if (transform.childCount == 0)
        {
            return;
        }

        while (sectionSpawnchannel.Count > 0)
        {
            sectionSpawnchannel.TryDequeue(out bool res);
            var newSection = roadSectionPool.RecycleObjectAt(latestRoadSection.newSpawnPosition[1].position);
            newSection.transform.parent = transform;
            latestRoadSection = newSection.GetComponent<RoadSectionBehaviour>();
        }

        foreach (Transform roadSection in transform)
        {
            if (!roadSection.gameObject.activeInHierarchy)
            {
                continue;
            }

            roadSection.position = new Vector2(roadSection.position.x, roadSection.position.y - (levelSpeed.value * Time.deltaTime));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var roadSection = collision.gameObject.GetComponent<RoadSectionBehaviour>();

        if (roadSection == null)
        {
            return;
        }

        sectionSpawnchannel.Enqueue(true);

        //var newSection = roadSectionPool.RecycleObjectAt(latestRoadSection.newSpawnPosition[1].position);
        //newSection.transform.parent = transform;
        //latestRoadSection = newSection.GetComponent<RoadSectionBehaviour>();
    }
}
