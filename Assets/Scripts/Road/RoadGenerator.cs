using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObjectPool2 roadSectionPool;
    public MultilaneRoad road;

    // Use this for initialization
    private void Awake()
    {
        roadSectionPool.Init();
    }

    private void Start()
    {
        var selectedPositions = SelectRoadSections();

        for (int i = 0; i < selectedPositions.Length; i++)
        {
            roadSectionPool.RecycleObjectAt(new Vector2(road.lanePositions[selectedPositions[i]].x,
                transform.position.y));
        }
    }

    private int[] SelectRoadSections()
    {
        var numberOfSections = UnityEngine.Random.Range(1, road.lanePositions.Length + 1);

        List<int> positions = new List<int>(3) { 0, 1, 2 };
        List<int> selectedPositions = new List<int>(numberOfSections);

        for (int i = 0; i < numberOfSections; i++)
        {
            try
            {
                var j = UnityEngine.Random.Range(0, positions.Count);
                selectedPositions.Add(positions[j]);

                positions.RemoveAt(j);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        Debug.Log("NS: " + numberOfSections);
        if (numberOfSections == 1 && selectedPositions.Count > 1)
        {
            Debug.Log("Locha hai");
        }

        return selectedPositions.ToArray();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var roadSection = collision.gameObject.GetComponent<RoadSectionBehaviour>();

        var selectedPositions = SelectRoadSections();

        for (int i = 0; i < selectedPositions.Length; i++)
        {
            roadSectionPool.RecycleObjectAt(new Vector2(road.lanePositions[selectedPositions[i]].x,
                roadSection.newSpawnPosition[1].position.y));
        }
    }
}
