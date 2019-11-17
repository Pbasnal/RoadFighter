using UnityEngine;
using System.Collections;

public class RoadGenerator : MonoBehaviour
{
    public GameObjectPool2 roadSectionPool;

    // Use this for initialization
    private void Awake()
    {
        roadSectionPool.Init();
    }

    private void Start()
    {
        var newSection = roadSectionPool.RecycleObject();
        newSection.transform.position = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var roadSection = collision.gameObject.GetComponent<RoadSectionBehaviour>();
        
        roadSectionPool.RecycleObjectAt(roadSection.newSpawnPosition[1].position);
    }
}
