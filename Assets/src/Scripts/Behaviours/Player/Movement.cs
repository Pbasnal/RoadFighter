using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IMove
{
    public int moveUnits = 1;
    public int movementRange = 1;
    public int moveSpeed = 1;
    public int currentPositionInUnits;
    public int newPositionInUnits;

    public int MoveUnits { get => moveUnits; set => moveUnits = value; }
    public int MovementRange { get => movementRange; set => movementRange = value; }
    public int MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public int CurrentPositionInUnits { get => currentPositionInUnits; set => currentPositionInUnits = value; }
    public int PreviousPositionInUnits { get => newPositionInUnits; set => newPositionInUnits = value; }

    public float coolDownInSec = 2;
    public float maxHealth = 100;
    public FloatValue health;

    public Transform bulletSpawnPoint;
    //public Bullet bullet;

    private int direction = 0;
    private bool canShoot = true;

    private PlayerMovementController playerMovementController;

    private IDictionary<KeyCode, Func<int>> commandMap;

    private void Awake()
    {
        playerMovementController = new PlayerMovementController(this);
        commandMap = new Dictionary<KeyCode, Func<int>>();
        commandMap.Add(KeyCode.LeftArrow, playerMovementController.MoveLeft);
        commandMap.Add(KeyCode.RightArrow, playerMovementController.MoveRight);
        commandMap.Add(KeyCode.LeftWindows, DontMove);
    }

    private int DontMove()
    {
        return direction;
    }

    private KeyCode GetKeyCode()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("LHit");
            return KeyCode.LeftArrow;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("RHit");
            return KeyCode.RightArrow;
        }
        else
        {
            Debug.Log("NoHit");
            return KeyCode.LeftWindows;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        health.value = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        direction = commandMap[GetKeyCode()]();
        transform.position = 
            new Vector3(transform.position.x + moveSpeed * Time.deltaTime * direction, 
            transform.position.y, 0);
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSecondsRealtime(coolDownInSec);
        canShoot = true;
    }
}