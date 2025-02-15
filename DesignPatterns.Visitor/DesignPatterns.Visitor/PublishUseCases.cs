namespace DesignPatterns.Visitor;

internal sealed class PublishUseCases
{
    internal async Task PublishAsync(IEnumerable<IFigure> figures, CancellationToken ct)
    {
        foreach (var figure in figures)
        {
            await figure.Accept(PublishAsyncVisitor.Instance)(ct);
        }
    }

    private sealed class PublishAsyncVisitor : IFigureVisitor<Func<CancellationToken, Task>>
    {
        public static PublishAsyncVisitor Instance { get; } = new();

        public Func<CancellationToken, Task> Visit(Circle circle) => async ct =>
        {
            await Task.Delay(1, ct);
        };

        public Func<CancellationToken, Task> Visit(Rectangle rectangle) => async ct =>
        {
            await Task.Delay(1, ct);
        };

        public Func<CancellationToken, Task> Visit(Triangle triangle) => async ct =>
        {
            await Task.Delay(1, ct);
        };
    }
}