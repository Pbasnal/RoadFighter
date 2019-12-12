public class PlayerMovementController
{
    private IMove moveableObject;

    public PlayerMovementController(IMove moveableObject)
    {
        this.moveableObject = moveableObject;
    }

    public int MoveLeft()
    {
        if (moveableObject.PreviousPositionInUnits < moveableObject.MovementRange * -1)
        {
            moveableObject.CurrentPositionInUnits = moveableObject.PreviousPositionInUnits;
            return 0;
        }
        moveableObject.PreviousPositionInUnits = moveableObject.CurrentPositionInUnits;
        moveableObject.CurrentPositionInUnits -= moveableObject.MoveUnits;
        return -1;
    }

    public int MoveRight()
    {
        if (moveableObject.PreviousPositionInUnits > moveableObject.MovementRange)
        {
            moveableObject.CurrentPositionInUnits = moveableObject.PreviousPositionInUnits;
            return 0;
        }
        moveableObject.PreviousPositionInUnits = moveableObject.CurrentPositionInUnits;
        moveableObject.CurrentPositionInUnits += moveableObject.MoveUnits;

        return 1;
    }
}