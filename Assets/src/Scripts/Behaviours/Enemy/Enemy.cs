using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject
{
    [HideInInspector] public int laneNumber;
    [HideInInspector] public List<string> calculationLog;

    public FloatValue playerHealth;
    public FloatValue levelSpeed;
    public bool isVisible = true;
    [Range(0.5f, 2)] public float speedMultipler;
    public float msTimePerDamage = 500;
    public float carSpeed => levelSpeed.value * speedMultipler;

    public GameObjectPool pool { get; set; }

    private float msTimeSinceLastDamage = 0;

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (carSpeed * Time.deltaTime), 0);
        }
    }
}
