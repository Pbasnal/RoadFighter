using System.Collections.Concurrent;
using UnityEngine;

public class RoadGenerator : MonoBehaviour, IGenerateRoad
{
    public GameObjectPool roadSectionPool;
    public int startingNumberOfSections = 3;
    public FloatValue levelSpeed;

    public GameObjectPool RoadSectionPool { get => roadSectionPool; set { value = roadSectionPool; } }
    public int StartingNumberOfSections { get => startingNumberOfSections; set { value = startingNumberOfSections; } }
    public FloatValue LevelSpeed { get => levelSpeed; set { value = levelSpeed; } }
    public Vector2 Position => transform.position;

    public Transform Transform => transform;

    private RoadGeneratorController roadGeneratorController;

    // Use this for initialization
    private void Awake()
    {
        roadGeneratorController = new RoadGeneratorController(this);
    }

    private void Start()
    {
        roadGeneratorController.Init();
    }

    private void FixedUpdate()
    {
        roadGeneratorController.FixedUpdate();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //var roadSection = collision.gameObject.GetComponent<RoadSectionBehaviour>();

        //if (roadSection == null)
        //{
        //    return;
        //}

        //sectionSpawnchannel.Enqueue(true);

        //var newSection = roadSectionPool.RecycleObjectAt(latestRoadSection.newSpawnPosition[1].position);
        //newSection.transform.parent = transform;
        //latestRoadSection = newSection.GetComponent<RoadSectionBehaviour>();
    }
}
