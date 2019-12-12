using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();

        if (enemy == null)
        {
            return;
        }

        enemy.pool.DeactivateObject(enemy.gameObject);
    }
}
