namespace Assets.Scripts.UnityLogic.ScriptableObjects
{
    public enum Direction
    {
        Left = -1,
        None = 0,
        Right = 1
    }

    //public class Direction
    //{
    //    public static Direction Left => new Left();
    //    public static Direction Right => new Right();
    //    public static Direction None => new None();

    //    protected int value { get; set; }

    //    public static implicit operator Direction(int i)
    //    {
    //        return new Direction { value = i };
    //    }

    //    public static implicit operator int(Direction dir)
    //    {
    //        return dir.value;
    //    }
    //}

    //public class Left : Direction
    //{
    //    public Left() => value = -1;
    //}

    //public class Right: Direction
    //{
    //    public Right() => value = 1;
    //}

    //public class None : Direction
    //{
    //    public None() => value = 0;
    //}
}
