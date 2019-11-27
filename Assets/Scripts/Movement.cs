using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int moveUnits = 1;
    public int movementRange = 1;
    public int moveSpeed = 1;
    public float coolDownInSec = 2;
    public float maxHealth = 100;
    public FloatValue health;

    public Transform bulletSpawnPoint;
    public Bullet bullet;

    public int currentPositionInUnits;
    private int direction = 0;
    private bool canShoot = true;

    // Start is called before the first frame update
    private void Start()
    {
        health.value = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        //health.value = maxHealth;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentPositionInUnits > movementRange * -1)
        {
            currentPositionInUnits -= moveUnits;
            direction = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentPositionInUnits < movementRange)
        {
            currentPositionInUnits += moveUnits;
            direction = 1;
        }

        if (canShoot && Input.GetKeyDown(KeyCode.Space))
        {
            canShoot = false;
            Instantiate(bullet.gameObject, bulletSpawnPoint.position, Quaternion.identity);
            StartCoroutine(Cooldown());
        }

        switch (direction)
        {
            case -1:
                if (currentPositionInUnits >= transform.position.x)
                {
                    transform.position = new Vector3(currentPositionInUnits, transform.position.y, 0);
                    direction = 0;
                }
                break;
            case 1:
                if (currentPositionInUnits <= transform.position.x)
                {
                    transform.position = new Vector3(currentPositionInUnits, transform.position.y, 0);
                    direction = 0;
                }
                break;
        }

        transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime * direction, transform.position.y, 0);
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSecondsRealtime(coolDownInSec);
        canShoot = true;
    }
}