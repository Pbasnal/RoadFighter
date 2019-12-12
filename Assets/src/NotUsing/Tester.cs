using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour
{
    public Enemy car1;
    public Enemy car2;
    public CollisionTester collisionTester;
    public new Collider2D collider;

    // Use this for initialization
    void Start()
    {
        collisionTester.Init(collider, transform);
    }

    private void Update()
    {
        var result = collisionTester.WillCarsCollideOnScreen(car1, car2);
    }
}
