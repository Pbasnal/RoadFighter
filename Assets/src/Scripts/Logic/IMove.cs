public interface IMove
{
    int MoveUnits { get; set; }
    int MovementRange { get; set; }
    int MoveSpeed { get; set; }
    int CurrentPositionInUnits { get; set; }
    int PreviousPositionInUnits { get; set; }
}