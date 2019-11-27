using UnityEngine;

public class LightDamage : MonoBehaviour
{
    public FloatValue playerHealth;

    public float msTimePerDamage = 500;

    private float msTimeSinceLastDamage = 0;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            playerHealth.value -= 2f;
            msTimeSinceLastDamage = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Player" && msTimeSinceLastDamage >= msTimePerDamage)
        {
            playerHealth.value -= 0.5f;
            msTimeSinceLastDamage = 0;
        }

        msTimeSinceLastDamage += Time.deltaTime * 1000;
    }
}
