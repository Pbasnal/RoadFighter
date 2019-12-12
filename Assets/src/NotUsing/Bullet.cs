using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10;
    public float coolDownInSec = 2;
    public float maxRange = 10;
    public Vector3 direction = Vector3.up;

    private Vector3 startPos;

    // Start is called before the first frame update
    private void Start()
    {
        Physics.IgnoreLayerCollision(8, 9);
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += direction * bulletSpeed * Time.deltaTime;

        var totalTravel = (transform.position - startPos).magnitude;
        if (totalTravel > maxRange)
        {
            Destroy(gameObject);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag != "Enemy")
        {
            return;
        }

        Destroy(collider.gameObject);
        Destroy(gameObject);
    }
}
