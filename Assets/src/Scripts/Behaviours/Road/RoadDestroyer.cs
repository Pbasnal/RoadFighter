using UnityEngine;

public class RoadDestroyer : MonoBehaviour
{
    public RoadSectionPool roadSectionPool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var sectionComponent = collision.gameObject.GetComponent<RoadSectionBehaviour>();

        if (sectionComponent == null || roadSectionPool == null)
        {
            return;
        }

        sectionComponent.SetActive(false);
        roadSectionPool.DeactivateObject(sectionComponent);
    }
}
