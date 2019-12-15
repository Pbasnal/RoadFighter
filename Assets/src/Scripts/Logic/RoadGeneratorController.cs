using System.Collections.Concurrent;
using UnityEngine;

public class RoadGeneratorController
{
    private IGenerateRoad roadGenerator;

    private RoadSectionBehaviour latestRoadSection;
    private ConcurrentQueue<bool> sectionSpawnchannel;

    public RoadGeneratorController(IGenerateRoad roadGenerator)
    {
        this.roadGenerator = roadGenerator;
        sectionSpawnchannel = new ConcurrentQueue<bool>();
        this.roadGenerator.RoadSectionPool.Init();
    }

    public void Init()
    {
        GameObject latest = null;
        Vector2 spawnPoint = roadGenerator.Position;

        for (int i = 0; i < roadGenerator.StartingNumberOfSections; i++)
        {
            latest = roadGenerator.RoadSectionPool.RecycleObjectAt(spawnPoint);
            latest.transform.parent = roadGenerator.Transform;
            latestRoadSection = latest.GetComponent<RoadSectionBehaviour>();
            spawnPoint = latestRoadSection.NewSpawnPosition;
        }
    }

    public void FixedUpdate()
    {
        if (roadGenerator.Transform.childCount == 0)
        {
            return;
        }

        while (sectionSpawnchannel.Count > 0)
        {
            sectionSpawnchannel.TryDequeue(out bool res);
            var newSection = roadGenerator.RoadSectionPool.RecycleObjectAt(latestRoadSection.NewSpawnPosition);
            newSection.transform.parent = roadGenerator.Transform;
            latestRoadSection = newSection.GetComponent<RoadSectionBehaviour>();
        }

        foreach (Transform roadSection in roadGenerator.Transform)
        {
            if (!roadSection.gameObject.activeInHierarchy)
            {
                continue;
            }

            roadSection.position = new Vector2(roadSection.position.x, roadSection.position.y - (roadGenerator.LevelSpeed.value * Time.deltaTime));
        }
    }
}
