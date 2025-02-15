﻿namespace DesignPatterns.Visitor;

internal sealed class PrintToConsoleUseCases
{
    internal void Print(IEnumerable<IFigure> figures)
    {
        foreach (var figure in figures)
        {
            Print(figure);
            Console.WriteLine();
        }
    }

    internal void Print(IFigure figure)
    {
        _ = figure switch
        {
            Circle circle => PrintCircle(circle),
            Rectangle rectangle => PrintRectangle(rectangle),
            _ => throw new NotImplementedException()
        };
    }

    private static int PrintCircle(Circle circle)
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

    private static int PrintRectangle(Rectangle rectangle)
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
}