using UnityEngine;
using System.Collections;

public class HeavyDamage : MonoBehaviour
{
    public FloatValue playerHealth;
    
    public float msTimePerDamage = 500;

    private float msTimeSinceLastDamage = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            playerHealth.value -= 8f;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.name == "Player" && msTimeSinceLastDamage >= msTimePerDamage)
        {
            playerHealth.value -= 1f;
            msTimeSinceLastDamage = 0;
        }

        msTimeSinceLastDamage += Time.deltaTime * 1000;
    }
}
