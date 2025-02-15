namespace DesignPatterns.Visitor;

public interface IFigureVisitor<T>
{
    T Visit(Circle circle);
    T Visit(Rectangle rectangle);
    T Visit(Triangle triangle);
}

public interface IFigure
{
    T Accept<T>(IFigureVisitor<T> visitor);
}

public sealed record Circle(int R) : IFigure
{
    public T Accept<T>(IFigureVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record Rectangle(int A, int B) : IFigure
{
    public T Accept<T>(IFigureVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record Triangle(int A, int B, int C) : IFigure
{
    public T Accept<T>(IFigureVisitor<T> visitor) => visitor.Visit(this);
}
