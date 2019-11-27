using UnityEngine;

public class HeavyDamage : MonoBehaviour
{
    public FloatValue playerHealth;

    public float msTimePerDamage = 500;

    private float msTimeSinceLastDamage = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            playerHealth.value -= 8f;
            Debug.Log("Player hit heavy");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && msTimeSinceLastDamage >= msTimePerDamage)
        {
            playerHealth.value -= 1f;
            msTimeSinceLastDamage = 0;
        }

        msTimeSinceLastDamage += Time.deltaTime * 1000;
    }
}
