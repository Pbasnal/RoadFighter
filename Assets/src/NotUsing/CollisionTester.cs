using UnityEngine;

[CreateAssetMenu(fileName = "CollisionTester", menuName = "Level/CollisionTester", order = 52)]
public class CollisionTester : ScriptableObject
{
    public Enemy car1;
    public Enemy car2;

    private Collider2D collider;
    private Transform transform;

    public void Init(Collider2D collider, Transform transform)
    {
        this.collider = collider;
        this.transform = transform;
    }

    public bool TestPrevious()
    {
        var fastCar = car1;
        var slowCar = car2;
        string debugStr;

        if (car1 == null || car2 == null) return false;

        var relativeSpeed = fastCar.carSpeed - slowCar.carSpeed;
        debugStr = "FC: " + fastCar.carSpeed + " SC: " + slowCar.carSpeed;

        if (relativeSpeed <= 0)
        {
            //Debug.Log(debugStr);
            return false;
        }

        var distanceBetweenCars = fastCar.gameObject.transform.localPosition.y - slowCar.gameObject.transform.localPosition.y;
        Debug.DrawLine(fastCar.gameObject.transform.position, Vector3.zero, Color.red);

        if (distanceBetweenCars < 0)
        {
            distanceBetweenCars *= -1;
        }

        var timeToCollision = distanceBetweenCars / relativeSpeed;
        var distanceSlowCarWillCoverBeforeCollision = slowCar.carSpeed * timeToCollision;

        debugStr += " D: " + distanceBetweenCars;
        debugStr += " T: " + timeToCollision;

        var positionOfCollider = transform.localPosition.y + collider.offset.y;
        var positionOfSlowCar = slowCar.transform.localPosition.y;

        var posOfSlowCarDestination = positionOfSlowCar - distanceSlowCarWillCoverBeforeCollision;

        debugStr += " PC: " + positionOfCollider + "  PCar: " + positionOfSlowCar + " di: " + distanceSlowCarWillCoverBeforeCollision + " de: " + posOfSlowCarDestination;

        if (posOfSlowCarDestination > positionOfCollider)
        {
            debugStr += " true";
            //Debug.Log(debugStr);
            return true;
        }
        //Debug.Log(debugStr);
        return false;
    }

    public bool WillCarsCollideOnScreen(Enemy fastCar, Enemy slowCar)
    {
        string debugStr;
        car1 = fastCar;
        car2 = slowCar;

        var relativeSpeed = fastCar.carSpeed - slowCar.carSpeed;
        debugStr = "FC: " + fastCar.carSpeed + " SC: " + slowCar.carSpeed;

        if (relativeSpeed <= 0)
        {
            return false;
        }

        var distanceBetweenCars = fastCar.gameObject.transform.localPosition.y - slowCar.gameObject.transform.localPosition.y;
        if (distanceBetweenCars < 0)
        {
            distanceBetweenCars *= -1;
        }

        var timeToCollision = distanceBetweenCars / relativeSpeed;
        var distanceSlowCarWillCoverBeforeCollision = slowCar.carSpeed * timeToCollision;

        debugStr += " D: " + distanceBetweenCars;
        debugStr += " T: " + timeToCollision;

        var positionOfCollider = transform.localPosition.y + collider.offset.y;
        var positionOfSlowCar = slowCar.transform.localPosition.y;

        var posOfSlowCarDestination = positionOfSlowCar - distanceSlowCarWillCoverBeforeCollision;

        debugStr += " PC: " + positionOfCollider + "  PCar: " + positionOfSlowCar + " di: " + distanceSlowCarWillCoverBeforeCollision + " de: " + posOfSlowCarDestination;

        if (posOfSlowCarDestination > positionOfCollider)
        {
            debugStr += " true";
            return true;
        }
        return false;
    }
}
