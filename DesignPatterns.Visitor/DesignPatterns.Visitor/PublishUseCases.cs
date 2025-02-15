namespace DesignPatterns.Visitor;

internal sealed class PublishUseCases
{
    internal async Task PublishAsync(IEnumerable<IFigure> figures, CancellationToken ct)
    {
        foreach (var figure in figures)
        {
            await figure.Accept(new PublishAsyncVisitor(ct));
        }
    }

    private sealed class PublishAsyncVisitor(CancellationToken ct) : IFigureVisitor<Task>
    {
        public async Task Visit(Circle circle)
        {
            await Task.Delay(1, ct);
        }

        public async Task Visit(Rectangle rectangle)
        {
            await Task.Delay(1, ct);
        }

        public async Task Visit(Triangle triangle)
        {
            await Task.Delay(1, ct);
        }
    }
}