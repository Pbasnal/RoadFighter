using System.Collections.Concurrent;
using UnityEngine;

public class RoadController : IRoadGenerator
{
    public IObjectPool<IRoadSection> RoadSectionPool { get; }
    public int StartingNumberOfSections { get; } = 3;
    public FloatValue LevelSpeed { get; }

    public IGameObjectFactory<IRoadSection> roadSectionFactory;

    private IRoadSection latestRoadSection;
    private ConcurrentQueue<bool> sectionSpawnchannel;

    public RoadController(IGameObjectFactory<IRoadSection> roadSectionFactory, IObjectPool<IRoadSection> roadSectionPool, FloatValue levelSpeed)
    {
        LevelSpeed = levelSpeed;
        RoadSectionPool = roadSectionPool;
        sectionSpawnchannel = new ConcurrentQueue<bool>();
        this.roadSectionFactory = roadSectionFactory;
    }

    public IRoadSection[] InitializeSections(Vector2 initialSpawnPosition, int startingNumberOfSections)
    {
        Vector3 spawnPoint = initialSpawnPosition;
        var roadSections = roadSectionFactory.GetMultipleRoadSections(startingNumberOfSections);

        for (int i = 0; i < startingNumberOfSections; i++)
        {
            if (roadSections[i] == null)
            {
                continue;
            }
            latestRoadSection = roadSections[i];
            latestRoadSection.SetActive(true);
            latestRoadSection.SetPosition(spawnPoint);
            RoadSectionPool.AddActiveObject(latestRoadSection);

            spawnPoint = latestRoadSection.NewSpawnPosition;
        }

        return roadSections;
    }

    public IRoadSection[] SpawnNewSectionIfAny()
    {
        IRoadSection[] roadSections = new IRoadSection[sectionSpawnchannel.Count];
        for (int i = 0; i < sectionSpawnchannel.Count; i++)
        {
            sectionSpawnchannel.TryDequeue(out bool res);
            if (!RoadSectionPool.RecycleObject(out var roadSection))
            {
                roadSection = roadSectionFactory.GetRoadSection();
            }
            roadSection.SetPosition(latestRoadSection.NewSpawnPosition);
            roadSection.SetActive(true);
            latestRoadSection = roadSection;
            roadSections[i] = roadSection;
        }

        return roadSections;
    }

    public void MoveActiveSection(IRoadSection roadSection)
    {
        roadSection.SetPosition(new Vector2(roadSection.Position.x,
            roadSection.Position.y - (LevelSpeed.value * Time.deltaTime)));
    }

    public void EnqueueForRecycle(IRoadSection roadSection)
    {
        sectionSpawnchannel.Enqueue(true);
    }
}
