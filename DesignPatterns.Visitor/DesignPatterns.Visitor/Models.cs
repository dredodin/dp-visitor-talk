namespace DesignPatterns.Visitor;

public interface IFigure;

public sealed record Circle(int R) : IFigure;

public sealed record Rectangle(int A, int B):IFigure;
