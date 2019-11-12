using UnityEngine;

public class RoadSpawn : MonoBehaviour
{
    public GameObject roadSection;

    private Road road;

    public void Init(Road road)
    {
        this.road = road;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        road.RoadSpawnStart(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        road.RoadSpawnComplete(collision);
    }
}
