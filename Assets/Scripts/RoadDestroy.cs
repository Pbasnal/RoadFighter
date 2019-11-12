using UnityEngine;

public class RoadDestroy : MonoBehaviour
{
    private Road road;

    public void Init(Road road)
    {
        this.road = road;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        road.RoadDestroyStart(collision);
    }
}
