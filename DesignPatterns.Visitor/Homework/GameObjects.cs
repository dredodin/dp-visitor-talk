namespace Homework;

public interface IGameObject
{
    int X { get; set; }
    int Y { get; set; }
}

public record Fruit : IGameObject
{
    public int X { get; set; }
    public int Y { get; set; }
}

public record Monster(int Strength) : IGameObject
{
    public int X { get; set; }
    public int Y { get; set; }
}

//public record Obstacle : IGameObject
//{
//    public int X { get; set; }
//    public int Y { get; set; }
//}
