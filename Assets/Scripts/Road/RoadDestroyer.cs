using UnityEngine;

public class RoadDestroyer : MonoBehaviour
{
    public GameObjectPool2 roadSectionPool;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        roadSectionPool.DeactivateObject(collision.gameObject);
    }
}
