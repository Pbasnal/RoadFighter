using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public RoadSpawn roadSectionSpwanPoint;
    public RoadDestroy roadSectionDestroyPoint;
    public FloatValue levelSpeed;
    public GameObject roadSection;

    public GameObject[] startingSections;

    public int activeQueueCount;
    public int inactiveQueueCount;

    private Queue<GameObject> activeRoadSectionPool;
    private Queue<GameObject> inactiveRoadSectionPool;

    // Start is called before the first frame update
    private void Start()
    {
        activeRoadSectionPool = new Queue<GameObject>();
        inactiveRoadSectionPool = new Queue<GameObject>();
        roadSectionSpwanPoint.Init(this);
        roadSectionDestroyPoint.Init(this);
        activeRoadSectionPool.Enqueue(Instantiate(roadSection,
            roadSectionSpwanPoint.transform.position, Quaternion.identity, transform));
    }

    private void Update()
    {
        activeQueueCount = activeRoadSectionPool.Count;
        inactiveQueueCount = inactiveRoadSectionPool.Count;
    }

    public void RoadSpawnStart(Collider2D collision)
    {
    }

    public void RoadSpawnComplete(Collider2D collision)
    {
        if (inactiveRoadSectionPool.Count == 0)
        {
            var spwanLocation = new Vector2(roadSectionSpwanPoint.transform.position.x, roadSectionSpwanPoint.transform.position.y);

            activeRoadSectionPool.Enqueue(Instantiate(roadSection, spwanLocation, Quaternion.identity, transform));
        }
        else
        {
            var roadSection = inactiveRoadSectionPool.Dequeue();
            roadSection.transform.position = roadSectionSpwanPoint.transform.position;
            roadSection.SetActive(true);
            activeRoadSectionPool.Enqueue(roadSection);
        }
    }

    public void RoadDestroyStart(Collider2D collision)
    {
        var roadSection = activeRoadSectionPool.Dequeue();
        roadSection.SetActive(false);
        inactiveRoadSectionPool.Enqueue(roadSection);
    }

    public void RoadDestroyComplete(Collider2D collision)
    {
    }
}
