namespace DesignPatterns.Visitor;

internal sealed class PrintToConsoleUseCases
{
    internal void Print(IEnumerable<IFigure> figures)
    {
        foreach (var figure in figures)
        {
            figure.Accept(PrintToConsoleVisitor.Instance);
            Console.WriteLine();
        }
    }

    private sealed class PrintToConsoleVisitor : IFigureVisitor<int>
    {
        public static PrintToConsoleVisitor Instance { get; } = new();

        public int Visit(Circle circle)
        {
            double thickness = 0.4;
            double rIn = circle.R - thickness, rOut = circle.R + thickness;

            for (int y = -circle.R; y <= circle.R; y++)
            {
                for (int x = -circle.R; x <= circle.R; x++)
                {
                    double value = x * x + y * y;
                    if (value >= rIn * rIn && value <= rOut * rOut)
                        Console.Write('*');
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }

            return 0;
        }

        public int Visit(Rectangle rectangle)
        {
            for (int y = 0; y < rectangle.A; y++)
            {
                for (int x = 0; x < rectangle.B; x++)
                {
                    Console.Write('*');
                }
                Console.WriteLine();
            }

            return 0;
        }

        public int Visit(Triangle triangle)
        {
            for (int y = 0; y < triangle.A; y++)
            {
                for (int x = 0; x <= triangle.B; x++)
                {
                    int currentWidth = (triangle.B * (y + 1)) / triangle.A;
                    if (x == 0 || y == triangle.A - 1 || x == currentWidth)
                        Console.Write('*');
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
            return 0;
        }
    }
}