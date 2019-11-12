using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public int laneNumber;

    public FloatValue playerHealth;
    public FloatValue levelSpeed;
    public bool isVisible = true;
    [Range(0.5f, 2)] public float speedMultipler;
    public float msTimePerDamage = 500;
    public float carSpeed => levelSpeed.value * speedMultipler;

    private float msTimeSinceLastDamage = 0;

    private void Start()
    {
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (carSpeed * Time.deltaTime), 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            playerHealth.value -= 2f;
            msTimeSinceLastDamage = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.name == "Player" && msTimeSinceLastDamage >= msTimePerDamage)
        {
            playerHealth.value -= 0.5f;
            msTimeSinceLastDamage = 0;
        }

        msTimeSinceLastDamage += Time.deltaTime * 1000;
    }
}
